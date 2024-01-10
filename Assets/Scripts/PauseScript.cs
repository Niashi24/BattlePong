using System;
using Freya;
using UnityEngine;
using UnityEngine.UI;

namespace BattlePong
{
	public class PauseScript : MonoBehaviour
	{
		[SerializeField]
		private GameObject pausePanel;

		[SerializeField]
		private Button pauseButton;

		[SerializeField]
		private Image[] pauseButtonImages;

		[SerializeField]
		private BattleManager battleManager;

		private void OnEnable()
		{
			battleManager.OnChangeGameState += AllowGameState;
		}

		private void OnDisable()
		{
			battleManager.OnChangeGameState += AllowGameState;
		}

		public void Pause()
		{
			if (Time.timeScale == 0) return;
			
			pausePanel.SetActive(true);
			Time.timeScale = 0;
		}

		private void AllowGameState(GameState gameState)
		{
			pauseButton.interactable = gameState == GameState.InMotion;
			foreach (var image in pauseButtonImages)
			{
				image.color = pauseButton.interactable ? Color.white : Color.gray;
			}
		}
	}
}