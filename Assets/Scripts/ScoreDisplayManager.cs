using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Freya;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BattlePong
{
	public class ScoreDisplayManager : MonoBehaviour
	{
		// [SerializeField]
		// private Transform p1Display, p2Display;

		[SerializeField]
		private ScoreDisplay p1Display, p2Display;

		[SerializeField]
		private Image scoreBG;

		// [SerializeField]
		// private TMP_Text p1Score1, p1Score2, p2Score1, p2Score2;
		//
		// [SerializeField]
		// private TMP_Text p1Result, p2Result;

		public async UniTask DisplayScore(int p1Score, int p2Score, int scoreToWin, bool p1Scored,
			CancellationToken cancellationToken)
		{
			await scoreBG.DOColor(scoreBG.color.WithAlpha(0.5f), 0.5f).ToUniTask(cancellationToken: cancellationToken);

			p1Display.gameObject.SetActive(true);
			p2Display.gameObject.SetActive(true);

			p1Display.SetScore(p1Score, p2Score);
			p2Display.SetScore(p2Score, p1Score);

			p1Display.SetAlpha(0);
			p2Display.SetAlpha(0);
			var p1Transform = p1Display.transform;
			var p2Transform = p2Display.transform;

			await UniTask.WhenAll(
				p1Transform.DOLocalMoveX(0, 0.5f).ToUniTask(cancellationToken: cancellationToken),
				p1Display.TweenAlpha(1, 0.5f, cancellationToken: cancellationToken),
				p2Transform.DOLocalMoveX(0, 0.5f).ToUniTask(cancellationToken: cancellationToken),
				p2Display.TweenAlpha(1, 0.5f, cancellationToken: cancellationToken)
			);
			await UniTask.Delay(1000, cancellationToken: cancellationToken);

			if (p1Scored)
				p1Score++;
			else
				p2Score++;

			p1Display.SetScore(p1Score, p2Score);
			p2Display.SetScore(p2Score, p1Score);
			await UniTask.Delay(1000, cancellationToken: cancellationToken);

			if (p1Scored && p1Score == scoreToWin)
			{
				SetVictory(true);
				await UniTask.WhenAll(
					p1Display.TweenVictoryAlpha(1, 0.5f, cancellationToken: cancellationToken),
					p2Display.TweenVictoryAlpha(1, 0.5f, cancellationToken: cancellationToken)
				);
				await UniTask.Delay(5_000, cancellationToken: cancellationToken);
			}
			else if (!p1Scored && p2Score == scoreToWin)
			{
				SetVictory(false);
				await UniTask.WhenAll(
					p1Display.TweenVictoryAlpha(1, 0.5f, cancellationToken: cancellationToken),
					p2Display.TweenVictoryAlpha(1, 0.5f, cancellationToken: cancellationToken)
				);
				await UniTask.Delay(5_000, cancellationToken: cancellationToken);
			}

			await UniTask.WhenAll(
				p1Transform.DOLocalMoveX(p1Transform.right.x * 300, 0.5f)
					.ToUniTask(cancellationToken: cancellationToken),
				p1Display.TweenAlpha(0, 0.5f, cancellationToken: cancellationToken),
				p1Display.TweenVictoryAlpha(0, 0.5f, cancellationToken: cancellationToken),
				p2Transform.DOLocalMoveX(p2Transform.right.x * 300, 0.5f)
					.ToUniTask(cancellationToken: cancellationToken),
				p2Display.TweenAlpha(0, 0.5f, cancellationToken: cancellationToken),
				p2Display.TweenVictoryAlpha(0, 0.5f, cancellationToken: cancellationToken));

			p1Display.gameObject.SetActive(false);
			p2Display.gameObject.SetActive(false);

			await scoreBG.DOColor(scoreBG.color.WithAlpha(0f), 0.5f).ToUniTask(cancellationToken: cancellationToken);
		}

		private void SetVictory(bool p1Wins)
		{
			p1Display.SetVictory(p1Wins);
			p2Display.SetVictory(!p1Wins);
		}
	}
}