using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Common.PixelTerrain;

namespace Polying.World {

	public class DetectedPixel : UnityEvent<HitPixelInfo> { }

	/// <summary>
	/// 地形の確認を行う
	/// </summary>
	public class TerrainChecker : MonoBehaviour {

		[SerializeField]
		private PixelTerrain _terrain;

		[SerializeField, Range(1f, 1000f)]
		private float _frontCheckDistance = 10f;    //前方確認距離
		[SerializeField, Range(1f, 1000f)]
		private float _sideCheckDistance = 5f;      //側方確認距離

		private HitPixelInfo _fronHitInfo;

		private DetectedPixel _frontDetected;
		private DetectedPixel _leftDetected;
		private DetectedPixel _rightDetected;

		public DetectedPixel frontDetected {
			get {
				return _frontDetected;
			}
		}
		public DetectedPixel leftDetected {
			get {
				return _leftDetected;
			}
		}
		public DetectedPixel rightDetected {
			get {
				return _rightDetected;
			}
		}

		private void Awake() {
			_frontDetected = new DetectedPixel();
			_leftDetected = new DetectedPixel();
			_rightDetected = new DetectedPixel();
		}

		private void FixedUpdate() {
			//前方の地形を確認
			CheckFront();
		}

		/// <summary>
		/// 前方の地形を確認
		/// </summary>
		private void CheckFront() {
			HitPixelInfo hitInfo;
			if(_terrain.Ray(transform.position,
							transform.right,
							_frontCheckDistance,
							out hitInfo)) {

				_frontDetected.Invoke(hitInfo);
			}
		}

		/// <summary>
		/// 前方の地形を確認する
		/// </summary>
		public bool CheckFront(out HitPixelInfo hitInfo) {
			if(_terrain.Ray(transform.position,
							transform.right,
							_frontCheckDistance,
							out hitInfo)) {

				_frontDetected.Invoke(hitInfo);
				return true;
			}
			return false;
		}

		/// <summary>
		/// 左方の地形を確認
		/// </summary>
		public bool CheckLeft(out HitPixelInfo hitInfo) {
			if(_terrain.Ray(transform.position,
							transform.up,
							_sideCheckDistance,
							out hitInfo)) {

				_leftDetected.Invoke(hitInfo);
				return true;
			}
			return false;
		}

		/// <summary>
		/// 右方の地形を確認
		/// </summary>
		public bool CheckRight(out HitPixelInfo hitInfo) {
			if(_terrain.Ray(transform.position,
							-transform.up,
							_sideCheckDistance,
							out hitInfo)) {

				_rightDetected.Invoke(hitInfo);
				return true;
			}
			return false;
		}
	}
}