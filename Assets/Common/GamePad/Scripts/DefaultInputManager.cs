#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace Common.GamePad {

	/// <summary>
	/// 標準のInputManagerを作成する
	/// </summary>
	public class DefaultInputManager {

		[MenuItem("Util/Reset InputManager")]
		public static void ResetInputManager() {
			var data = new InputManagerData();
			data.Apply();
			/*
			Debug.Log("インプットマネージャーの設定を開始します。");
			InputManagerMaker newInputManager = new InputManagerMaker();

			Debug.Log("設定を全てクリアします。");
			newInputManager.Clear();

			Debug.Log("プレイヤーごとの設定を追加します。");
			for(int i = 0; i < 4; i++) {
				AddPlayerInputSettings(newInputManager, i);
			}

			Debug.Log("グローバル設定を追加します。");
			AddGlobalInputSettings(newInputManager);

			Debug.Log("インプットマネージャーの設定が完了しました。");
			*/
		}
	}
}
#endif