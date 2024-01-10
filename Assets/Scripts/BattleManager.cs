using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using BattlePong;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Freya;
using SaturnRPG.Utilities.Extensions;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
	[SerializeField]
	private ScoreTrigger player1ScoreTrigger, player2ScoreTrigger;

	[SerializeField]
	private IntReference player1Score, player2Score;

	[SerializeField]
	private PongSettings pongSettings;

	[FormerlySerializedAs("scoreDisplay")]
	[SerializeField]
	private ScoreDisplayManager scoreDisplayManager;

	[SerializeField]
	private MatchCountdown matchCountdown;

	[SerializeField]
	private Image screenFade;

	[SerializeField]
	private GameResetter gameResetter;

	[SerializeField]
	private ParticleSystem scoreParticleSystem;

	[SerializeField]
	private SpriteRenderer scoreCone;

	[SerializeField]
	private BallScript ballScript;

	[SerializeField]
	private FloatReference matchTimer;

	public GameState GameState { get; private set; } = GameState.Start;
	public event Action<GameState> OnChangeGameState;

	private void OnEnable()
	{
		player1ScoreTrigger.OnScore += Player1Score;
		player2ScoreTrigger.OnScore += Player2Score;

		StartRoutine().Forget();
	}

	private void OnDisable()
	{
		player1ScoreTrigger.OnScore -= Player1Score;
		player2ScoreTrigger.OnScore -= Player2Score;
	}

	private void Update()
	{
		if (GameState == GameState.InMotion)
			matchTimer.Value += Time.deltaTime;
	}

	private CancellationToken DestroyToken => this.GetCancellationTokenOnDestroy();

	private async UniTask StartRoutine()
	{
		screenFade.color = screenFade.color.WithAlpha(1);
		GameState = GameState.Start;
		OnChangeGameState?.Invoke(GameState);
		player1Score.Value = player2Score.Value = 0;
		matchTimer.Value = 0;

		await screenFade.DOFade(0, 1).ToUniTask(cancellationToken: DestroyToken);
		await matchCountdown.CountdownFromStart(cancellationToken: DestroyToken);
		// await UniTask.Delay(5_000);

		ballScript.StartRandom();
		GameState = GameState.InMotion;
		OnChangeGameState?.Invoke(GameState);
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
		GameState = GameState.Score;
		OnChangeGameState?.Invoke(GameState);
		// collision2D.contacts[0].normalImpulse
		ballScript.gameObject.SetActive(false);
		await PerformParticleSystem(hit2D);

		await scoreDisplayManager.DisplayScore(player1Score.Value, player2Score.Value, pongSettings.FirstToN, p1Scored, cancellationToken: DestroyToken);
		if (p1Scored)
			player1Score.Value++;
		else
			player2Score.Value++;

		if (player1Score.Value == pongSettings.FirstToN || player2Score.Value == pongSettings.FirstToN)
		{
			// Exit to main menu
			await screenFade.DOFade(1, 2).ToUniTask(cancellationToken: DestroyToken);
			player1Score.Value = player2Score.Value = 0;
			matchTimer.Value = 0;
			
			await SceneManager.LoadSceneAsync("TitleScene");
			return;
		}
		else
		{
			await gameResetter.ResetSet();
		}
		
		ballScript.ResetBall();
		ballScript.gameObject.SetActive(true);
		ballScript.SetDirection(Vector2.zero);
		await UniTask.Delay(2_000, cancellationToken: DestroyToken);
		
		ballScript.SetDirection(p1Scored ? Vector2.up : Vector2.down);
		GameState = GameState.InMotion;
		OnChangeGameState?.Invoke(GameState);
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

		await UniTask.Delay((int)(scoreParticleSystem.main.duration * 2 * 1000), cancellationToken: DestroyToken);
		// yield return new WaitForSeconds(scoreParticleSystem.main.duration * 2);
		scoreParticleSystem.gameObject.SetActive(false);
	}
}