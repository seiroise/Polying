using UnityEngine;
using System;

namespace Polying.Test {

	/// <summary>
	/// 爆発
	/// </summary>
	[RequireComponent(typeof(Collider2D))]
	public class Explosion : TriggerAttacker {

		[Header("Explosion")]
		[SerializeField, Range(0f, 1000f)]
		private float _force = 100f;
		[SerializeField, Range(0f, 10f)]
		private float _hitTime = 0.5f;

		private float _timer = 0f;
		private Collider2D _collider;

		private void Awake() {
			_collider = GetComponent<Collider2D>();
		}

		private void Update() {
			if(_timer < _hitTime) {
				_timer += Time.deltaTime;
				if(_timer >= _hitTime) {
					_collider.enabled = false;
				}
			}
		}

		protected override void OnTriggerEnter2D(Collider2D co) {
			base.OnTriggerEnter2D(co);
			var dir = (co.transform.position - transform.position);
			dir.Normalize();
			var rbody = co.GetComponent<Rigidbody2D>();
			if(rbody) {
				rbody.AddForceAtPosition(dir * _force, transform.position);
			}
		}
	}
}