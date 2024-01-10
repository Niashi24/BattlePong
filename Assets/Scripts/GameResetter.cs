using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace BattlePong
{
	public class GameResetter : MonoBehaviour
	{
		[SerializeField]
		private PaddleScript paddleScript1, paddleScript2;

		[SerializeField]
		private IntReference p1Score, p2Score;

		[SerializeField]
		private FloatReference matchTimer;

		public async UniTask ResetGame()
		{
			await ResetSet();
			p1Score.Value = p2Score.Value = 0;
		}

		public async UniTask ResetSet()
		{
			matchTimer.Value = 0;
			await ResetPaddle();
		}

		private UniTask ResetPaddle()
		{
			paddleScript1.ResetCharge();
			paddleScript2.ResetCharge();
			return UniTask.WhenAll(paddleScript1.transform.DOMoveX(0, 0.5f).ToUniTask(), 
				paddleScript2.transform.DOMoveX(0, 0.5f).ToUniTask());
		}
	}
}