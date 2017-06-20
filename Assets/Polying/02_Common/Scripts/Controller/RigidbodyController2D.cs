using UnityEngine;
using System;

namespace Polying.Common {

	/// <summary>
	/// Rigidbody2Dのコントローラ
	/// </summary>
	[RequireComponent(typeof(Rigidbody2D))]
	public class RigidbodyController2D : MonoBehaviour {

		[Header("Base Move Parameters")]
		[SerializeField, Range(0f, 500f)]
		private float _accel = 200f;				//加速度
		[SerializeField, Range(0f, 1f)]
		private float _accelThresh = 0.5f;			//加速閾値
		[SerializeField, Range(0f, 400f)]
		private float _brake = 200f;				//ブレーキ
		[SerializeField, Range(1f, 10f)]
		private float _steering = 10f;				//曲がりやすさ
		[SerializeField, Range(0f, 100f)]
		private float _lowSpeedScaleThres = 10f;    //低速時の速度補正閾値

		[Header("Base Rotation Parameters")]
		[SerializeField, Range(30f, 520f)]
		private float _maxRotateAccele = 360f;		//最大回転加速度
		[SerializeField, Range(1f, 100f)]
		private float _rotateAccele = 30f;			//回転加速度
		[SerializeField, Range(0.01f, 1f)]
		private float _angulerDrag = 0.3f;			//回転の抵抗

		protected Transform _trans;
		protected Rigidbody2D _rbody;

		private Vector2 _moveDir;
		private float _speed;
		private Vector2 _addForce;

		private float _gazeAngle;
		private float _deltaAngle;
		private float _angulerSpeed;
		private float _addTorque;

		protected virtual void Awake() {
			_trans = GetComponent<Transform>();
			_rbody = GetComponent<Rigidbody2D>();
		}

		private void FixedUpdate() {
			_speed = _rbody.velocity.magnitude;
			_angulerSpeed = _rbody.angularVelocity = Mathf.Clamp(_rbody.angularVelocity, -_maxRotateAccele, _maxRotateAccele);
			Move();
			Rotate();
		}

		/// <summary>
		/// 移動
		/// </summary>
		private void Move() {
			if(_moveDir.magnitude > _accelThresh) {
				if(_speed < _lowSpeedScaleThres) {
					// 低速補正
					float scale = _accel / (_speed + 1f);
					_addForce = _moveDir * _accel * scale * Time.deltaTime;
				} else {
					_addForce = _moveDir * _accel * Time.deltaTime;
				}
				// 方向転換補正
				_addForce -= _rbody.velocity * _steering;
				_rbody.AddForce(_addForce);
			} else if(_speed > 0f) {
				//ブレーキ
				_addForce = -_rbody.velocity * _brake * Time.deltaTime;
				_rbody.AddForce(_addForce);
			}
		}

		/// <summary>
		/// 回転
		/// </summary>
		private void Rotate() {
			_addTorque = (_deltaAngle - _angulerSpeed * _angulerDrag) * _rotateAccele * Time.deltaTime;
			_rbody.AddTorque(_addTorque);
		}

		/// <summary>
		/// 方向の設定
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public void SetMoveDir(float x, float y) {
			_moveDir.x = x;
			_moveDir.y = y;
			_moveDir.Normalize();
		}

		/// <summary>
		/// 注視角度の設定
		/// </summary>
		/// <param name="angle">Angle.</param>
		public void SetGazeAngle(float angle) {
			_gazeAngle = angle;
			_deltaAngle = Mathf.DeltaAngle(_trans.eulerAngles.z, angle);
		}
	}
}