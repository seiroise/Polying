using UnityEngine;
using System.Collections;
using System;
using Polying.World.Explorer;

namespace Polying.Test {

	/// <summary>
	/// レーザーランチャー
	/// </summary>
	public class LaserLauncher : Launcher {

		[Header("Laser")]
		[SerializeField]
		private Laser _laser;
		[SerializeField, Range(1, 10)]
		private int _reflect = 1;
		[SerializeField, Range(1f, 500f)]
		private float _distance = 30f;
		[SerializeField]
		private string _layerName;

		private int _layer;

		private void Awake() {
			_layer = LayerMask.NameToLayer(_layerName);
		}

		/// <summary>
		/// 弾を発射する
		/// </summary>
		protected override void Launch() {
			if(_laser) {
				float dis = _distance;
				int reflect = _reflect;
				ShotLaser(transform.position, transform.right, ref dis, ref reflect);
			}
		}

		/// <summary>
		/// レーザーの発射
		/// </summary>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="distance">Distance.</param>
		private void ShotLaser(Vector2 origin, Vector2 direction, ref float distance, ref int reflect) {
			RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, _layer);
			if(hit.collider) {
				MakeLaser(origin, direction, hit.distance);
				distance -= hit.distance;
				--reflect;
				if(distance > 0f && reflect > 0) {
					Vector2 dir = (hit.normal * 2f + direction).normalized;
					ShotLaser(hit.point + dir * 0.01f, dir, ref distance, ref reflect);
				}
			} else {
				MakeLaser(origin, direction, distance);
				distance = 0f;
			}
		}

		/// <summary>
		/// レーザーの作成
		/// </summary>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="distance">Distance.</param>
		private void MakeLaser(Vector2 origin, Vector2 direction, float distance) {
			Quaternion quat = Quaternion.AngleAxis(Functions.VectorToDeg(direction), new Vector3(0f, 0f, 1f));
			var laserObj = (GameObject)Instantiate(_laser.gameObject, origin + direction * distance * 0.5f, quat);
			laserObj.GetComponent<Laser>().SetDistance(distance);
		}
	}
}