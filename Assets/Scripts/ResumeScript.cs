using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BattlePong
{
	public class ResumeScript : MonoBehaviour
	{
		[SerializeField]
		private MatchCountdown matchCountdown;

		[SerializeField]
		private GameObject pausePanel;
		
		public void Resume()
		{
			ResumeRoutine(this.GetCancellationTokenOnDestroy()).Forget();
		}

		private async UniTask ResumeRoutine(CancellationToken cancellationToken)
		{
			pausePanel.SetActive(false);
			await matchCountdown.CountdownFromPause(cancellationToken);
			Time.timeScale = 1;
		}
	}
}