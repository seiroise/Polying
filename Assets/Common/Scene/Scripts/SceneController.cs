using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Common.Scene {

	/// <summary>
	/// シーン制御
	/// </summary>
	public class SceneController : MonoBehaviour {

		/// <summary>
		/// フェードのオプション
		/// </summary>
		[Serializable]
		public struct FadeOption {

			[SerializeField, Range(0.1f, 10f)]
			private float _inTime;
			[SerializeField, Range(0f, 60f)]
			private float _intervalTime;
			[SerializeField, Range(0.1f, 10f)]
			private float _outTime;

			public float inTime {
				get {
					return _inTime;
				}
				set {
					_inTime = value;
				}
			}
			public float intervalTime {
				get {
					return _intervalTime;
				}
				set {
					_intervalTime = value;
				}
			}
			public float outTime {
				get {
					return _outTime;
				}
				set {
					_outTime = value;
				}
			}


			public FadeOption(float inTime, float intervalTime, float outTime) {
				_inTime = inTime;
				_intervalTime = intervalTime;
				_outTime = outTime;
			}
		}

		[Header("Fade Parameter")]
		[SerializeField]
		private FadeOption _fadeOpt;    //フェードオプション

		[Header("Scene Parameter")]
		[SerializeField]
		private bool _loadEmptyScene;	//シーン遷移時に空のシーンを挟むか
		[SerializeField]
		private string _emptySceneName;	//空のシーン名

		[Header("Init Parameter")]
		[SerializeField]
		private bool _isDontDestroy;

		private Texture2D _tex;    //黒いテクスチャ
		private float _alpha;           //現在の透明度
		private bool _isFading;         //フェード中フラグ

		public bool isFading {
			get {
				return _isFading;
			}
		}

		private void Awake() {
			if(_isDontDestroy) {
				DontDestroyOnLoad(this);
			}

			//黒いテクスチャを作る
			_tex = MakeTexture(Color.black);
		}

		private void OnGUI() {
			if(!_isFading) return;

			//透明度を更新して黒テクスチャを描画
			GUI.color = new Color(0, 0, 0, _alpha);
			GUI.DrawTexture(new Rect(-10, -10, Screen.width + 10, Screen.height + 10), _tex);
		}

		/// <summary>
		/// 指定した色のテクスチャを生成する。
		/// </summary>
		/// <returns>テクスチャ</returns>
		private Texture2D MakeTexture(Color color) {
			Texture2D texture;
			texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
			SetTextureColor(texture, color);
			return texture;
		}

		/// <summary>
		/// フェード時に使用するテクスチャの色を設定する
		/// </summary>
		private void SetTextureColor(Texture2D tex, Color color) {
			for(int x = 0; x < tex.width; ++x) {
				for(int y = 0; y < tex.height; ++y) {
					tex.SetPixel(x, y, color);
				}
			}
			tex.Apply();
		}

		/// <summary>
		/// フェードアウト/イン間でのシーンの遷移
		/// </summary>
		/// <returns>イテレータ</returns>
		/// <param name="sceneName">遷移シーン名</param>
		private IEnumerator LoadSceneFadeOutIn(string sceneName) {
			yield return StartCoroutine(FadeOut(_fadeOpt.outTime));
			_isFading = true;
			if(_loadEmptyScene) {
				SceneManager.LoadScene(_emptySceneName);
			}
			SceneManager.LoadScene(sceneName);

			if(_fadeOpt.intervalTime > 0f) {
				yield return StartCoroutine(Timer(_fadeOpt.intervalTime));
			}
			yield return StartCoroutine(FadeIn(_fadeOpt.inTime));
		}

		/// <summary>
		/// フェードイン処理。指定の時間をかけて_alphaを徐々に0に変化させる
		/// </summary>
		/// <returns>イテレータ</returns>
		/// <param name="time">フェードイン時間</param>
		private IEnumerator FadeIn(float time) {
			_isFading = true;

			var timer = time;
			do {
				timer -= Time.deltaTime;
				_alpha = timer / time;
				yield return 0;
			} while(timer > 0);

			_alpha = 0f;
			yield return 0;

			_isFading = false;
		}

		/// <summary>
		/// フェードアウト処理。指定の時間をかけて_alphaを徐々に1に変化させる
		/// </summary>
		/// <returns>イテレータ</returns>
		/// <param name="time">フェードアウト時間</param>
		private IEnumerator FadeOut(float time) {
			_isFading = true;

			var timer = 0f;
			do {
				timer += Time.deltaTime;
				_alpha = timer / time;
				yield return 0;
			} while(timer <= time);

			_alpha = 1f;
			yield return 0;

			_isFading = false;
		}

		/// <summary>
		/// 指定した時間後にコールバックを実行するタイマー
		/// </summary>
		/// <returns>イテレータ</returns>
		/// <param name="time">タイマー時間</param>
		private IEnumerator Timer(float time) {
			yield return new WaitForSeconds(time);
		}

		/// <summary>
		/// フェードに使用するテクスチャの色を設定する
		/// </summary>
		/// <param name="color">テクスチャの色</param>
		public void SetTextureColor(Color color) {
			
			SetTextureColor(_tex, color);
		}

		/// <summary>
		/// シーンのロード
		/// </summary>
		/// <param name="sceneName">ロードするシーン</param>
		public void LoadScene(string sceneName) {
			StartCoroutine(LoadSceneFadeOutIn(sceneName));
		}
	}
}