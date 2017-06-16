using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections;
using Common.PixelTerrain;
using System.Collections.Generic;

namespace Polying.World.Explorer {

	/// <summary>
	/// 地形の確認を行う
	/// </summary>
	public class TerrainChecker : MonoBehaviour {

		/// <summary>
		/// ピクセルの検出イベント
		/// </summary>
		public class DetectedPixel : UnityEvent<HitPixelInfo[]> { }

		private static bool _init;
		private static Vector2[] _frontDirs;
		private static Vector2 _leftDir;
		private static Vector2 _rightDir;

		[SerializeField]
		private PixelTerrain _terrain;

		[SerializeField, Range(1f, 1000f)]
		private float _checkDistance = 10f;    //確認距離
		[SerializeField, Range(0f, 90f)]
		private float _leftAngle = 80f;
		[SerializeField, Range(0f, -90f)]
		private float _rightAngle = -80f;

		private float _checkInterval = 0.5f;
		private float _checkTimer;
		private DetectedPixel _frontDetected;
		private HitPixelInfo[] _hitInfos;

		public float checkInterval {
			get {
				return _checkInterval;
			}
			set {
				_checkInterval = value;
				_checkTimer = value;
			}
		}
		public DetectedPixel frontDetected {
			get {
				return _frontDetected;
			}
		}

		private void Awake() {
			if(!_init) {
				_frontDirs = AngleToDirection(FloatRange(-20f, 20f, 10f).ToArray());
				_leftDir = AngleToDirection(_leftAngle);
				_rightDir = AngleToDirection(_rightAngle);
				_init = true;
			}
		}

		private void Update() {
			DrawRays();
		}

		/// <summary>
		/// 指定した範囲をstepごとに区切った値の配列を出力する
		/// </summary>
		/// <returns>The range.</returns>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		/// <param name="step">Step.</param>
		private IEnumerable<float> FloatRange(float min, float max, float step) {
			for(float v = min; v <= max; v += step) {
				yield return v;
			}
		}

		/// <summary>
		/// 角度配列を方向ベクトル配列に変換
		/// </summary>
		/// <returns>The to direction.</returns>
		/// <param name="angle">Angles.</param>
		private Vector2 AngleToDirection(float angle) {
			float theta = angle * Mathf.Deg2Rad;
			return new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
		}

		/// <summary>
		/// 角度配列を方向ベクトル配列に変換
		/// </summary>
		/// <returns>The to direction.</returns>
		/// <param name="angles">Angles.</param>
		private Vector2[] AngleToDirection(float[] angles) {
			var dirs = new Vector2[angles.Length];
			float theta;
			for(int i = 0; i < angles.Length; ++i) {
				theta = angles[i] * Mathf.Deg2Rad;
				dirs[i] = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
			}
			return dirs;
		}

		/// <summary>
		/// デバッグ用にレイを描画する
		/// </summary>
		private void DrawRays() {
			for(int i = 0; i < _frontDirs.Length; ++i) {
				Debug.DrawRay(transform.position, transform.rotation * _frontDirs[i] * _checkDistance);
			}
			Debug.DrawRay(transform.position, transform.rotation * _leftDir * _checkDistance, Color.blue);
			Debug.DrawRay(transform.position, transform.rotation * _rightDir * _checkDistance, Color.green);
		}

		/// <summary>
		/// 前方のある程度の地形を確認
		/// </summary>
		/// <returns><c>true</c>, if front around was checked, <c>false</c> otherwise.</returns>
		/// <param name="hitInfos">Hit infos.</param>
		public bool CheckFrontAround(out HitPixelInfo[] hitInfos) {
			hitInfos = new HitPixelInfo[_frontDirs.Length];
			var hit = false;
			//主に前方の -45~45を探索
			for(int i = 0; i < _frontDirs.Length; ++i) {
				if(_terrain.Ray(transform.position,
								transform.rotation * _frontDirs[i],
								_checkDistance,
								out hitInfos[i])) {
					hit = true;
				}
			}
			return hit;
		}

		/// <summary>
		/// 左方の地形を確認
		/// </summary>
		/// <returns><c>true</c>, if left was checked, <c>false</c> otherwise.</returns>
		/// <param name="hitInfo">Hit info.</param>
		public bool CheckLeft(out float eval, out HitPixelInfo hitInfo) {
			if(_terrain.Ray(transform.position,
			                transform.rotation * _leftDir,
							_checkDistance,
							out hitInfo)) {
				eval = hitInfo.distance / _checkDistance;
				return true;
			} else {
				eval = 1f;
				return false;
			}
		}

		/// <summary>
		/// 右方の地形を確認
		/// </summary>
		/// <returns><c>true</c>, if right was checked, <c>false</c> otherwise.</returns>
		/// <param name="hitInfo">Hit info.</param>
		public bool CheckRight(out float eval, out HitPixelInfo hitInfo) {
			if(_terrain.Ray(transform.position,
			                transform.rotation * _rightDir,
							_checkDistance,
							out hitInfo)) {
				eval = hitInfo.distance / _checkDistance;
				return true;
			} else {
				eval = 1f;
				return false;
			}
		}

		/// <summary>
		/// 前方に進むための方向とその方向の評価値を取得する。
		/// 前方に進むべき方向がない場合はfalseを返す
		/// </summary>
		/// <returns><c>true</c>, if front direction was solved, <c>false</c> otherwise.</returns>
		/// <param name="eval">Eval.</param>
		/// <param name="dir">Dir.</param>
		public bool SolveFrontDirection(out float eval, out Vector2 dir) {
			HitPixelInfo[] hitInfos;
			if(CheckFrontAround(out hitInfos)) {
				float[] evals = new float[hitInfos.Length];
				for(int i = 0; i < hitInfos.Length; ++i) {
					if(hitInfos[i] == null) {
						evals[i] = 1f;
					} else {
						evals[i] = hitInfos[i].distance / _checkDistance;
					}
				}
				//評価値の高い方向に進む
				float sumEval, maxEval = 0f;
				int maxIndex = -1;
				for(int i = 1; i < evals.Length - 1; ++i) {
					sumEval = 0f;
					sumEval += evals[i - 1];
					sumEval += evals[i];
					sumEval += evals[i + 1];
					sumEval *= 0.33f;
					if(sumEval > maxEval) {
						maxEval = sumEval;
						maxIndex = i;
					}
				}
				eval = maxEval;
				//評価値の高い方向に進む存在しない
				if(hitInfos[maxIndex] != null) {
					dir = hitInfos[maxIndex].direction;
				} else {
					dir = transform.rotation * _frontDirs[maxIndex];
				}
				return true;
			} else {
				//そのまま前方に進む
				eval = 0f;
				dir = transform.right;
				return true;
			}
		}
	}
}