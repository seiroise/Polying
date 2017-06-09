using UnityEngine;
using System;
using EditorUtil;
using System.IO;

namespace Common.PixelTerrain {

	/// <summary>
	/// レコードのサムネイル
	/// </summary>
	[Serializable]
	public class RecordThumbnail {
		public Texture2D icon;
		public string name;
		public PixelDBRecord record;
	}

	/// <summary>
	/// ピクセルDBのデータオブジェクト
	/// </summary>
	public class PixelDBDataObject : ScriptableObject {

		/// <summary>
		/// 保存用
		/// </summary>
		[Serializable]
		private class PixelRecords {
			public PixelDBRecord[] _records;

			public PixelRecords(PixelDBRecord[] records) {
				_records = records;
			}
		}

		[Header("Material")]
		[SerializeField]
		private Material _material;
		[Header("Texture")]
		[SerializeField]
		private Texture2D _texture;

		[Header("Records")]
		[SerializeField]
		private string _jsonFile = "PixelDB.json";		//読み込みjsonファイル名

		[SerializeField]
		private PixelDBRecord[] _records;	//レコード

		public Material material {
			get {
				return _material;
			}
		}

		/// <summary>
		/// 初期レコードから作成したDBを返す
		/// </summary>
		/// <returns>データベース</returns>
		public PixelDB MakeDB() {
			var db = new PixelDB();
			db.AddRecords(_records);
			return db;
		}

		/// <summary>
		/// レコードをjsonにして保存
		/// </summary>
		/// <param name="filePath">ファイルパス</param>
		private void SaveJsonFromRecords(string filePath) {
			using(TextWriter w = new StreamWriter(filePath)) {
				var json = JsonUtility.ToJson(new PixelRecords(_records), true);
				w.Write(json);
			}
		}

		/// <summary>
		/// jsonからレコードを読み込み
		/// </summary>
		/// <param name="filePath">ファイルパス</param>
		private PixelRecords LoadRecordsFromJson(string filePath) {
			PixelRecords recs;
			using(TextReader r = new StreamReader(filePath)) {
				var json = r.ReadToEnd();
				recs = JsonUtility.FromJson<PixelRecords>(json);
			}
			return recs;
		}

		/// <summary>
		/// セーブ
		/// </summary>
		public void SaveJson() {
			var path = Application.streamingAssetsPath + "/" + _jsonFile;
			Debug.Log("Save json: " + path);
			SaveJsonFromRecords(path);
		}

		/// <summary>
		/// ロード
		/// </summary>
		public void LoadJson() {
			var path = Application.streamingAssetsPath + "/" + _jsonFile;
			Debug.Log("Load json: " + path);
			_records = LoadRecordsFromJson(path)._records;
		}
	}
}