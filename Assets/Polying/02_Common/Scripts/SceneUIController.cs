using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Polying.Common {

	/// <summary>
	/// シーン毎のUI制御クラスの基底
	/// </summary>
	public class SceneUIController : MonoBehaviour {

		/// <summary>
		/// ButtonのonClickイベントにコールバックを重複しないように追加する。
		/// </summary>
		/// <param name="btn">ボタン</param>
		/// <param name="callback">追加するコールバック</param>
		protected static void AddOnClickListener(Button btn, UnityAction callback) {
			btn.onClick.RemoveListener(callback);
			btn.onClick.AddListener(callback);
		}
	}
}