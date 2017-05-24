#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Common.GamePad {

	/// <summary>
	/// InputManager.assetの内容をデータ化したもの
	/// </summary>
	public class InputManagerData {

		private Dictionary<string, List<InputAxisData>> _axesTable;
		private SerializedObject _serializedObj;
		private SerializedProperty _axesProp;

		public InputManagerData() {
			_serializedObj = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
			_axesProp = _serializedObj.FindProperty("m_Axes");
			_axesTable = MakeAxesTable(_axesProp);
		}

		/// <summary>
		/// 軸情報のテーブルを作成する
		/// </summary>
		/// <returns>作成したテーブル</returns>
		/// <param name="axesProp">軸情報が格納されているプロパティ</param>
		private static Dictionary<string, List<InputAxisData>> MakeAxesTable(SerializedProperty axesProp) {
			var axes = new Dictionary<string, List<InputAxisData>>();
			foreach(SerializedProperty prop in axesProp) {
				var data = InputAxisData.FromSerializedProperty(prop);
				if(!axes.ContainsKey(data.name)) {
					axes.Add(data.name, new List<InputAxisData>());
				}
				axes[data.name].Add(data);
			}
			return axes;
		}

		/// <summary>
		/// 書き込まれている設定を全て消す
		/// </summary>
		private void Erase() {
			_axesProp.ClearArray();
			_serializedObj.ApplyModifiedProperties();
		}

		/// <summary>
		/// 内容の反映
		/// </summary>
		public void Apply() {
			Erase();
			foreach(List<InputAxisData> axisList in _axesTable.Values) {
				for(int i = 0; i < axisList.Count; ++i) {
					_axesProp.arraySize++;
					_serializedObj.ApplyModifiedProperties();
					var axisProp = _axesProp.GetArrayElementAtIndex(_axesProp.arraySize - 1);
					axisList[i].SetSerializedProperty(axisProp);
				}
			}
			_serializedObj.ApplyModifiedProperties();
		}

		/// <summary>
		/// 設定データの追加
		/// </summary>
		/// <param name="axis">設定データ</param>
		public void AddAxis(InputAxisData axis) {
			var name = axis.name;
			if(!_axesTable.ContainsKey(name)) {
				_axesTable.Add(name, new List<InputAxisData>());
			}
			_axesTable[name].Add(axis);
		}

		/// <summary>
		/// 指定した名称の設定データリストへの参照を返す。存在しない場合はnullを返す
		/// </summary>
		/// <returns>設定データリスト</returns>
		/// <param name="name">設定データの名称</param>
		public List<InputAxisData> GetAxes(string name) {
			if(_axesTable.ContainsKey(name)) {
				return _axesTable[name];
			} else {
				return null;
			}
		}

		/// <summary>
		/// 指定した設定データを削除する
		/// </summary>
		/// <returns>削除した場合はtrue</returns>
		/// <param name="axis">削除する設定データ</param>
		public bool Remove(InputAxisData axis) {
			if(_axesTable.ContainsKey(axis.name)) {
				return _axesTable[axis.name].Remove(axis);
			}
			return false;
		}

		/// <summary>
		/// 指定した名称の設定データを全て削除する
		/// </summary>
		/// <returns>削除したものがある場合はtrue</returns>
		/// <param name="name">削除する設定データの名称</param>
		public bool Remove(string name) {
			if(_axesTable.ContainsKey(name)) {
				return _axesTable.Remove(name);
			}
			return false;
		}
	}
}
#endif