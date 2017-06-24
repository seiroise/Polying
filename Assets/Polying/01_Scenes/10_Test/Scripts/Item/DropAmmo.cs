using UnityEngine;
using System;
using System.Text;

namespace Polying.Test {
	
	/// <summary>
	/// そこらへんに落ちてる弾薬
	/// </summary>
	[RequireComponent(typeof(SpriteRenderer))]
	public class DropAmmo : DropItem {

		[Header("DropAmmo")]
		[SerializeField]
		private Ammo _addAmmo;

		[Header("PickUp Effect Parameter")]
		[SerializeField]
		private EffectParams _pickupEffectParams;

		/// <summary>
		/// 拾う
		/// </summary>
		/// <param name="entity">Entity.</param>
		protected override void PickUp(Entity entity) {
			var shooter = entity as ShooterEntity;
			if(shooter) {
				shooter.AddAmmo(_addAmmo);
			}
			var renderer = GetComponent<SpriteRenderer>();
			var emitter = FindObjectOfType<ParticleSystem>();

			if(emitter) {
				EffectFunctions.EmitParticle(emitter, _pickupEffectParams, transform, renderer.sprite);
			}
			Destroy(gameObject);
		}

		/// <summary>
		/// 拾った時のメッセージ
		/// </summary>
		/// <returns>The pick up message.</returns>
		public override string GetPickUpMsg() {
			return _addAmmo.ToString();
		}
	}
}