using UnityEngine;
using Object = UnityEngine.Object;

namespace SaturnRPG.Utilities.Extensions
{
	public static class ObjectExtensions
	{
		public static T Log<T>(this T obj, string prefix = "")
		{
			Debug.Log(prefix + obj);
			return obj;
		}

		public static T Log<T>(this T obj, Object context, string prefix = "")
		{
			Debug.Log(prefix + obj, context);
			return obj;
		}

		public static void DebugCircle(Vector2 position, float radius, Color color, float duration)
		{
			foreach (var angle in 0f.StepTo(Mathf.PI * 2f, 10))
			{
				Debug.DrawRay(position, angle.AngleToDirection() * radius, color, duration);
			}
		}
	}
}