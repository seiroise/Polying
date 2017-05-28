using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Polying.Common;

namespace Polying.Title {

	/// <summary>
	/// タイトル画面のUI制御
	/// </summary>
	public class TitleUIController : SceneUIController {

		[SerializeField]
		private Button _startBtn;

		private void Awake() {
			AddOnClickListener(_startBtn, OnStartBtnClicked);
		}

		/// <summary>
		/// Startボタンのクリックコールバック
		/// </summary>
		private void OnStartBtnClicked() {
			GameManager.instance.LoadScene("Menu");
		}
	}
}