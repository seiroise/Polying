using UnityEngine;
using System;
using System.Collections.Generic;

namespace EditorUtil {

	/// <summary>
	/// シリアライズ可能な連想配列
	/// </summary>
	[Serializable]
	public class SerializableTable<TKey, TValue, TPair> where TPair : KeyAndValue<TKey, TValue>, new() {

		[SerializeField]
		protected List<TPair> _list;
		protected Dictionary<TKey, TValue> _table;

		public SerializableTable() {
			Clear();
		}

		/// <summary>
		/// 指定したkeyに対するvalueの取得
		/// </summary>
		public TValue Get(TKey key) {
			if(_table.ContainsKey(key)) {
				return _table[key];
			}
			return default(TValue);
		}

		/// <summary>
		/// 指定したkeyに指定したvalueを設定する。すでに存在するkeyの場合は上書きを行う
		/// </summary>
		public void Set(TKey key, TValue value) {
			if(_table.ContainsKey(key)) {
				_table[key] = value;
			} else {
				_table.Add(key, value);
			}
			Apply();
		}

		/// <summary>
		/// 指定したkeyを削除する
		/// </summary>
		public void Remove(TKey key) {
			if(_table.ContainsKey(key)) {
				_table.Remove(key);
			}
			Apply();
		}

		/// <summary>
		/// テーブルとそれに対応するシリアライズ可能なリストを空にする
		/// </summary>
		public void Clear() {
			_table = new Dictionary<TKey, TValue>();
			_list = new List<TPair>();
		}

		/// <summary>
		/// 反復処理用の列挙子を返す
		/// </summary>
		/// <returns>列挙子</returns>
		public IEnumerator<TPair> GetEnumerator() {
			for(int i = 0; i < _list.Count; ++i) {
				yield return _list[i];
			}
		}

		/// <summary>
		/// 変更の更新
		/// </summary>
		private void Apply() {
			_list = ConvertDictionaryToList(_table);
		}

		/// <summary>
		/// ペアのリストを辞書に変換する
		/// </summary>
		private static Dictionary<TKey, TValue> ConvertListToDictionary(List<TPair> list) {
			Dictionary<TKey, TValue> table = new Dictionary<TKey, TValue>();
			foreach(var pair in list) {
				table.Add(pair.key, pair.value);
			}
			return table;
		}

		/// <summary>
		/// 辞書をペアのリストに変換する
		/// </summary>
		private static List<TPair> ConvertDictionaryToList(Dictionary<TKey, TValue> table) {
			List<TPair> list = new List<TPair>();
			if(table != null) {
				foreach(var pair in table) {
					TPair t = new TPair();
					t.key = pair.Key;
					t.value = pair.Value;
					list.Add(t);
				}
			}
			return list;
		}
	}
}