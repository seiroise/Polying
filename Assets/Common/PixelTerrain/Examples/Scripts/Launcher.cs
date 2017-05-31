using UnityEngine;
using System;

namespace Common.PixelTerrain {

	/// <summary>
	/// 発射機
	/// </summary>
	public class Launcher : MonoBehaviour {

		[Header("Bullet Parameter")]
		[SerializeField]
		private Bullet _bullet;
		[SerializeField]
		private Transform _shotPos;
		[SerializeField, Range(0f, 1000f)]
		private float _initVelocity = 10f;

		/// <summary>
		/// 弾を発射
		/// </summary>
		public void Shot() {
			var bullet = Instantiate<Bullet>(_bullet);
			bullet.transform.position = _shotPos.position;
			bullet.transform.rotation = transform.rotation;
			bullet.SetVelocity(_initVelocity);
		}
	}
}