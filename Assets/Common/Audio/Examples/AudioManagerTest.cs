using UnityEngine;
using System;

namespace Common.Audio {

	/// <summary>
	/// AudioManagerのテスト
	/// </summary>
	[RequireComponent(typeof(AudioManager))]
	public class AudioManagerTest : MonoBehaviour {

		[Header("Test BGM Clip")]
		[SerializeField]
		private AudioClip _testBGMClip1;
		[SerializeField]
		private AudioClip _testBGMClip2;

		[Header("Test SE Clip")]
		[SerializeField]
		private AudioClip _testSEClip;

		private AudioManager _audio;

		private void Awake() {
			_audio = GetComponent<AudioManager>();
		}

		private void OnGUI() {
			GUILayout.BeginHorizontal();

			//SE
			GUILayout.BeginVertical();
			if(GUILayout.Button("Play 1 SE")) {
				_audio.PlaySE(_testSEClip);
			}

			if(GUILayout.Button("Play 2 SE")) {
				MultiSEPlay(_testSEClip, 2);
			}

			if(GUILayout.Button("Play 4 SE")) {
				MultiSEPlay(_testSEClip, 4);
			}

			if(GUILayout.Button("Play 8 SE")) {
				MultiSEPlay(_testSEClip, 8);
			}

			if(GUILayout.Button("Play 10 SE")) {
				MultiSEPlay(_testSEClip, 10);
			}
			GUILayout.EndVertical();

			//BGM
			GUILayout.BeginVertical();
			if(GUILayout.Button("Play BGM 1")) {
				_audio.PlayBGM(_testBGMClip1);
			}
			if(GUILayout.Button("Play BGM 2")) {
				_audio.PlayBGM(_testBGMClip2);
			}
			GUILayout.EndVertical();

			GUILayout.EndHorizontal();
		}

		private void MultiSEPlay(AudioClip clip, int cnt) {
			for(int i = 0; i < cnt; ++i) {
				_audio.PlaySE(clip);
			}
		}
	}
}