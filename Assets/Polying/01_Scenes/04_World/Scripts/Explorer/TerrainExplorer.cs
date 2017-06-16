using UnityEngine;
using System;
using Common.PixelTerrain;
using Polying.Common.Controller;

namespace Polying.World.Explorer {

	/// <summary>
	/// 地形の探索を行う
	/// </summary>
	[RequireComponent(typeof(TerrainChecker), typeof(RigidbodyController))]
	public class TerrainExplorer : MonoBehaviour {

		[Header("Thresholds")]
		[SerializeField, Range(0f, 1f)]
		private float _turnThresh = 0.1f;		//旋回するかの閾値
		[SerializeField, Range(0f, 1f)]
		private float _adjustThresh = 0.1f;		//軌道調整するかの閾値

		[Header("Exploring")]
		[SerializeField, Range(0f, 180f)]
		private float _addNodeThre = 5f;		//角度変化時のノードの追加閾値
		[SerializeField, Range(1f, 100f)]
		private float _samplingDistance;		//周囲を確認する距離間隔

		private TerrainChecker _checker;
		private RigidbodyController _controller;

		private float _checkInterval = 0.5f;
		private float _checkTimer;

		private ExploringMap _map;				//探索地図

		private void Awake() {
			_checker = GetComponent<TerrainChecker>();
			_controller = GetComponent<RigidbodyController>();

			var testAxis = new Vector2(0.1f, 0.8f);
			_controller.SetAxis(testAxis.x, testAxis.y);
		}

		private void Update() {
			_checkTimer -= Time.deltaTime;
			if(_checkTimer <= 0f) {
				UpdateCourse();
			}
		}

		/// <summary>
		/// 軌道の更新
		/// </summary>
		/// <returns>The course.</returns>
		private void UpdateCourse() {
			Vector2 dir;
			float eval;
			//前方確認
			_checker.SolveFrontDirection(out eval, out dir);

			//左右確認
			HitPixelInfo hitInfo;
			float lEval, rEval;
			_checker.CheckLeft(out lEval, out hitInfo);
			_checker.CheckRight(out rEval, out hitInfo);

			if(eval < _adjustThresh) {
				//旋回
				if(lEval > rEval) {
					dir = transform.rotation * new Vector2(0f, 1f);
				} else {
					dir = transform.rotation * new Vector2(0f, -1f);
				}
			} else {
				//軌道修正
				if(lEval > rEval) {
					//左側に軌道修正
					if(rEval < _adjustThresh) {
						//距離近いほど評価値を高くする
						rEval = (1f - (rEval * 0.5f) / _adjustThresh) + 0.5f;
						dir = Quaternion.AngleAxis(20f * rEval, new Vector3(0f, 0f, 1f)) * dir;
						eval *= 0.8f;	//速度をちょっと落とす
					}
				} else {
					//右側に軌道修正
					if(lEval < _adjustThresh) {
						lEval = (1f - (lEval * 0.5f) / _adjustThresh) + 0.5f;
						dir = Quaternion.AngleAxis(-20f * lEval, new Vector3(0f, 0f, 1f)) * dir;
						eval *= 0.8f;	//速度をちょっと落とす
					}
				}
			}

			//軌道の修正
			_controller.SetAxis(dir.x, dir.y);
			_controller.accele = eval;
		}
	}
}