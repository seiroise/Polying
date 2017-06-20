using UnityEngine;
using UnityEngine.UI;
using System;

namespace Polying.CommonUI {

	/// <summary>
	/// 値のテキストと一次限量を表すイメージ
	/// </summary>
	public sealed class ValueBar : MonoBehaviour {

		[SerializeField]
		private Text _valueTxt;
		[SerializeField]
		private Image _valueImg;

		/// <summary>
		/// 値の設定
		/// </summary>
		public void SetValue(int val, int max) {
			float ratio = val / (float)max;
			_valueTxt.text = val.ToString();
			_valueImg.fillAmount = ratio;
		}
	}
}