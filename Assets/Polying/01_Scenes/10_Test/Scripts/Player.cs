using UnityEngine;
using System;

namespace Polying.Test {

	/// <summary>
	/// プレイヤー
	/// </summary>
	[RequireComponent(typeof(KeyMouseController2D))]
	public class Player : MonoBehaviour {

		[SerializeField]
		private Launcher[] _launchers;

		private KeyMouseController2D _controller;

		private void Awake() {
			_controller = GetComponent<KeyMouseController2D>();
		}

		private void Update() {
			if(Input.GetMouseButtonDown(0)) {
				foreach(var l in _launchers) {
					l.Shot();
				}
			}
		}
	}
}