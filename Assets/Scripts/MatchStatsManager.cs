using System;
using UnityEngine;

namespace BattlePong
{
	public class MatchStatsManager : MonoBehaviour
	{
		[SerializeField]
		private MatchStats matchStats1, matchStats2;

		[SerializeField]
		private PongSettings currentSettingsRef;

		private void OnEnable()
		{
			matchStats1.SetFirstTo(currentSettingsRef.FirstToN);
			matchStats2.SetFirstTo(currentSettingsRef.FirstToN);
		}

		public void SetTimer(float timer)
		{
			var ts = GetTimeString(timer);
			matchStats1.SetTimer(ts);
			matchStats2.SetTimer(ts);
		}

		private string GetTimeString(float timer)
		{
			float minutes = Mathf.Floor(timer / 60);
			timer -= minutes * 60;
			string mS = minutes != 0 ? $"{minutes}:" : "";

			float seconds = Mathf.Floor(timer);
			timer -= seconds;
			string sS = seconds.ToString();
			if (minutes != 0)
				sS = sS.PadLeft(2, '0');

			const int numDecimals = 2;
			int decimals = (int)Math.Floor(timer * (int)Math.Pow(10, numDecimals));
			string dS = decimals.ToString().PadLeft(numDecimals, '0');

			return $"{mS}{sS}.{dS}";

		}
	}
}