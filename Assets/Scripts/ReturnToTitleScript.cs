using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BattlePong
{
    public class ReturnToTitleScript : MonoBehaviour
    {
        [SerializeField]
        private Image screenFade;

        [SerializeField]
        private GameObject pausePanel;
        
        public void ReturnToTitle()
        {
            ReturnToTitleRoutine(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTask ReturnToTitleRoutine(CancellationToken cancellationToken)
        {
            pausePanel.SetActive(false);
            await screenFade.DOFade(1, 1).SetUpdate(true).ToUniTask(cancellationToken: cancellationToken);
            await SceneManager.LoadSceneAsync("TitleScene");
            Time.timeScale = 1;
        }
    }
}
