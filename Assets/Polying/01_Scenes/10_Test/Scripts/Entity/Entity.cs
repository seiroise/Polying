using UnityEngine;
using UnityEngine.Events;
using System;

namespace Polying.Test {

	/// <summary>
	/// ゲーム内の実態
	/// </summary>
	[RequireComponent(typeof(SpriteRenderer))]
	public class Entity : MonoBehaviour {

		public enum EntityType {
			None,
			Player,
			NormalEnemy,
			BossEnemy,
			Object,
			ExpObject
		}

		[Header("Status")]
		[SerializeField, Range(10, 1000)]
		private int _hp = 100;
		[SerializeField]
		private EntityType _type;

		[Header("Die Effect Parameter")]
		[SerializeField]
		private EffectParams _dieEffectParams;

		private int _currentHP;

		private SpriteRenderer _renderer;
		private ParticleSystem _emitter;

		private HPEvent _onChangedHP;
		private EntityEvent _onDied;

		public int hp {
			get {
				return _hp;
			}
		}
		public int currentHP {
			get {
				return _currentHP;
			}
		}
		public EntityType type {
			get {
				return _type;
			}
		}
		public HPEvent onChangedHP {
			get {
				return _onChangedHP;
			}
		}
		public EntityEvent onDied {
			get {
				return _onDied;
			}
		}

		protected virtual void Awake() {
			_currentHP = _hp;
			_onChangedHP = new HPEvent();
			_onDied = new EntityEvent();
			_renderer = GetComponent<SpriteRenderer>();
			_emitter = FindObjectOfType<ParticleSystem>();
		}

		protected virtual void Start() {
			EntityManager.instance.RegistEntity(this);
		}

		/// <summary>
		/// HPを下げる。HPが0以下になった場合はtrueを返す
		/// </summary>
		/// <returns><c>true</c>, if hp was decreased, <c>false</c> otherwise.</returns>
		/// <param name="damage">Damage.</param>
		public bool DecreaseHP(int damage) {
			if(_currentHP <= 0) return false;
			_currentHP -= damage;
			_onChangedHP.Invoke(_currentHP, _hp);
			if(_currentHP <= 0) {
				_currentHP = 0;
				_onDied.Invoke(this);
				if(_emitter) {
					EffectFunctions.EmitParticle(_emitter, _dieEffectParams, transform, _renderer.sprite);
				}
				Destroy(gameObject);
				return true;
			} else {
				return false;
			}
		}
	}
}