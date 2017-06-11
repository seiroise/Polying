using UnityEngine;
using System;
using System.Collections.Generic;

namespace Common.PixelTerrain {

	/// <summary>
	/// PixelTerrainのピクセルの種類DB
	/// </summary>
	public class PixelDB {

		private Dictionary<int, PixelDBRecord> _pixelDB;

		public PixelDB() {
			_pixelDB = new Dictionary<int, PixelDBRecord>();

			//基本要素
			Add(PixelDBRecord.MakePixel(0, "space"));
			Add(PixelDBRecord.MakePixel(1, "air"));
		}

		/// <summary>
		/// 標準的なDBを作成
		/// </summary>
		/// <returns>作成したデータベース</returns>
		public static PixelDB MakeDefault() {
			var db = new PixelDB();

			//地形ブロック
			db.Add(PixelDBRecord.MakeDrawPixel(10, "rock", 30, 1, new Color32(128, 128, 128, 255)));
			db.Add(PixelDBRecord.MakeDrawPixel(11, "middle rock", 1, 100, new Color32(64, 64, 64, 255)));
			db.Add(PixelDBRecord.MakeDrawPixel(12, "hard rock", 500, 1, new Color32(32, 32, 32, 255)));

			//鉱石ブロック
			db.Add(PixelDBRecord.MakeDrawPixel(20, "iron ore", 200, 1, new Color32(150, 100, 60, 255)));

			return db;
		}

		/// <summary>
		/// DBにレコードを追加
		/// </summary>
		/// <param name="data">レコード</param>
		public void Add(PixelDBRecord data) {
			if(_pixelDB.ContainsKey(data.id)) return;
			var temp = data.Clone();
			_pixelDB.Add(temp.id, temp);
		}

		/// <summary>
		/// DBに複数のレコードを追加
		/// </summary>
		/// <param name="records">追加するレコード</param>
		public void AddRecords(PixelDBRecord[] records) {
			for(int i = 0; i < records.Length; ++i) {
				Add(records[i]);
			}
		}

		/// <summary>
		/// DBから指定した識別番号のレコードのコピーを返す
		/// </summary>
		/// <returns>コピーしたデータ</returns>
		/// <param name="id">識別番号</param>
		public PixelDBRecord GetCopiedRecord(int id) {
			PixelDBRecord rec;
			_pixelDB.TryGetValue(id, out rec);
			return rec.Clone();
		}

		/// <summary>
		/// DBから指定した識別番号のレコードを返す
		/// </summary>
		/// <returns>元データ</returns>
		/// <param name="id">識別番号</param>
		public PixelDBRecord GetRecord(int id) {
			PixelDBRecord rec;
			_pixelDB.TryGetValue(id, out rec);
			return rec;
		}

		/// <summary>
		/// 指定した識別番号のデータを持つピクセルデータを返す
		/// </summary>
		/// <returns>ピクセルデータ</returns>
		/// <param name="id">識別番号</param>
		public PixelData IDToPixelData(int id) {
			var record = _pixelDB[id];
			return new PixelData(record.id, record.durability, record.color, record.isDraw);
		}
	}
}