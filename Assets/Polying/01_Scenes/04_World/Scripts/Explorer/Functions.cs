using System;
using System.Collections.Generic;
using UnityEngine;

namespace Polying.World.Explorer {

	/// <summary>
	/// ユーティリティ関数
	/// </summary>
	public class Functions {

		/// <summary>
		/// 指定した範囲をstepごとに区切った値の配列を出力する
		/// </summary>
		/// <returns>The range.</returns>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		/// <param name="step">Step.</param>
		public static IEnumerable<float> FloatRange(float min, float max, float step) {
			for(float v = min; v <= max; v += step) {
				yield return v;
			}
		}

		/// <summary>
		/// 角度をベクトルに変換
		/// </summary>
		/// <returns>The to direction.</returns>
		/// <param name="deg">Angles.</param>
		public static Vector2 DegToVector(float deg) {
			return RadToVector(deg * Mathf.Deg2Rad);
		}

		/// <summary>
		/// 角度をベクトルに変換
		/// </summary>
		/// <returns>The to vector.</returns>
		/// <param name="rad">RAD.</param>
		public static Vector2 RadToVector(float rad) {
			return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
		}

		/// <summary>
		/// ベクトルを角度に変換
		/// </summary>
		/// <returns>The to angle.</returns>
		/// <param name="dir">Dir.</param>
		public static float VectorToDeg(Vector2 dir) {
			return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		}

		/// <summary>
		/// ベクトルを角度に変換
		/// </summary>
		/// <returns>The to angle.</returns>
		/// <param name="dir">Dir.</param>
		public static float VectorToRad(Vector2 dir) {
			return Mathf.Atan2(dir.y, dir.x);
		}

		/// <summary>
		/// 角度配列を方向ベクトル配列に変換
		/// </summary>
		/// <returns>The to direction.</returns>
		/// <param name="angles">Angles.</param>
		public static Vector2[] AngleToDirection(float[] angles) {
			var dirs = new Vector2[angles.Length];
			float theta;
			for(int i = 0; i < angles.Length; ++i) {
				dirs[i] = DegToVector(angles[i]);
			}
			return dirs;
		}
	}
}