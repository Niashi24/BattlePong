
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Freya;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TitleBattleManager: MonoBehaviour
{
    [SerializeField]
    private ScoreTrigger player1ScoreTrigger, player2ScoreTrigger;

    [SerializeField]
    private ParticleSystem scoreParticleSystem;

    [SerializeField]
    private SpriteRenderer scoreCone;

    [SerializeField]
    private BallScript ballScript;

    [SerializeField]
    private PaddleScript paddle1, paddle2;

    [SerializeField]
    private Image screenFade;

    private void OnEnable()
    {
        player1ScoreTrigger.OnScore += Player1Score;
        player2ScoreTrigger.OnScore += Player2Score;
        StartRoutine();
        FadeIn().Forget();
    }

    private void OnDisable()
    {
        player1ScoreTrigger.OnScore -= Player1Score;
        player2ScoreTrigger.OnScore -= Player2Score;
    }

    private CancellationToken DestroyToken => this.GetCancellationTokenOnDestroy();

    private void StartRoutine()
    {
        ballScript.StartRandom();
    }

    private async UniTask FadeIn()
    {
        screenFade.gameObject.SetActive(true);
        screenFade.color = screenFade.color.WithAlpha(1f);
        screenFade.raycastTarget = false;
        await screenFade.DOFade(0f, 1).ToUniTask(cancellationToken: DestroyToken);
        screenFade.gameObject.SetActive(false);
    }

    private void Player1Score(RaycastHit2D hit2D)
    {
        ScoreRoutine(hit2D, true).Forget();
    }

    private void Player2Score(RaycastHit2D hit2D)
    {
        ScoreRoutine(hit2D, false).Forget();
    }

    private async UniTask ScoreRoutine(RaycastHit2D hit2D, bool p1Scored)
    {
        ballScript.gameObject.SetActive(false);
        await PerformParticleSystem(hit2D);

        await ResetSet();
        
        ballScript.ResetBall();
        ballScript.SetDirection(Vector2.zero);
        ballScript.gameObject.SetActive(true);
        await UniTask.Delay(2_000, cancellationToken: DestroyToken);
        
        ballScript.SetDirection(p1Scored ? Vector2.up : Vector2.down);
    }

    private UniTask ResetSet()
    {
        paddle1.ResetCharge();
        paddle2.ResetCharge();
        return UniTask.WhenAll(
            paddle1.transform.DOLocalMoveX(0, 0.5f).ToUniTask(),
            paddle2.transform.DOLocalMoveX(0, 0.5f).ToUniTask()
        );
    }

    private async UniTask PerformParticleSystem(RaycastHit2D hit2D)
    {
        scoreParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        scoreParticleSystem.gameObject.SetActive(true);
		
        var mainModule = scoreParticleSystem.main;
        var mainModuleStartColor = mainModule.startColor;
        mainModuleStartColor.color = ballScript.Color;
        mainModule.startColor = mainModuleStartColor;
        scoreCone.color = ballScript.Color;
		
        scoreParticleSystem.transform.SetPositionAndRotation(hit2D.point,
            Quaternion.LookRotation(hit2D.normal, Vector3.up));
        scoreParticleSystem.Play();

        await UniTask.Delay((int)(scoreParticleSystem.main.duration * 2 * 1000),
            cancellationToken: DestroyToken);
        scoreParticleSystem.gameObject.SetActive(false);
    }
}