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

		[SerializeField]
		private Launcher _lacunher;
		[SerializeField]
		private Launcher _bigLacunher;

		[SerializeField]
		private ItemCollector _collecter;

		private Rigidbody _rBody;
		private Vector3 _movement;

		private void Awake() {
			_rBody = GetComponent<Rigidbody>();
		}

		private void Start() {
			_terrain.ExcavateCircle(transform.position, 5f, 2000);
		}

		private void Update() {
			_movement.x = Input.GetAxis("Horizontal");
			_movement.y = Input.GetAxis("Vertical");

			_rBody.velocity += _movement * _speed * Time.deltaTime;

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
		}
	}
}