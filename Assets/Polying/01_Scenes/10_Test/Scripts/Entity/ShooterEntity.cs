using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Polying.Test {

	/// <summary>
	/// 攻撃できる実態
	/// </summary>
	public class ShooterEntity : Entity {

		[Header("Ammo")]
		[SerializeField]
		private Ammo _ammo;
		[SerializeField]
		private Ammo _maxAmmo;

		[Header("Launcher")]
		[SerializeField]
		private List<Launcher> _launchers;

		private AmmoEvent _onChangedAmmo;
		private AmmoEvent _onNotEnoughAmmo;

		public Ammo ammo {
			get {
				return _ammo;
			}
		}
		public Ammo maxAmmo {
			get {
				return _maxAmmo;
			}
		}
		public AmmoEvent onChangedAmmo {
			get {
				return _onChangedAmmo;
			}
		}
		public AmmoEvent onNotEnoughAmmo {
			get {
				return _onNotEnoughAmmo;
			}
		}

		protected override void Awake() {
			base.Awake();
			_onChangedAmmo = new AmmoEvent();
			_onNotEnoughAmmo = new AmmoEvent();
			for(int i = 0; i < _launchers.Count; ++i) {
				_launchers[i].SetOwner(this);
			}
		}

		/// <summary>
		/// 弾薬の消費
		/// </summary>
		/// <returns><c>true</c>, if ammo was subed, <c>false</c> otherwise.</returns>
		/// <param name="use">Use.</param>
		public bool UseAmmo(Ammo use) {
			if(_ammo.Sub(use)) {
				_onChangedAmmo.Invoke(_ammo, _maxAmmo);
				return true;
			} else {
				_onNotEnoughAmmo.Invoke(_ammo, _maxAmmo);
				return false;
			}
		}

		/// <summary>
		/// 弾薬を増やす
		/// </summary>
		public void AddAmmo(Ammo add) {
			_ammo.AddMax(add, _maxAmmo);
			_onChangedAmmo.Invoke(_ammo, _maxAmmo);
		}

		/// <summary>
		/// トリガーを引く
		/// </summary>
		public void PullTrigger() {
			for(int i = 0; i < _launchers.Count; ++i) {
				_launchers[i].PullTrigger();
			}
		}

		/// <summary>
		/// トリガーを押しもどす
		/// </summary>
		public void PushTrigger() {
			for(int i = 0; i < _launchers.Count; ++i) {
				_launchers[i].PushTrigger();
			}
		}

		/// <summary>
		/// 指定した座標の方向を向かせる
		/// </summary>
		/// <returns>The look.</returns>
		/// <param name="position">Position.</param>
		public void Look(Vector3 position) {
			for(int i = 0; i < _launchers.Count; ++i) {
				_launchers[i].Look(position);
			}
		}
	}
}