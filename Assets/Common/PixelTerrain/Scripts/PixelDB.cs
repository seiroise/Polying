using UnityEngine;
using System.Collections.Generic;

namespace Common.PixelTerrain {

	/// <summary>
	/// ピクセルのレコード
	/// </summary>
	public class PixelRecord {

		public byte id;			//識別番号
		public string name;		//名前
		public int durability;	//耐久値
		public byte toID;		//耐久値が0以下になった場合の識別番号
		public Color32 color;	//色
		public bool isDraw;		//描画する必要があるか

		public PixelRecord(byte id, string name, int durability, byte toID, Color32 color, bool isDraw) {
			this.id = id;
			this.name = name;
			this.durability = durability;
			this.toID = toID;
			this.color = color;
			this.isDraw = isDraw;
		}

		/// <summary>
		/// 深いコピーを返す
		/// </summary>
		/// <returns>コピー</returns>
		public PixelRecord DeepCopy() {
			return (PixelRecord)MemberwiseClone();
		}
	}

	/// <summary>
	/// PixelTerrainのピクセルの種類DB
	/// </summary>
	public class PixelDB {

		private Dictionary<byte, PixelRecord> _pixelDB;

		public PixelDB() {
			_pixelDB = new Dictionary<byte, PixelRecord>();
			_pixelDB.Add(0, new PixelRecord(0, "air", 0, 0, new Color32(0, 0, 0, 0), false));
		}

		/// <summary>
		/// 標準的なDBを作成
		/// </summary>
		/// <returns>作成したデータベース</returns>
		public static PixelDB MakeDefault() {
			var db = new PixelDB();
			db.Add(1, new PixelRecord(1, "rock", 30, 0, new Color32(128, 128, 128, 255), true));
			db.Add(2, new PixelRecord(2, "middle rock", 100, 0, new Color32(64, 64, 64, 255), true));
			db.Add(3, new PixelRecord(3, "hard rock", 500, 0, new Color32(32, 32, 32, 255), true));

			db.Add(10, new PixelRecord(10, "iron ore", 200, 0, new Color32(150, 100, 60, 255), true));

			return db;
		}

		/// <summary>
		/// DBにレコードを追加
		/// </summary>
		/// <param name="id">識別番号</param>
		/// <param name="data">レコード</param>
		public void Add(byte id, PixelRecord data) {
			_pixelDB.Add(id, data.DeepCopy());
		}

		/// <summary>
		/// DBから指定した識別番号のレコードを返す
		/// </summary>
		/// <returns>データ</returns>
		/// <param name="id">識別番号</param>
		public PixelRecord Get(byte id) {
			return _pixelDB[id].DeepCopy();
		}

		/// <summary>
		/// 指定した識別番号のデータを持つピクセルデータを返す
		/// </summary>
		/// <returns>ピクセルデータ</returns>
		/// <param name="id">識別番号</param>
		public Pixel ToPixel(byte id) {
			var record = _pixelDB[id];
			return new Pixel(record.id, record.durability, record.color, record.isDraw);
		}
	}
}