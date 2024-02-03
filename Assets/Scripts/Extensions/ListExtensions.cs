using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Random = System.Random;

namespace SaturnRPG.Utilities.Extensions
{
	public static class ListExtensions
	{
		// Taken from https://stackoverflow.com/questions/273313/randomize-a-listt

		private static readonly Random _rng = new Random();

		/// <summary>
		/// Shuffles a list in-place according to the Fisher-Yates Shuffle.
		/// </summary>
		/// <param name="list">The list to shuffle.</param>
		public static void Shuffle(this IList list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = _rng.Next(n + 1);
				(list[k], list[n]) = (list[n], list[k]);
			}
		}

		/// <summary>
		/// Returns a bool representing if the given index is within the range of the list ([0, list.Count))
		/// </summary>
		/// <param name="list">The list to check.</param>
		/// <param name="index">The index to check.</param>
		/// <returns></returns>
		public static bool IsInRange(this IList list, int index)
		{
			return index >= 0 && index < list.Count;
		}

		/// <summary>
		/// Returns the item at the given index in the list if the index is within the range of the list ([0, list.Count)).
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="index">The index of the item to get.</param>
		/// <typeparam name="T">The type of the items in the list.</typeparam>
		/// <returns>The item at the given index if it exists. Returns the default value of an item if it is not within the range.</returns>
		public static T GetIfInRange<T>(this IList<T> list, int index)
		{
			return index >= 0 && index < list.Count ? list[index] : default;
		}

		/// <summary>
		/// Filters items in the list in-place such that only items that return true in the predicate will be kept.
		/// </summary>
		/// <param name="list">The list to filter.</param>
		/// <param name="predicate">The predicate function that determines if the item is kept. Should return 'true' if kept, 'false' if removed.</param>
		/// <typeparam name="T">The type of the items in the list.</typeparam>
		public static void Retain<T>(this IList<T> list, [NotNull] Func<T, bool> predicate)
		{
			for (int i = 0; i < list.Count; i++)
				if (!predicate(list[i]))
					list.RemoveAt(i--);
		}

		/// <summary>
		/// Pops off the item at the end of the list (like a stack).
		/// </summary>
		/// <param name="list">The list to pop off.</param>
		/// <typeparam name="T">The type of the items in the list.</typeparam>
		/// <returns>The item at the end of the list, if it exists. Returns 'default' if the list is empty.</returns>
		public static T PopEnd<T>(this IList<T> list)
		{
			if (list.Count == 0) return default;

			var item = list[^1];
			list.RemoveAt(list.Count - 1);
			return item;
		}

		/// <summary>
		/// Returns the index of the first item that passes the predicate.
		/// </summary>
		/// <param name="list">The list to search through.</param>
		/// <param name="predicate">The predicate function. Should return 'true' if it passes the search query.</param>
		/// <typeparam name="T">The type of the items in the list.</typeparam>
		/// <returns>The index of the first item that passes the predicate. Returns -1 if none pass.</returns>
		public static int FirstIndexWhere<T>(this IList<T> list, [NotNull] Func<T, bool> predicate)
		{
			for (int i = 0; i < list.Count; i++)
				if (predicate(list[i]))
					return i;

			return -1;
		}

		/// <summary>
		/// Returns the index of the last item that passes the predicate.
		/// </summary>
		/// <param name="list">The list to search through.</param>
		/// <param name="predicate">The predicate function. Should return 'true' if it passes the search query.</param>
		/// <typeparam name="T">The type of the items in the list.</typeparam>
		/// <returns>The index of the last item that passes the predicate. Returns -1 if none pass.</returns>
		public static int LastIndexWhere<T>(this IList<T> list, [NotNull] Func<T, bool> predicate)
		{
			for (int i = list.Count - 1; i >= 0; i--)
				if (predicate(list[i]))
					return i;

			return -1;
		}

		/// <summary>
		/// Returns the first item in the Enumerable that passes the predicate, if any exist.
		/// </summary>
		/// <param name="enumerable">The enumerable to search through.</param>
		/// <param name="predicate">The predicate function. Should return 'true' if it passes the search query.</param>
		/// <typeparam name="T">The type of the items in the IEnumerable</typeparam>
		/// <returns>The first item in the Enumerable that passes the predicate. If none exist, it returns the 'default' value of T.</returns>
		public static T FirstWhere<T>(this IEnumerable<T> enumerable, [NotNull] Func<T, bool> predicate)
		{
			foreach (var item in enumerable)
				if (predicate(item))
					return item;

			return default;
		}

		/// <summary>
		/// Gets the first item in the enumerable that passes the predicate, if any exist.
		/// </summary>
		/// <param name="enumerable">The enumerable to search through</param>
		/// <param name="predicate">The predicate function. Should return 'true' if it passes the search query.</param>
		/// <param name="first">The output argument that will contain the item or 'default'.</param>
		/// <typeparam name="T">The type of the item in the enumerable</typeparam>
		/// <returns>Returns 'true' if any item exists that passes the predicate, 'false' if it does not.</returns>
		public static bool TryFirstWhere<T>(this IEnumerable<T> enumerable, [NotNull] Func<T, bool> predicate,
			out T first)
		{
			foreach (var item in enumerable)
			{
				if (!predicate(item)) continue;

				first = item;
				return true;
			}

			first = default;
			return false;
		}

		/// <summary>
		/// Gets the first item in the enumerable that passes the predicate.
		/// If no such item exists, it gets the last item in the enumerable.
		/// If the enumerable is empty, it returns the default value for the type.
		/// </summary>
		/// <param name="enumerable">The enumerable to search through</param>
		/// <param name="predicate">The predicate function. Should return 'true' if it passes the search query.</param>
		/// <typeparam name="T">The type of the item in the enumerable</typeparam>
		/// <returns>
		/// If there are no items in the enumerable it returns the default value for the type.
		/// If there are no items that pass the predicate, it returns the last item.
		/// Otherwise, it returns the first item that passes the predicate.
		/// </returns>
		public static T FirstWhereOrLast<T>(this IEnumerable<T> enumerable, [NotNull] Func<T, bool> predicate)
		{
			T prev = default;
			foreach (var item in enumerable)
			{
				if (predicate(item)) return item;
				prev = item;
			}

			return prev;
		}

		/// <summary>
		/// Gets the first item in the enumerable that passes the predicate.
		/// If no such item exists, it gets the last item in the enumerable.
		/// If the enumerable is empty, it returns the given default value.
		/// </summary>
		/// <param name="enumerable">The enumerable to search through</param>
		/// <param name="predicate">The predicate function. Should return 'true' if it passes the search query.</param>
		/// <typeparam name="T">The type of the item in the enumerable</typeparam>
		/// <returns>
		/// If there are no items in the enumerable it returns the given default value.
		/// If there are no items that pass the predicate, it returns the last item.
		/// Otherwise, it returns the first item that passes the predicate.
		/// </returns>
		public static T FirstWhereOrLastOrDefault<T>(this IEnumerable<T> enumerable, [NotNull] Func<T, bool> predicate, T _default)
		{
			T prev = default;
			bool any = false;
			foreach (var item in enumerable)
			{
				if (predicate(item)) return item;
				any = true;
				prev = item;
			}
			
			if (any)
				return prev;
			return _default;
		}

		public static void ForEach<T>(this IEnumerable<T> enumerable, [NotNull] Action<T> action)
		{
			foreach (var item in enumerable)
				action(item);
		}
	}
}