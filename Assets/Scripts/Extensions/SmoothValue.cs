using UnityEngine;

namespace SaturnRPG.Utilities.Extensions
{
	[System.Serializable]
	public class SmoothValue
	{
		public float value;
		public float target;

		[SerializeField]
		private float smoothTime;

		private float _currentVelocity;

		public float Tick()
		{
			return value = Mathf.SmoothDamp(value, target, ref _currentVelocity, smoothTime);
		}
	}
}