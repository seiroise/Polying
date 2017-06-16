using UnityEngine;
using System;

namespace Polying.Common.Controller {

	/// <summary>
	/// キーボードとマウスのコントローラ
	/// </summary>
	[RequireComponent(typeof(Rigidbody))]
	public class KeyMouseController : MonoBehaviour {

		[Header("Camera")]
		[SerializeField]
		private Camera _camera;

		[Header("Parameters")]
		[SerializeField, Range(0f, 100f)]
		private float _speed = 1f;

		private Transform _trans;
		private Rigidbody _rbody;

		private float _gazeAngle;
		private Vector3 _mousePoint;

		private Vector3 _axis;

		private void Awake() {
			_trans = GetComponent<Transform>();
			_rbody = GetComponent<Rigidbody>();
		}

		private void Update() {
			InputKey();
			InputMouse();

			Move();
			Rotate();
		}

		/// <summary>
		/// キーボード入力
		/// </summary>
		private void InputKey() {
			_axis.x = Input.GetAxis("Horizontal");
			_axis.y = Input.GetAxis("Vertical");
		}

		/// <summary>
		/// マウス入力
		/// </summary>
		private void InputMouse() {
			_mousePoint = Input.mousePosition;
			_mousePoint.z = -_camera.transform.position.z;
			_mousePoint = _camera.ScreenToWorldPoint(Input.mousePosition) - _trans.position;
			_gazeAngle = Mathf.Atan2(_mousePoint.y, _mousePoint.x) * Mathf.Rad2Deg;
		}

		/// <summary>
		/// 移動
		/// </summary>
		private void Move() {
			//_rbody.position += _axis * _speed * Time.deltaTime;
			_rbody.MovePosition(_rbody.position + _axis * _speed * Time.deltaTime);
		}

		/// <summary>
		/// 回転
		/// </summary>
		private void Rotate() {
			_trans.eulerAngles = new Vector3(0f, 0f, _gazeAngle);
		}
	}
}