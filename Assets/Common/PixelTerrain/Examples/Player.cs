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
		private float _speed = 1f;

		private Rigidbody _rBody;
		private Vector3 _movement;

		private bool _isCollisioned;	//衝突したか
		private Vector3 contact;		//衝突座標


		private void Awake() {
			_rBody = GetComponent<Rigidbody>();
		}

		private void Start() {
			_terrain.CircleAdd(transform.position, 5f, -1024);
		}

		private void Update() {
			_movement.x = Input.GetAxis("Horizontal");
			_movement.y = Input.GetAxis("Vertical");

			_rBody.velocity += _movement * _speed * Time.deltaTime;

			if(_isCollisioned) {
				_terrain.CircleAdd(contact, 0.5f, -10);
				_isCollisioned = false;
			}
		}

		private void OnCollisionStay(Collision co) {
			if(_terrain && !_isCollisioned) {
				_isCollisioned = true;
				contact = co.contacts[0].point;
			}
		}
	}
}