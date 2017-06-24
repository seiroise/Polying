using UnityEngine;
using System;

namespace Common.PixelTerrain {

	/// <summary>
	/// プレイヤーを追跡
	/// </summary>
	public class PlayerTrack : MonoBehaviour {

		[SerializeField]
		private Transform _target;

		[SerializeField]
		private Vector3 _offset;

		private void Update() {
			if(_target) {
				transform.position = _target.position + _offset;
			}
		}
	}
}