using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SaturnRPG.Utilities.Extensions
{
	public static class VectorExtensions
	{
		public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
		{
			return new Vector3(
				x ?? original.x,
				y ?? original.y,
				z ?? original.z
			);
		}

		public static Vector3 DirectionTo(this Vector3 from, Vector3 to) => (to - from).normalized;

		/// <summary>
		/// Returns a vector containing all the elements of the original vector rounded to the nearest integer.
		/// </summary>
		/// <param name="original">The original Vector.</param>
		public static Vector3 Round(this Vector3 original)
		{
			return new Vector3(
				Mathf.Round(original.x),
				Mathf.Round(original.y),
				Mathf.Round(original.z)
			);
		}

		public static Vector3 RoundTo(this Vector3 original, float nearest)
			=> new(original.x.RoundTo(nearest), original.y.RoundTo(nearest), original.z.RoundTo(nearest));

		/// <summary>
		/// Returns a vector going in the same direction as the 'original' vector with the given magnitude.
		/// </summary>
		/// <param name="original">The original vector dictating the direction of the output vector.</param>
		/// <param name="magnitude">The magnitude of the output vector.</param>
		public static Vector3 WithMagnitude(this Vector3 original, float magnitude)
			=> original.normalized * magnitude;

		public static Vector3 Average(this IEnumerable<Vector3> enumerable)
		{
			Vector3 average = default;
			int count = 0;
			foreach (var item in enumerable)
			{
				count++;
				average += item;
			}

			if (count != 0) average /= count;

			return average;
		}

		public static Vector2 Average(this IEnumerable<Vector2> enumerable)
		{
			Vector2 average = default;
			int count = 0;
			foreach (var item in enumerable)
			{
				count++;
				average += item;
			}

			if (count != 0) average /= count;

			return average;
		}

		public static float Average(this IEnumerable<float> enumerable)
		{
			float average = default;
			int count = 0;
			foreach (var item in enumerable)
			{
				count++;
				average += item;
			}

			if (count != 0) average /= count;

			return average;
		}

		public static int Average(this IEnumerable<int> enumerable)
		{
			int average = default;
			int count = 0;
			foreach (var item in enumerable)
			{
				count++;
				average += item;
			}

			if (count != 0) average /= count;

			return average;
		}

		public static Bounds CalcBounds(this Vector3[] vertices)
		{
			if (vertices.Length == 0) return default;
			float minX, minY, maxX, maxY;

			minX = maxX = vertices[0].x;
			minY = maxY = vertices[0].y;

			for (int i = 1; i < vertices.Length; i++)
			{
				if (vertices[i].x < minX)
					minX = vertices[i].x;
				if (vertices[i].y < minY)
					minY = vertices[i].y;
				if (vertices[i].x > maxX)
					maxX = vertices[i].x;
				if (vertices[i].y > maxY)
					maxY = vertices[i].y;
			}

			return new Bounds
			{
				min = new Vector3(minX, minY, 0),
				max = new Vector3(maxX, maxY, 0)
			};
		}

		public static Vector2 DirectionTo(this Vector2 from, Vector2 to) => (to - from).normalized;

		public static Vector2 ToVector2(this Vector3 from) => new Vector2(from.x, from.y);

		public static Vector2 Rotate(this Vector2 original, float angleRad)
		{
			float sin = Mathf.Sin(angleRad);
			float cos = Mathf.Cos(angleRad);

			return new Vector2(original.x * cos - original.y * sin, original.x * sin + original.y * cos);
		}

		/// <summary>
		/// Projects the 'original' Vector2 along the 'newDirection' Vector.
		/// Note: assumes that the newDirection Vector is normalized
		/// </summary>
		public static Vector2 ProjectToDirection(this Vector2 original, Vector2 newDirection) 
			=> Vector2.Dot(original, newDirection) * newDirection;

		public static Vector2 With(this Vector2 original, float? x = null, float? y = null) =>
			new Vector2(x ?? original.x, y ?? original.y);

		public static Vector2 Round(this Vector2 original)
			=> new(Mathf.Round(original.x), Mathf.Round(original.y));

		public static Vector2 RoundTo(this Vector2 original, float nearest)
			=> new(original.x.RoundTo(nearest), original.y.RoundTo(nearest));

		public static float Angle(this Vector2 original)
			=> Mathf.Atan2(original.y, original.x);

		public static Vector2 ComponentMultiply(this Vector2 a, Vector2 b) => new(a.x * b.x, a.y * b.y);

		public static Vector2 ComponentDivide(this Vector2 a, Vector2 b) => new(a.x / b.x, a.y / b.y);

		public static Vector3 ToVector3(this Vector2 a, float z = 0) => new(a.x, a.y, z);

		public static Vector2 WithMagnitude(this Vector2 a, float magnitude) => a.normalized * magnitude;

		public static Vector2 Reflect(this Vector2 ray, Vector2 normal)
		{
			var u = Vector2.Dot(ray, normal) * normal;
			var w = ray - u;
			return w - u;
		}

		// Returns the intersection point of the vector lines
		// v = v1 + v2 * t and w = v3 + v4 * u
		public static Vector2 GetIntersectionPoint(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4)
		{
			float determinant = v2.x * v4.y - v4.x * v2.y;
			if (determinant == 0) return Vector2.zero;

			// Derived from setting v1 + v2*t = v3 + v4*u
			float t = (v4.x * (v1.y - v3.y) + v4.y * (v3.x - v1.x)) / determinant;

			return v1 + v2 * t;
		}

		public static (float, float) MinMax(this IEnumerable<float> enumerable)
		{
			bool any = false;
			float min = float.MaxValue, max = float.MinValue;
			foreach (float value in enumerable)
			{
				any = true;
				if (value > max) max = value;
				if (value < min) min = value;
			}

			return any ? (min, max) : (0, 0);
		}

		public static float Sign0(this float value)
		{
			if (value == 0) return 0;
			return Mathf.Sign(value);
		}

		public static float IncreaseAbs(this float value, float increase, float maxAbs = Mathf.Infinity)
		{
			return value.Sign0() * Mathf.Clamp(Mathf.Abs(value) + increase, 0, maxAbs);
		}

		public static float MaxAbs(this float value, float maxAbs)
		{
			return value.Sign0() * Mathf.Min(Math.Abs(value), maxAbs);
		}

		public static Vector2 AngleToDirection(this float angleRadians)
			=> new(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));

		// public static float DecreaseAbs(this float value, float dec)

		public static int Round(this float value)
		{
			return (int)Mathf.Round(value);
		}

		public static float RoundTo(this float value, float nearest)
		{
			return Mathf.Round(value / nearest) * nearest;
		}
		
		public static float Remap (this float value, float from1, float to1, float from2, float to2)
			=> (value - from1) / (to1 - from1) * (to2 - from2) + from2;

		public static IEnumerable<float> StepTo(this float start, float end, int steps)
		{
			if (steps <= 0) yield break;
			if (steps == 1) yield return start;
			else if (steps == 2)
			{
				yield return start;
				yield return end;
			}
			else
			{
				float stepsF = steps - 1;
				for (int i = 0; i < steps; i++)
				{
					yield return Mathf.Lerp(start, end, i / stepsF);
				}
			}
		}
	}
}