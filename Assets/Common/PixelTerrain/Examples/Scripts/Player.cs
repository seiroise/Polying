using UnityEngine;
using System.Collections;

namespace Common.PixelTerrain {

	/// <summary>
	/// プレイヤー
	/// </summary>
	[RequireComponent(typeof(Rigidbody))]
	public class Player : MonoBehaviour {

		[SerializeField]
		private PixelTerrain _terrain;

		[SerializeField, Range(1f, 100f)]
		private float _moveSpeed = 1f;
		[SerializeField, Range(1f, 500f)]
		private float _rotateSpeed = 200f;

		[SerializeField]
		private Launcher _lacunher;
		[SerializeField]
		private Launcher _bigLacunher;

		[SerializeField]
		private ItemCollector _collecter;

		private Rigidbody _rBody;
		private Vector3 _inputAxis;
		private bool _inputedAxis;

		private void Awake() {
			_rBody = GetComponent<Rigidbody>();
		}

		private void Start() {
			_terrain.ReduceDurabilityInCircle(transform.position, 5f, 2000);
		}

		private void FixedUpdate() {
			Rotation();
			Move();
		}

		private void Update() {
			InputKey();
		}

		/// <summary>
		/// キー入力
		/// </summary>
		private void InputKey() {
			//軸入力
			_inputAxis.x = Input.GetAxis("Horizontal");
			_inputAxis.y = Input.GetAxis("Vertical");
			_inputedAxis = _inputAxis.magnitude > 0.1f;

			//ボタン入力
			if(Input.GetKeyDown(KeyCode.Z)) {
				if(_lacunher) {
					_lacunher.Shot();
				}
			}
			if(Input.GetKeyDown(KeyCode.X)) {
				if(_bigLacunher) {
					_bigLacunher.Shot();
				}
			}
			if(Input.GetKeyDown(KeyCode.C)) {
				if(_collecter) {
					_collecter.Collect();
				}
			}
			if(Input.GetKeyDown(KeyCode.V)) {
				if(_terrain) {
					HitPixelInfo hitInfo;
					for(int i = 0; i < 100; ++i) {
						_terrain.Ray(transform.position, transform.right, 100f, out hitInfo);
					}

					var sw = System.Diagnostics.Stopwatch.StartNew();
					if(_terrain.Ray(transform.position, transform.right, 100f, out hitInfo)) {
						sw.Stop();
						Debug.Log(string.Format("hit id:{0} point:{1} dist:{2}", hitInfo.pixel.id, hitInfo.pixelPoint, hitInfo.distance));
						Debug.Log("Raycast: " + sw.Elapsed + ", dist: " + hitInfo.distance);
					} else {
						sw.Stop();
						Debug.Log("no hit");
					}
				}
			}

		}

		/// <summary>
		/// 回転処理
		/// </summary>
		private void Rotation() {
			if(!_inputedAxis) return;
			float angle = Mathf.Atan2(_inputAxis.y, _inputAxis.x) * Mathf.Rad2Deg;
			float delta = Mathf.DeltaAngle(transform.eulerAngles.z, angle) * Mathf.Deg2Rad * 0.5f;

			// sin補間
			float y = Mathf.Sin(delta);

			//_rBody.AddTorque(0f, 0f, y * 50f * Time.deltaTime);
			_rBody.angularVelocity = new Vector3(0f, 0f, y * _rotateSpeed * Time.deltaTime);
		}

		/// <summary>
		/// 移動処理
		/// </summary>
		private void Move() {
			if(!_inputedAxis) return;
			_rBody.velocity += transform.right * _moveSpeed * Time.deltaTime;
		}
	}
}