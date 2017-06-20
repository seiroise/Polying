using UnityEngine;
using System.Collections;

namespace Polying.Common {

	/// <summary>
	/// 2Dのキーボードとマウスのコントローラ
	/// </summary>
	public class KeyMouseController2D : RigidbodyController2D {

		[Header("Camera")]
		[SerializeField]
		private Camera _camera;

		private Vector3 _mousePosition;
		private Vector3 _mouseLocalPosition;
		private float _mouseAngle;

		public Vector3 mousePosition {
			get {
				return _mousePosition;
			}
		}
		public Vector3 mouseLocalPosition {
			get {
				return _mouseLocalPosition;
			}
		}
		public float mouseAngle {
			get {
				return _mouseAngle;
			}
		}

		protected override void Awake() {
			base.Awake();
			if(!_camera) {
				_camera = Camera.main;
			}
		}

		private void Update() {
			InputKey();
			InputMouse();
		}

		/// <summary>
		/// キーボード入力
		/// </summary>
		private void InputKey() {
			SetMoveDir(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		}

		/// <summary>
		/// マウス入力
		/// </summary>
		private void InputMouse() {
			_mousePosition = Input.mousePosition;
			_mousePosition.z = -_camera.transform.position.z;
			_mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
			_mouseLocalPosition = _mousePosition - _trans.position;
			_mouseAngle = Mathf.Atan2(_mouseLocalPosition.y, _mouseLocalPosition.x) * Mathf.Rad2Deg;
			SetGazeAngle(_mouseAngle);
		}
	}
}