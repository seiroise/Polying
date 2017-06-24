using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Polying.CommonUI;

namespace Polying.Test {

	/// <summary>
	/// ShooterEntityの状態表示
	/// </summary>
	public class ShooterEntityIndicator : MonoBehaviour {

		[Header("HP")]
		[SerializeField]
		private ValueBar _hpBar;

		[Header("Ammo")]
		[SerializeField]
		private ValueBar _bulletBar;
		[SerializeField]
		private ValueBar _grenadeBar;
		[SerializeField]
		private ValueBar _energyBar;

		/// <summary>
		/// 状態表示するShooterEntityの設定
		/// </summary>
		/// <param name="target">Target.</param>
		public void SetShooterEntity(ShooterEntity target) {
			target.onChangedHP.RemoveListener(OnChangedHP);
			target.onChangedHP.AddListener(OnChangedHP);
			target.onChangedAmmo.RemoveListener(OnChangedAmmo);
			target.onChangedAmmo.AddListener(OnChangedAmmo);
		}

		/// <summary>
		/// HPの変化
		/// </summary>
		/// <param name="max">Hp.</param>
		private void OnChangedHP(int current, int max) {
			_hpBar.SetValue(current, max);
		}

		/// <summary>
		/// 弾薬量の変化
		/// </summary>
		/// <param name="current">Current.</param>
		/// <param name="max">Max.</param>
		private void OnChangedAmmo(Ammo current, Ammo max) {
			_bulletBar.SetValue(current.bullet, max.bullet);
			_grenadeBar.SetValue(current.grenade, max.grenade);
			_energyBar.SetValue(current.energy, max.energy);
		}
	}
}