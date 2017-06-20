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

		/// <summary>
		/// エフェクトパラメータ
		/// </summary>
		[Serializable]
		public class EffectParams {
			[SerializeField, MinMaxRange(0.01f, 100f)]
			public MinMax speed;
			[SerializeField, MinMaxRange(-90f, 360f)]
			public MinMax angle;
			[SerializeField, MinMaxRange(0.01f, 100f)]
			public MinMax size;
			[SerializeField, MinMaxRange(0.01f, 100f)]
			public MinMax lifetime;
			[SerializeField]
			public Color32 color;
		}

		[Header("Trail Effect Parameter")]
		[SerializeField]
		private EffectParams _trailEffectParams;

		[Header("Hit Effect Parameter")]
		[SerializeField, Range(1, 100)]
		private int _emitCnt = 10;
		[SerializeField]
		private EffectParams _hitEffectParams;

		private Rigidbody2D _rbody;
		private ParticleSystem _particleSystem;

		private void Awake() {
			_rbody = GetComponent<Rigidbody2D>();
			_particleSystem = FindObjectOfType<ParticleSystem>();
		}

		private void Update() {
			if(_particleSystem) {
				EmitParticle(_trailEffectParams, transform.position, 0f);
			}
		}

		protected override void OnCollisionEnter2D(Collision2D co) {
			float angle = Functions.VectorToDeg(co.relativeVelocity) + 180f;
			Vector3 position = co.contacts[0].point;
			for(int i = 0; i < _emitCnt; ++i) {
				EmitParticle(_hitEffectParams, position, angle);
			}
			base.OnCollisionEnter2D(co);
		}

		/// <summary>
		/// パーティクルの生成
		/// </summary>
		/// <param name="param">Parameter.</param>
		/// <param name="posision">Posision.</param>
		/// <param name="baseAngle">Base angle.</param>
		private void EmitParticle(EffectParams param, Vector3 posision, float baseAngle) {
			Vector3 position = transform.position;
			_particleSystem.Emit(
				position:	position,
				velocity:	Functions.DegToVector(param.angle.random + baseAngle) * param.speed.random,
				size:		param.size.random,
				lifetime:	param.lifetime.random,
				color:		param.color);
		}
	}
}