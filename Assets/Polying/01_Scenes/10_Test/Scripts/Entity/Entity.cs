using UnityEngine;
using UnityEngine.Events;
using System;

namespace Polying.Test {

	/// <summary>
	/// ゲーム内の実態
	/// </summary>
	public class Entity : MonoBehaviour {

		[Header("Status")]
		[SerializeField, Range(10, 1000)]
		private int _hp = 100;

		private int _currentHP;

		private HPEvent _onChangedHP;
		private UnityEvent _onDied;

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
		public HPEvent onChangedHP {
			get {
				return _onChangedHP;
			}
		}
		public UnityEvent onDie {
			get {
				return _onDied;
			}
		}

		protected virtual void Awake() {
			_currentHP = _hp;
			_onChangedHP = new HPEvent();
		}

		protected virtual void Start() {
			
		}

		/// <summary>
		/// HPを下げる。HPが0以下になった場合はtrueを返す
		/// </summary>
		/// <returns><c>true</c>, if hp was decreased, <c>false</c> otherwise.</returns>
		/// <param name="damage">Damage.</param>
		public bool DecreaseHP(int damage) {
			if(_currentHP <= 0) return false;
			_currentHP -= damage;
			if(_onChangedHP != null) {
				_onChangedHP.Invoke(_currentHP, _hp);
			}
			if(_currentHP <= 0) {
				_currentHP = 0;
				if(_onDied != null) {
					_onDied.Invoke();
				}
				Destroy(gameObject);
				return true;
			} else {
				return false;
			}
		}
	}
}