using UnityEngine;
using System;
using EditorUtil;
using Polying.World.Explorer;

namespace Polying.Test {

	/// <summary>
	/// 弾
	/// </summary>
	[RequireComponent(typeof(Rigidbody2D))]
	public class Bullet : CollisionAttacker {

		[Header("Trail Effect Parameter")]
		[SerializeField]
		private EffectParams _trailEffectParams;

		[Header("Hit Effect Parameter")]
		[SerializeField, Range(1, 100)]
		private int _emitCnt = 10;
		[SerializeField]
		private EffectParams _hitEffectParams;

		private Rigidbody2D _rbody;
		private ParticleSystem _emitter;

		private void Awake() {
			_rbody = GetComponent<Rigidbody2D>();
			_emitter = FindObjectOfType<ParticleSystem>();
		}

		private void Update() {
			if(_emitter) {
				EffectFunctions.EmitParticle(_emitter, _trailEffectParams, transform.position, 0f);
			}
		}

		protected override void OnCollisionEnter2D(Collision2D co) {
			float angle = Functions.VectorToDeg(co.relativeVelocity) + 180f;
			Vector3 position = co.contacts[0].point;
			for(int i = 0; i < _emitCnt; ++i) {
				EffectFunctions.EmitParticle(_emitter, _hitEffectParams, position, angle);
			}
			base.OnCollisionEnter2D(co);
		}
	}
}