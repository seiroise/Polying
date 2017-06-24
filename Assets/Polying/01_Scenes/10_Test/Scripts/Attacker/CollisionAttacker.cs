using UnityEngine;
using System;

namespace Polying.Test {

	/// <summary>
	/// 当たり判定を伴う攻撃オブジェクト
	/// </summary>
	public class CollisionAttacker : EntityAttacker {

		protected virtual void OnCollisionEnter2D(Collision2D co) {
			HitEntity(co.gameObject.GetComponent<Entity>());
			Destroy(gameObject);
		}
	}
}