using UnityEngine;
using System;

namespace Common.PixelTerrain {

	/// <summary>
	/// Launcherから出る弾
	/// </summary>
	[RequireComponent(typeof(Rigidbody))]
	public class Bullet : MonoBehaviour {

		[SerializeField]
		private AnimationEffect _effect;
		[SerializeField, Range(0, 1000)]
		private int excavate = 30;
		[SerializeField, Range(0f, 250f)]
		private float excavateArea = 1f;

		private Rigidbody _rbody;

		private void Awake() {
			if(!_rbody) {
				_rbody = GetComponent<Rigidbody>();
			}
		}

		private void OnCollisionEnter(Collision co) {
			var terrain = PixelTerrain.instance;
			if(terrain) {
				terrain.ExcavateCircle(co.contacts[0].point, excavateArea, excavate);
			}
			if(_effect) {
				Instantiate(_effect, transform.position, transform.rotation);
			}
			Destroy(gameObject);
		}

		/// <summary>
		/// 速度を設定する
		/// </summary>
		/// <param name="velocity">速度</param>
		public void SetVelocity(float velocity) {
			if(!_rbody) {
				_rbody = GetComponent<Rigidbody>();
			}
			_rbody.velocity = transform.right * velocity;
		}
	}
}