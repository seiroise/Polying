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

		private float _checkInterval = 0.5f;
		private float _checkTimer;
		private HitPixelInfo[] _hitInfos;

		private void Awake() {
			_checker = GetComponent<TerrainChecker>();
			_controller = GetComponent<RigidbodyController>();

			var testAxis = new Vector2(0.1f, 0.8f);
			_controller.SetAxis(testAxis.x, testAxis.y);
		}

		private void Update() {
			_checkTimer -= Time.deltaTime;
			if(_checkTimer <= 0f) {
				Vector2 dir;
				_checker.SolveFrontDirection(out dir);
				_controller.SetAxis(dir.x, dir.y);
			}
		}
	}
}