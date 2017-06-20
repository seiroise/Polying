using UnityEngine;
using System;

namespace Polying.Test {

	/// <summary>
	/// 当たり判定を伴わない攻撃オブジェクト
	/// </summary>
	public class TriggerAttacker : EntityAttacker {
		
		protected virtual void OnTriggerEnter2D(Collider2D co) {
			HitEntity(co.GetComponent<Entity>());
		}
	}
}