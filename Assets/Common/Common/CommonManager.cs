using UnityEngine;
using System.Collections;
using Common.Audio;
using Common.Scene;
using System;

namespace Common.Common {

	/// <summary>
	/// 共通処理クラスの管理
	/// </summary>
	[RequireComponent(typeof(AudioController), typeof(SceneController))]
	public class CommonManager : SingletonMonoBehaviour<CommonManager> {

		private AudioController _audio;
		private SceneController _scene;

		protected override void SingletonAwake() {
			_audio = GetComponent<AudioController>();
			_scene = GetComponent<SceneController>();
		}

		/// <summary>
		/// 指定したAudioClipをBGmとして再生する
		/// </summary>
		/// <param name="clip">BGMとして再生するAudioClip</param>
		public void PlayBGM(AudioClip clip) {
			_audio.PlayBGM(clip);
		}

		/// <summary>
		/// 指定したAudioClipをSEとして再生する
		/// </summary>
		/// <param name="clip">SEとして再生するAudioClip</param>
		public void PlaySE(AudioClip clip) {
			_audio.PlaySE(clip);
		}

		/// <summary>
		/// 指定した名前のシーンに遷移する
		/// </summary>
		/// <param name="sceneName">遷移するシーン名</param>
		public void LoadScene(string sceneName) {
			_scene.LoadScene(sceneName);
		}
	}
}