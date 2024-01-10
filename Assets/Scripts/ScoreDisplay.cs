using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Freya;
using TMPro;
using UnityEngine;

namespace BattlePong
{
    public class ScoreDisplay : MonoBehaviour
    {
        [field: SerializeField]
        public TMP_Text Player1Text { get; private set; }
        [field: SerializeField]
        public TMP_Text Player2Text { get; private set; }
        [field: SerializeField]
        public TMP_Text DashText { get; private set; }
        [field: SerializeField]
        public TMP_Text VictoryText { get; private set; }

        public UniTask TweenAlpha(float alpha, float duration, CancellationToken cancellationToken)
        {
            return UniTask.WhenAll(
                Player1Text.DOFade(alpha, duration).ToUniTask(cancellationToken: cancellationToken),
                Player2Text.DOFade(alpha, duration).ToUniTask(cancellationToken: cancellationToken),
                DashText.DOFade(alpha, duration).ToUniTask(cancellationToken: cancellationToken)
            );
        }

        public void SetScore(int p1Score, int p2Score)
        {
            Player1Text.text = p1Score.ToString();
            Player2Text.text = p2Score.ToString();
        }

        public void SetAlpha(float alpha)
        {
            Player1Text.color = Player1Text.color.WithAlpha(alpha);
            Player2Text.color = Player2Text.color.WithAlpha(alpha);
            DashText.color = DashText.color.WithAlpha(alpha);
        }

        public UniTask TweenVictoryAlpha(float alpha, float duration, CancellationToken cancellationToken)
            => VictoryText.DOFade(alpha, duration).ToUniTask(cancellationToken: cancellationToken);

        public void SetVictory(bool won)
        {
            VictoryText.text = won ? "Victory" : "Defeat";
        }
    }
}
