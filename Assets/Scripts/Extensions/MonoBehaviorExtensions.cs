using UnityEngine;
using UnityEngine.Pool;

namespace SaturnRPG.Utilities.Extensions
{
	public static class MonoBehaviorExtensions
	{
		public static ObjectPool<T> CreateMonoPool<T>(this T prefab, Vector3? position = null, Quaternion? rotation = null, Transform parent = null)
			where T : Component
		{
			return new ObjectPool<T>(
				createFunc: () => GameObject.Instantiate<T>(
					prefab,
					position ?? Vector3.zero,
					rotation ?? Quaternion.identity,
					parent
				),
				actionOnGet: (x) => x.gameObject.SetActive(true),
				actionOnDestroy: (x) => GameObject.Destroy(x.gameObject),
				actionOnRelease: (x) => x.gameObject.SetActive(false)
			);
		}
	}
}