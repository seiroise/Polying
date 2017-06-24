using UnityEngine;
using System;

namespace Polying.Test {

	/// <summary>
	/// ランチャー
	/// </summary>
	public abstract class Launcher : MonoBehaviour {

		[Header("Parameter")]
		[SerializeField]
		protected bool _oneShot = true;
		[SerializeField, Range(0.01f, 60f)]
		protected float _shotInterval = 0.5f;
		[SerializeField, Range(1f, 60f)]
		protected float _rotation = 30f;
		[SerializeField]
		protected Ammo _useAmmo;

		[Header("Shake")]
		[SerializeField, Range(0.01f, 10f)]
		protected float _shakeForce = 1f;
		[SerializeField]
		protected ShakeTransform _shake;

		private ShooterEntity _owner;

		protected Vector3 _lookDirection;
		protected Vector3 _lookAngles;
		protected float _timer;
		protected bool _triggerd;

		private void Update() {
			if(_timer < _shotInterval) {
				_timer += Time.deltaTime;
			} else if(_triggerd){
				CheckAndLaunch();
			}
		}

		/// <summary>
		/// 確認と発射
		/// </summary>
		private void CheckAndLaunch() {
			if(_timer >= _shotInterval && _owner.UseAmmo(_useAmmo)) {
				Launch();
			}
		}

		/// <summary>
		/// 弾を発射する
		/// </summary>
		protected abstract void Launch();

		/// <summary>
		/// ランチャーの持ち主を設定する
		/// </summary>
		/// <value>The set owner.</value>
		public void SetOwner(ShooterEntity owner) {
			_owner = owner;
		}

		/// <summary>
		/// 指定した座標の方向を向く
		/// </summary>
		/// <param name="position">Position.</param>
		public void Look(Vector3 position) {
			_lookDirection = position - transform.position;
			_lookAngles.z = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg;
			transform.eulerAngles = _lookAngles;
		}

		/// <summary>
		/// トリガーを引く
		/// </summary>
		public void PullTrigger() {
			if(_oneShot) {
				CheckAndLaunch();
			} else {
				_triggerd = true;
			}
		}

		/// <summary>
		/// トリガーを押しもどす
		/// </summary>
		public void PushTrigger() {
			_triggerd = false;
		}
	}
}