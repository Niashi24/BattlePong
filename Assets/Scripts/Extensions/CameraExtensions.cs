using UnityEngine;

namespace SaturnRPG.Utilities.Extensions
{
	public static class CameraExtensions
	{
		public static Vector2 GetMouseWorldPosition(this Camera camera)
		{
			return camera.ScreenToWorldPoint(Input.mousePosition);
		}
	}
}