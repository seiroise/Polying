using UnityEngine;
using System.Collections;
using Common.Common;
using System;
using Common.Audio;
using Common.Scene;

namespace Polying.Common {

	/// <summary>
	/// ゲームの管理
	/// </summary>
	public class GameManager : SingletonMonoBehaviour<GameManager> {

		[Header("Controller")]
		[SerializeField]
		private AudioController _audio;
		[SerializeField]
		private SceneController _scene;

		protected override void SingletonAwake() {
			
		}

		/// <summary>
		/// 指定した名前のシーンに遷移する。
		/// </summary>
		/// <param name="sceneName">シーン名</param>
		public void LoadScene(string sceneName) {
			if(_scene.isFading) {
				Debug.LogWarning("scene is loading");
			} else {
				_scene.LoadScene(sceneName);
			}
		}
	}
}