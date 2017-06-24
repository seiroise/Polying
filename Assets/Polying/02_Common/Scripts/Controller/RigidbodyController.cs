using UnityEngine;
using System.Collections;

namespace Polying.Common.Controller {

	/// <summary>
	/// Ridigbodyのコントローラ
	/// </summary>
	[RequireComponent(typeof(Rigidbody))]
	public class RigidbodyController : MonoBehaviour {

		[Header("Parameters")]
		[SerializeField, Range(0f, 100f)]
		private float _moveSpeed = 10f;		//移動速度
		[SerializeField, Range(0f, 1f)]
		private float _accel = 1f;
		[SerializeField, Range(0f, 500f)]
		private float _rotateSpeed = 200f;	//回転速度

		private Rigidbody _rbody;
		private Vector2 _moveAxis;
		private bool _locked;

		public float accele {
			get {
				return _accel;
			}
			set {
				_accel = Mathf.Clamp01(value);
			}
		}

		public bool locked {
			get {
				return _locked;
			}
			set {
				_locked = value;
			}
		}

		private void Awake() {
			_rbody = GetComponent<Rigidbody>();
			_moveAxis = Vector2.zero;
			_locked = false;
		}

		private void FixedUpdate() {
			if(!_locked) {
				Rotation();
				Move();
			}
		}

		/// <summary>
		/// 回転処理
		/// </summary>
		private void Rotation() {
			float angle = Mathf.Atan2(_moveAxis.y, _moveAxis.x) * Mathf.Rad2Deg;
			float delta = Mathf.DeltaAngle(transform.eulerAngles.z, angle) * Mathf.Deg2Rad * 0.5f;
			// sin補間
			float y = Mathf.Sin(delta);
			//_rBody.AddTorque(0f, 0f, y * 50f * Time.deltaTime);
			_rbody.angularVelocity = new Vector3(0f, 0f, y * _rotateSpeed * Time.deltaTime);
		}

		/// <summary>
		/// 移動処理
		/// </summary>
		private void Move() {
			_rbody.velocity += transform.right * _moveSpeed * _accel * Time.deltaTime;
		}

		/// <summary>
		/// 移動軸の設定
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public void SetAxis(float x, float y) {
			_moveAxis.x = x;
			_moveAxis.y = y;
		}
	}
}