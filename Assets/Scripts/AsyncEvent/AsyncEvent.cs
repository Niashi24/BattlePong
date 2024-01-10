
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace SaturnRPG.Utilities
{
	public class AsyncEvent
	{
		private readonly HashSet<Func<UniTask>> _callbacks = new();

		public void Subscribe(Func<UniTask> callback)
		{
			_callbacks.Add(callback);
		}

		public void Unsubscribe(Func<UniTask> callback)
		{
			_callbacks.Remove(callback);
		}

		public async UniTask Invoke()
		{
			var tasks = _callbacks
				.Select(x => x.Invoke())
				.ToArray();

			await UniTask.WhenAll(tasks);
		}

		public void SubUnsub(bool subscribe, Func<UniTask> callback)
		{
			if (subscribe)
				Subscribe(callback);
			else
				Unsubscribe(callback);
		}
	}
	
	public class AsyncEvent<T>
	{
		private readonly HashSet<Func<T, UniTask>> _callbacks = new();

		public void Subscribe(Func<T, UniTask> callback)
		{
			_callbacks.Add(callback);
		}

		public void Unsubscribe(Func<T, UniTask> callback)
		{
			_callbacks.Remove(callback);
		}

		public async UniTask Invoke(T value)
		{
			var tasks = _callbacks
				.Select(x => x.Invoke(value))
				.ToArray();

			await UniTask.WhenAll(tasks);
		}

		public void SubUnsub(bool subscribe, Func<T, UniTask> callback)
		{
			if (subscribe)
				Subscribe(callback);
			else
				Unsubscribe(callback);
		}
	}
	
	public class AsyncEvent<T1, T2>
	{
		private readonly HashSet<Func<T1, T2, UniTask>> _callbacks = new();

		public void Subscribe(Func<T1, T2, UniTask> callback)
		{
			_callbacks.Add(callback);
		}

		public void Unsubscribe(Func<T1, T2, UniTask> callback)
		{
			_callbacks.Remove(callback);
		}

		public async UniTask Invoke(T1 value1, T2 value2)
		{
			var tasks = _callbacks
				.Select(x => x.Invoke(value1, value2))
				.ToArray();

			await UniTask.WhenAll(tasks);
		}

		public void SubUnsub(bool subscribe, Func<T1, T2, UniTask> callback)
		{
			if (subscribe)
				Subscribe(callback);
			else
				Unsubscribe(callback);
		}
	}
	
	public class AsyncEvent<T1, T2, T3>
	{
		private readonly HashSet<Func<T1, T2, T3, UniTask>> _callbacks = new();

		public void Subscribe(Func<T1, T2, T3, UniTask> callback)
		{
			_callbacks.Add(callback);
		}

		public void Unsubscribe(Func<T1, T2, T3, UniTask> callback)
		{
			_callbacks.Remove(callback);
		}

		public async UniTask Invoke(T1 value1, T2 value2, T3 value3)
		{
			var tasks = _callbacks
				.Select(x => x.Invoke(value1, value2, value3))
				.ToArray();

			await UniTask.WhenAll(tasks);
		}

		public void SubUnsub(bool subscribe, Func<T1, T2, T3, UniTask> callback)
		{
			if (subscribe)
				Subscribe(callback);
			else
				Unsubscribe(callback);
		}
	}
}