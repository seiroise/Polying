using UnityEngine;
using System.Collections;

namespace Polying.Test {

	/// <summary>
	/// 2Dのキーボードとマウスのコントローラ
	/// </summary>
	public class KeyMouseController2D : RigidbodyController2D {

		[Header("Camera")]
		[SerializeField]
		private Camera _camera;

		private Vector3 _mousePoint;

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
			_mousePoint = Input.mousePosition;
			_mousePoint.z = -_camera.transform.position.z;
			_mousePoint = _camera.ScreenToWorldPoint(Input.mousePosition) - _trans.position;
			SetGazeAngle(Mathf.Atan2(_mousePoint.y, _mousePoint.x) * Mathf.Rad2Deg);
		}
	}
}