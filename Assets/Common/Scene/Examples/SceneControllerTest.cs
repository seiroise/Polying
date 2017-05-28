using UnityEngine;
using System.Collections;

namespace Common.Scene {

	/// <summary>
	/// SceneControllerのテスト
	/// </summary>
	[RequireComponent(typeof(SceneController))]
	public class SceneControllerTest : MonoBehaviour {

		[Header("Load Scene")]
		[SerializeField]
		private string _sceneName;
		[SerializeField]
		private Color _fadeColor;

		private SceneController _scene;

		private void Awake() {
			_scene = GetComponent<SceneController>();
		}

		private void OnGUI() {
			if(GUILayout.Button("Load Scene")) {
				_scene.SetTextureColor(_fadeColor);
				_scene.LoadScene(_sceneName);
			}
		}
	}
}