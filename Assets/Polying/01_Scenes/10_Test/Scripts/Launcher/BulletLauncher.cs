using UnityEngine;
using System.Collections;

namespace Polying.Test {

	/// <summary>
	/// バレットランチャー
	/// </summary>
	public class BulletLauncher : Launcher {

		[Header("Bullet")]
		[SerializeField]
		private Rigidbody2D _bullet;
		[SerializeField, Range(1f, 3000f)]
		private float _shotForce = 500f;

		/// <summary>
		/// 弾を発射する
		/// </summary>
		protected override void Launch() {
			if(_bullet) {
				var bullet = (GameObject)Instantiate(_bullet.gameObject, transform.position + transform.right, transform.rotation);
				var b = bullet.GetComponent<Rigidbody2D>();
				b.AddForce(transform.right * _shotForce);
				if(_shake) {
					_shake.Shake(_shakeForce, 20f, 0.2f);
				}
				_timer = 0f;
			}
		}
	}
}