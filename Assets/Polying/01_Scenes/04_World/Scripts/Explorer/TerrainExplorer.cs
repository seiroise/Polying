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
			Vector2 dir;
			_checker.SolveFrontDirection(out dir);
			_controller.SetAxis(dir.x, dir.y);
		}
	}
}