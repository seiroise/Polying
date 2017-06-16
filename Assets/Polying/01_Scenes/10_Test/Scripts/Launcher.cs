using UnityEngine;
using System.Collections;

namespace Polying.Test {

	/// <summary>
	/// ランチャー
	/// </summary>
	public class Launcher : MonoBehaviour {

		[SerializeField]
		private Rigidbody2D _bullet;
		[SerializeField, Range(1f, 3000f)]
		private float _shotForce = 500f;

		/// <summary>
		/// 発射
		/// </summary>
		public void Shot() {
			var b = Instantiate<Rigidbody2D>(_bullet);
			b.position = transform.position + transform.right * 1f;
			b.AddForce(transform.right * _shotForce);
		}
	}
}