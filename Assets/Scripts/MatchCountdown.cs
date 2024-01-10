using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Freya;
using TMPro;
using UnityEngine;

public class MatchCountdown : MonoBehaviour
{
    [SerializeField]
    private TMP_Text countdownText1, countdownText2;
    
    public async UniTask CountdownFromStart(CancellationToken cancellationToken)
    {
        countdownText1.text = countdownText2.text = "READY...";
        await UniTask.WhenAll(
            countdownText1.DOFade(1, 1).ToUniTask(cancellationToken: cancellationToken),
            countdownText2.DOFade(1, 1).ToUniTask(cancellationToken: cancellationToken)
        );

        await UniTask.Delay(2_000);
        
        for (int i = 3; i > 0; i--)
        {
            countdownText1.text = countdownText2.text = i.ToString();
            await UniTask.Delay(1000, cancellationToken: cancellationToken);
        }

        countdownText1.text = countdownText2.text = "GO!";
        
        StartText(cancellationToken: cancellationToken).Forget();
    }

    public async UniTask CountdownFromPause(CancellationToken cancellationToken)
    {
        countdownText1.color = countdownText2.color = countdownText1.color.WithAlpha(1);
        
        for (int i = 3; i > 0; i--)
        {
            countdownText1.text = countdownText2.text = i.ToString();
            await UniTask.Delay(1000, ignoreTimeScale:true, cancellationToken: cancellationToken);
        }

        countdownText1.text = countdownText2.text = "0";
        StartText(cancellationToken: cancellationToken).Forget();
    }

    private async UniTask StartText(CancellationToken cancellationToken)
    {
        await UniTask.Delay(500, cancellationToken: cancellationToken);
        await UniTask.WhenAll(
            countdownText1.DOFade(0, 1).ToUniTask(cancellationToken: cancellationToken),
            countdownText2.DOFade(0, 1).ToUniTask(cancellationToken: cancellationToken)
        );
    }
}
