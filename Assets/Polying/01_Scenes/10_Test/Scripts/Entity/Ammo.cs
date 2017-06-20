using UnityEngine;
using System;

namespace Polying.Test {

	/// <summary>
	/// 弾薬
	/// </summary>
	[Serializable]
	public class Ammo {
		[SerializeField, Range(0, 1000)]
		public int bullet;
		[SerializeField, Range(0, 1000)]
		public int grenade;
		[SerializeField, Range(0, 1000)]
		public int energy;

		public Ammo(int bullet, int grenade, int energy) {
			this.bullet = bullet;
			this.grenade = grenade;
			this.energy = energy;
		}

		/// <summary>
		/// 弾薬を加算する
		/// </summary>
		/// <returns>The add.</returns>
		/// <param name="add">Ammo.</param>
		public void Add(Ammo add) {
			bullet += add.bullet;
			grenade += add.grenade;
			energy += add.energy;
		}

		/// <summary>
		/// 最大値を指定して弾薬を加算する
		/// </summary>
		/// <param name="add">Add.</param>
		/// <param name="max">Max.</param>
		public void AddMax(Ammo add, Ammo max) {
			bullet = Mathf.Max(bullet + add.bullet, max.bullet);
			grenade = Mathf.Max(grenade + add.grenade, max.grenade);
			energy = Mathf.Max(energy + add.energy, max.energy);
		}

		/// <summary>
		/// 弾薬の減算。弾薬がたりない場合はfalseを返す
		/// </summary>
		/// <returns>The sub.</returns>
		/// <param name="sub">Ammo.</param>
		public bool Sub(Ammo sub) {
			if(bullet >= sub.bullet &&
			   grenade >= sub.grenade &&
			   energy >= sub.energy) {
				bullet -= sub.bullet;
				grenade -= sub.grenade;
				energy -= sub.energy;
				return true;
			} else {
				return false;
			}
		}
	}
}