using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        
        public void StartGame()
        {
            if (_loading) return;
            FadeScreen().Forget();
        }

        private async UniTask FadeScreen()
        {
            _loading = true;
            await screenFade.DOFade(1, 1);
            await SceneManager.LoadSceneAsync(sceneName);
            _loading = false;
        }
    }
}
