using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Polying.Common;

namespace Polying.Menu {

	/// <summary>
	/// メニュー画面のUI制御
	/// </summary>
	public class MenuUIController : SceneUIController {

		[SerializeField]
		private Button _playBtn;
		[SerializeField]
		private Button _configBtn;
		[SerializeField]
		private Button _toTitleBtn;

		private void Awake() {
			AddOnClickListener(_playBtn, OnPlayBtnClicked);
			AddOnClickListener(_configBtn, OnConfigBtnClicked);
			AddOnClickListener(_toTitleBtn, OnToTitleBtnClicked);
		}

		/// <summary>
		/// Playボタンのクリックコールバック
		/// </summary>
		private void OnPlayBtnClicked() {
			Debug.Log("comming soon.");
		}

		/// <summary>
		/// Configボタンのクリックコールバック
		/// </summary>
		private void OnConfigBtnClicked() {
			Debug.Log("comming soon.");
		}

		/// <summary>
		/// ToTitleボタンのクリックコールバック
		/// </summary>
		private void OnToTitleBtnClicked() {
			GameManager.instance.LoadScene("Title");
		}
	}
}