using UnityEngine;
using System;

namespace Polying.Test {

	/// <summary>
	/// エンティティへの攻撃
	/// </summary>
	public abstract class EntityAttacker : MonoBehaviour {

		[SerializeField, Range(1, 1000)]
		private int _damage = 10;
		[SerializeField]
		private EntityAttacker _hitEmitAttacker;

		/// <summary>
		/// エンティティ
		/// </summary>
		/// <param name="entity">Entity.</param>
		protected void HitEntity(Entity entity) {
			if(entity) {
				entity.DecreaseHP(_damage);
			}
			if(_hitEmitAttacker) {
				Instantiate(_hitEmitAttacker, transform.position, transform.rotation);
			}
		}
	}
}