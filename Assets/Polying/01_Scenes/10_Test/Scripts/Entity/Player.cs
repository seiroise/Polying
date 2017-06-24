using UnityEngine;
using Polying.Common;

namespace Polying.Test {

	/// <summary>
	/// プレイヤー
	/// </summary>
	[RequireComponent(typeof(KeyMouseController2D))]
	public class Player : ShooterEntity {

		[Header("Player")]
		[SerializeField]
		private ShooterEntityIndicator _indicator;

		private KeyMouseController2D _controller;

		protected override void Awake() {
			base.Awake();
			_controller = GetComponent<KeyMouseController2D>();
		}

		protected override void Start() {
			base.Start();
			if(_indicator) {
				_indicator.SetShooterEntity(this);
			}
		}

		private void Update() {
			if(Input.GetMouseButtonDown(0)) {
				PullTrigger();
			} else if(Input.GetMouseButtonUp(0)) {
				PushTrigger();
			}
			Look(_controller.mousePosition);
		}
	}
}