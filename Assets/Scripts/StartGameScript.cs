using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Freya;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BattlePong
{
    public class StartGameScript : MonoBehaviour
    {
        [SerializeField]
        private Image screenFade;

        [SerializeField]
        private string sceneName = "PVPBattle";

        private bool _loading = false;

        private CancellationToken _cancellationToken;

        private void Start()
        {
            _cancellationToken = this.GetCancellationTokenOnDestroy();
        }
        
        public void StartGame()
        {
            if (_loading) return;
            FadeScreen().Forget();
        }

        private async UniTask FadeScreen()
        {
            _loading = true;
            screenFade.gameObject.SetActive(true);
            screenFade.color = screenFade.color.WithAlpha(0f);
            screenFade.raycastTarget = true;
            await screenFade.DOFade(1, 1).ToUniTask(cancellationToken: _cancellationToken);
            await SceneManager.LoadSceneAsync(sceneName);
            _loading = false;
        }
    }
}
