using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace Common.PixelTerrain {

	/// <summary>
	/// アイテムの所持量を表示するバー
	/// </summary>
	public class ItemBar : MonoBehaviour {

		/// <summary>
		/// アイテムバーに関するデータ
		/// </summary>
		[Serializable]
		public class ItemBarData {
			public string name;
			public int num;
			public RectTransform area;
		}

		[SerializeField]
		private ItemBarData[] _datas;

		private Dictionary<string, ItemBarData> _dataTables;
		private bool _needUpdate;
		private int _totalNum;

		private void Awake() {
			_dataTables = new Dictionary<string, ItemBarData>();
			for(int i = 0; i < _datas.Length; ++i) {
				_dataTables.Add(_datas[i].name, _datas[i]);
			}
		}

		private void Update() {
			UpdateItemBar();
		}

		/// <summary>
		/// アイテムバーの更新
		/// </summary>
		private void UpdateItemBar() {
			if(!_needUpdate) return;

			float basePar = 0f;
			for(int i = 0; i < _datas.Length; ++i) {
				float par = (float)_datas[i].num / _totalNum;
				Vector2 t;
				t = _datas[i].area.anchorMin;
				t.x = basePar;
				_datas[i].area.anchorMin = t;

				basePar += par;

				t = _datas[i].area.anchorMax;
				t.x = basePar;
				_datas[i].area.anchorMax = t;
			}
			_needUpdate = false;
		}

		/// <summary>
		/// アイテムの追加
		/// </summary>
		/// <param name="name">アイテム名</param>
		/// <param name="num">個数</param>
		public void AddItem(string name, int num) {
			if(!_dataTables.ContainsKey(name)) return;
			_dataTables[name].num += num;
			_totalNum += num;
			_needUpdate = true;
		}
	}
}