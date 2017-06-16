using UnityEngine;
using System.Collections;

namespace Polying.Test {

	/// <summary>
	/// Transformを揺らす
	/// </summary>
	public class ShakeTransform : MonoBehaviour {

		private Vector2 dir;

		private void Update() {
			
		}

		/// <summary>
		/// 揺らす
		/// </summary>
		/// <param name="force"></param>
		public void Shake(float force) {
			dir.x = Random.Range(-1f, 1f);
		}
	}
}