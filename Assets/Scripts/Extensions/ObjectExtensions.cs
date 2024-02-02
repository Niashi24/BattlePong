using UnityEngine;
using Object = UnityEngine.Object;

namespace SaturnRPG.Utilities.Extensions
{
	public static class ObjectExtensions
	{
		public static T Log<T>(this T obj)
		{
			Debug.Log(obj);
			return obj;
		}

		public static T Log<T>(this T obj, Object context)
		{
			Debug.Log(obj, context);
			return obj;
		}
	}
}