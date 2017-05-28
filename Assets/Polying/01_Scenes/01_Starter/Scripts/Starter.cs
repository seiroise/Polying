using UnityEngine;
using System.Collections;
using Polying.Common;

namespace Polying.Starter {

	/// <summary>
	/// 開始処理
	/// </summary>
	public class Starter : MonoBehaviour {

		[SerializeField]
		private string _nextSceneName;

		private IEnumerator Start() {
			yield return new WaitForSeconds(2f);
			var gm = GameManager.instance;
			gm.LoadScene(_nextSceneName);
		}
	}
}