using UnityEngine;
using System.Collections.Generic;
using Common.Common;
using System;

namespace Common.PixelTerrain {

	/// <summary>
	/// x, yの添字
	/// </summary>
	public struct XYIndex {

		public int x;
		public int y;

		public XYIndex(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}

	/// <summary>
	/// 変化したピクセルのデータ
	/// </summary>
	public struct ChangedPixelData {
		
		public Vector2 point;
		public int id;

		public ChangedPixelData(Vector2 point, int id) {
			this.point = point;
			this.id = id;
		}
	}

	/// <summary>
	/// レイのヒットしたピクセルに関する情報
	/// </summary>
	public class HitPixelInfo {
		public PixelData pixel;
		public Vector2 pixelPoint;
		public Vector2 direction;
		public float distance;

		public HitPixelInfo(PixelData pixel, Vector2 pixelPoint, Vector2 direction, float distance) {
			this.pixel = pixel;
			this.pixelPoint = pixelPoint;
			this.direction = direction;
			this.distance = distance;
		}
	}

	/// <summary>
	/// ピクセル地形
	/// </summary>
	[RequireComponent(typeof(PixelChunkPool))]
	public class PixelTerrain : SingletonMonoBehaviour<PixelTerrain> {

		/// <summary>
		/// 有効なチャンクのデータとチャンク
		/// </summary>
		public class ActiveChunk {
			
			public PixelChunkData data;
			public PixelChunk chunk;

			public ActiveChunk(PixelChunkData data, PixelChunk chunk) {
				this.data = data;
				this.chunk = chunk;
			}
		}

		[Header("Terrain Parameter")]
		[SerializeField, Range(1f, 128f)]
		private float _chunkSize = 8f;			//チャンクの大きさ
		[SerializeField, Range(1, 128)]
		private int _pixelNum = 8;				//チャンク内の縦横のピクセル数

		[Header("PixelDBData")]
		[SerializeField]
		private PixelDBDataObject _dbData;

		private PixelChunkPool _pixelChunkPool;	//オブジェクトのプール

		private PixelDB _pixelDB;				//ピクセルDB
		private PixelTerrainBuilder _builder;	//地形の作成

		private float _pixelSize;				//ピクセルの大きさ

		private Dictionary<XYIndex, PixelChunkData> _chunks;	//全てのチャンクデータ
		private Dictionary<XYIndex, ActiveChunk> _showChunks;	//表示されているチャンクデータ

		private List<ChangedPixelData> _changes;				//変化したピクセルのデータ

		public float chunkSize {
			get {
				return _chunkSize;
			}
		}
		public int pixelNum {
			get {
				return _pixelNum;
			}
		}
		public PixelDB pixelDB {
			get {
				return _pixelDB;
			}
			set {
				_pixelDB = value;
			}
		}
		public bool isExcavated {
			get {
				return _changes.Count > 0;
			}
		}

		protected override void SingletonAwake() {
			_pixelChunkPool = GetComponent<PixelChunkPool>();

			if(_dbData) {
				_pixelDB = _dbData.MakeDB();
			} else {
				_pixelDB = PixelDB.MakeDefault();
			}
			_builder = new NoiseBuilder(this);

			_pixelSize = _chunkSize / _pixelNum;
			_chunks = new Dictionary<XYIndex, PixelChunkData>();
			_showChunks = new Dictionary<XYIndex, ActiveChunk>();
			_changes = new List<ChangedPixelData>();
		}

		#region Chunk Function

		/// <summary>
		/// 指定したインデックスのチャンクデータを作成する
		/// </summary>
		/// <returns>作成したチャンクデータ</returns>
		private PixelChunkData MakeChunk(XYIndex index) {
			PixelChunkData chunkData;
			var idArray = _builder.MakeIDArray(index);
			chunkData = PixelChunkData.MakeSquareArray(idArray, _pixelDB);
			_chunks.Add(index, chunkData);
			return chunkData;
		}

		/// <summary>
		/// 指定したインデックスに対応するチャンクを返す
		/// </summary>
		/// <returns>インデックスに対応するチャンク</returns>
		/// <param name="index">インデックス</param>
		private PixelChunkData GetChunk(XYIndex index) {
			PixelChunkData chunk;
			if(!_chunks.TryGetValue(index, out chunk)) {
				chunk = MakeChunk(index);
			}
			return chunk;
		}

		/// <summary>
		/// 指定したインデックスのチャンクを表示する
		/// </summary>
		/// <param name="index">インデックス</param>
		public void ShowChunk(XYIndex index) {
			var pixelChunk = _pixelChunkPool.Get();
			var chunkData = GetChunk(index);
			_showChunks.Add(index, new ActiveChunk(chunkData, pixelChunk));
			pixelChunk.pixelSize = _pixelSize;
			pixelChunk.gameObject.SetActive(true);
			pixelChunk.SetPixelChunkData(chunkData, _dbData.material);
			pixelChunk.transform.localPosition = new Vector3(index.x * _chunkSize, index.y * _chunkSize);
		}

		/// <summary>
		/// 指定したインデックスのチャンクを非表示にする
		/// </summary>
		/// <param name="index">インデックス</param>
		public void HideChunk(XYIndex index) {
			if(_showChunks.ContainsKey(index)) {
				var activeChunk = _showChunks[index];
				activeChunk.chunk.gameObject.SetActive(false);
				_showChunks.Remove(index);
			}
		}

		#endregion

		#region Pixel Function

		/// <summary>
		/// 指定した座標のピクセルIDを変更する
		/// </summary>
		/// <param name="point">座標</param>
		/// <param name="id">ピクセルID</param>
		public void SetPixel(Vector2 point, int id) {
			if(_pixelDB.GetRecord(id) == null) return;
			XYIndex index = PointToIndex(point);
			var chunk = GetChunk(index);
			int x, y;
			PointToPixelIndex(point.x, point.y, out x, out y);
			chunk.SetPixel(x, y, id);
		}

		/// <summary>
		/// 指定した座標のピクセルを返す。改良版
		/// </summary>
		/// <returns>The pixel fx.</returns>
		/// <param name="point">Point.</param>
		private PixelData GetPixel(Vector2 point) {
			XYIndex index = PointToIndex(point);
			PixelChunkData chunk = GetChunk(index);
			int x, y;
			PointToPixelIndex(point.x, point.y, out x, out y);
			return chunk.GetCopiedPixel(x, y);
		}

		/// <summary>
		/// 指定した座標のピクセルレコードを返す。改良版
		/// </summary>
		/// <returns>The pixel fx.</returns>
		/// <param name="point">Point.</param>
		private PixelDBRecord GetPixelRecord(Vector2 point) {
			XYIndex index = PointToIndex(point);
			PixelChunkData chunk = GetChunk(index);
			int x, y;
			PointToPixelIndex(point.x, point.y, out x, out y);
			return _pixelDB.GetRecord(chunk.GetPixelID(x, y));
		}

		/// <summary>
		/// 指定した座標のピクセルの耐久値を減らす
		/// </summary>
		/// <param name="point">座標</param>
		/// <param name="excavation">減少値</param>
		public void ReduceDurability(Vector2 point, int excavation) {
			XYIndex index = PointToIndex(point);
			var chunk = GetChunk(index);
			int x, y;
			PointToPixelIndex(point.x, point.y, out x, out y);
			int id;
			if(chunk.ReduceDurability(x, y, excavation, out id)) {
				_changes.Add(new ChangedPixelData(point, id));
			}
		}

		/// <summary>
		/// 指定した円の範囲の耐久値を減らす
		/// </summary>
		/// <param name="point">中心座標</param>
		/// <param name="radius">半径</param>
		/// <param name="excavation">減少値</param>
		public void ReduceDurabilityInCircle(Vector2 point, float radius, int excavation) {
			//radius
			float minx = point.x - radius;
			float miny = point.y - radius;
			float maxx = minx + radius * 2;
			float maxy = miny + radius * 2;
			Vector2 p = Vector2.zero;
			for(float x = minx; x < maxx; x += _pixelSize) {
				for(float y = miny; y < maxy; y += _pixelSize) {
					p.x = x;
					p.y = y;
					if(Vector2.Distance(point, p) <= radius) {
						ReduceDurability(p, excavation);
					}
				}
			}
		}

		/// <summary>
		/// 変化したピクセルのデータ配列を返す
		/// </summary>
		/// <returns>地形データ配列</returns>
		public ChangedPixelData[] GetChanges() {
			var temp = _changes.ToArray();
			_changes.Clear();
			return temp;
		}

		/// <summary>
		/// ピクセル版のレイ
		/// </summary>
		/// <returns>The ray.</returns>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="distance">Distance.</param>
		/// <param name="hitInfo">Hit info.</param>
		public bool Ray(Vector2 origin, Vector2 direction, float distance, out HitPixelInfo hitInfo) {
			Vector2 normRay = direction.normalized * _pixelSize;
			float expDis = 0f;
			Vector2 point = origin;
			hitInfo = null;

			for(; expDis < distance; expDis += _pixelSize) {
				point.x += normRay.x;
				point.y += normRay.y;
				var pixel = GetPixel(point);
				if(pixel.isDraw) {
					//衝突
					hitInfo = new HitPixelInfo(pixel, point, direction.normalized, Vector2.Distance(origin, point));
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// ピクセル版のレイ
		/// </summary>
		/// <returns>The ray.</returns>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="distance">Distance.</param>
		public bool Ray(Vector2 origin, Vector2 direction, float distance) {
			Vector2 normRay = direction.normalized * _pixelSize;
			float expDis = 0f;
			Vector2 point = origin;

			for(; expDis < distance; expDis += _pixelSize) {
				point.x += normRay.x;
				point.y += normRay.y;
				var pixelRec = GetPixelRecord(point);
				if(pixelRec.isDraw) {
					return true;
				}
			}
			return false;
		}

		#endregion

		#region Convert Function

		/// <summary>
		/// 座標からインデックスに変換する
		/// </summary>
		/// <param name="x">x座標</param>
		/// <param name="y">y座標</param>
		/// <param name="chunkx">チャンクx座標</param>
		/// <param name="chunky">チャンクy座標</param>
		public void PointToIndex(float x, float y, out int chunkx, out int chunky) {
			chunkx = Mathf.FloorToInt(x / _chunkSize);
			chunky = Mathf.FloorToInt(y / _chunkSize);
		}

		/// <summary>
		/// 座標からインデックスに変換する
		/// </summary>
		/// <returns>インデックス</returns>
		/// <param name="point">座標</param>
		public XYIndex PointToIndex(Vector2 point) {
			return new XYIndex(
				Mathf.FloorToInt(point.x / _chunkSize),
				Mathf.FloorToInt(point.y / _chunkSize));
		}

		/// <summary>
		/// 座標からピクセルインデックスに変換する
		/// </summary>
		/// <param name="x">x座標</param>
		/// <param name="y">y座標</param>
		/// <param name="pixelx">ピクセルx座標</param>
		/// <param name="pixely">ピクセルy座標</param>
		public void PointToPixelIndex(float x, float y, out int pixelx, out int pixely) {
			int px = Mathf.FloorToInt(x / _pixelSize) % _pixelNum;
			int py = Mathf.FloorToInt(y / _pixelSize) % _pixelNum;
			pixelx = px >= 0 ? px : px + _pixelNum;
			pixely = py >= 0 ? py : py + _pixelNum;
		}

		/// <summary>
		/// 座標からピクセルインデックスに変換する
		/// </summary>
		/// <returns>ピクセルインデックス</returns>
		/// <param name="point">座標</param>
		public XYIndex PointToPixelIndex(Vector2 point) {
			int px = Mathf.FloorToInt(point.x / _pixelSize) % _pixelNum;
			int py = Mathf.FloorToInt(point.y / _pixelSize) % _pixelNum;
			return new XYIndex(
				px >= 0 ? px : px + _pixelNum,
				py >= 0 ? py : py + _pixelNum);
		}

		#endregion

		#region Utility Function

		/// <summary>
		/// 表示されているチャンクのインデックス配列を取得する
		/// </summary>
		/// <returns>インデックス配列</returns>
		public XYIndex[] GetShowChunkIndices() {
			XYIndex[] indices = new XYIndex[_showChunks.Count];
			var i = 0;
			foreach(var index in _showChunks.Keys) {
				indices[i] = index;
				++i;
			}
			return indices;
		}

		/// <summary>
		/// 指定した座標に構造物を建てる
		/// </summary>
		/// <param name="structure">構造物データ</param>
		public void BuildStructure(Vector2 point, StructureData structure) {
			int xlen = structure.idArray.GetLength(0);
			int ylen = structure.idArray.GetLength(1);
			Vector2 offset = Vector2.zero;

			for(int x = 0; x < xlen; ++x) {
				for(int y = 0; y < ylen; ++y) {
					offset.y += _pixelSize;
					SetPixel(point + offset, structure.idArray[x, y]);
				}
				offset.x += _pixelSize;
			}
		}

		#endregion
	}
}