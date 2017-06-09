using UnityEngine;
using System;
using Common.PixelTerrain;
using Polying.Common;

namespace Polying.World {

	/// <summary>
	/// 地形の探索を行う
	/// </summary>
	[RequireComponent(typeof(TerrainChecker), typeof(RigidbodyController))]
	public class TerrainExplorer : MonoBehaviour {

		private TerrainChecker _checker;
		private RigidbodyController _controller;

		private void Awake() {
			_checker = GetComponent<TerrainChecker>();
			_controller = GetComponent<RigidbodyController>();

			var testAxis = new Vector2(0.1f, 0.8f);
			_controller.SetAxis(testAxis.x, testAxis.y);
		}

		private void Start() {
			_checker.frontDetected.RemoveListener(OnCheckFront);
			_checker.frontDetected.AddListener(OnCheckFront);
		}

		/// <summary>
		/// 前方確認
		/// </summary>
		private void OnCheckFront(HitPixelInfo hitInfo) {
			//右に行くか左に行くか決める
			HitPixelInfo leftHitInfo;
			HitPixelInfo rightHitInfo;
			_checker.CheckLeft(out leftHitInfo);
			_checker.CheckRight(out rightHitInfo);

			if(leftHitInfo != null && rightHitInfo != null) {
				//どちらとも近くに壁 -> 後ろへ
				Vector2 axis = -transform.right;
				_controller.SetAxis(axis.x, axis.y);
				Debug.Log("go back");
			} else if(leftHitInfo != null && rightHitInfo == null) {
				//左に壁 -> 右へ
				Vector2 axis = -transform.up;
				_controller.SetAxis(axis.x, axis.y);
				Debug.Log("go right");
			} else if(leftHitInfo == null && rightHitInfo != null) {
				//右に壁 -> 左へ
				Vector2 axis = transform.up;
				_controller.SetAxis(axis.x, axis.y);
				Debug.Log("go left1");
			} else if(leftHitInfo == null && rightHitInfo == null) {
				//左右に壁なし -> ひとまず左
				Vector2 axis = transform.up;
				_controller.SetAxis(axis.x, axis.y);
				Debug.Log("go left2");
			}
		}
	}
}