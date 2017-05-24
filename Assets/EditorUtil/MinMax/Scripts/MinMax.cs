using UnityEngine;
using System.Collections.Generic;

namespace EditorUtil {

	/// <summary>
	/// 最大値と最小値を保持するクラス
	/// </summary>
	[System.Serializable]
	public struct MinMax {

		[SerializeField]
		private float _min;		//最小値
		public float min { get { return _min; } set { _min = value; } }
		[SerializeField]
		private float _max;		//最大値
		public float max { get { return _max; } set { _min = value; } }

		public float random { get { return Random.Range(min, max); } }
		public int randomInt { get { return (int)Random.Range(min, max); } }

		public MinMax(float min, float max) {
			this._min = min;
			this._max = max;
		}

		/// <summary>
		/// 指定した値が最小値と最大値の範囲に収まるように修正して返す
		/// </summary>
		/// <returns>結果</returns>
		/// <param name="value">値</param>
		public float Clamp(float value) {
			return Mathf.Clamp(value, min, max);
		}
	}
}