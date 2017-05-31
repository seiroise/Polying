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
	/// 掘られた地形データ
	/// </summary>
	public struct Excavated {
		
		public Vector2 point;
		public byte id;

		public Excavated(Vector2 point, byte id) {
			this.point = point;
			this.id = id;
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

		private PixelChunkPool _pixelChunkPool;	//プール

		private float _pixelSize;				//ピクセルの大きさ
		private PixelDB _pixelDB;				//ピクセルDB

		private Dictionary<XYIndex, PixelChunkData> _chunks;	//全てのチャンクデータ
		private Dictionary<XYIndex, ActiveChunk> _showChunks;	//表示されているチャンクデータ

		private List<Excavated> _excavates;						//掘ったピクセルのデータ

		public PixelDB pixelDB {
			get {
				return _pixelDB;
			}
		}
		public bool isExcavated {
			get {
				return _excavates.Count > 0;
			}
		}

		protected override void SingletonAwake() {
			_pixelChunkPool = GetComponent<PixelChunkPool>();

			_pixelSize = _chunkSize / _pixelNum;
			_pixelDB = PixelDB.MakeDefault();

			_chunks = new Dictionary<XYIndex, PixelChunkData>();
			_showChunks = new Dictionary<XYIndex, ActiveChunk>();
			_excavates = new List<Excavated>();
		}

		/// <summary>
		/// 指定したインデックスのチャンクデータをパーリンノイズから作成する
		/// </summary>
		/// <returns>PixelChunk</returns>
		/// <param name="sx">開始x座標</param>
		/// <param name="sy">開始y座標</param>
		/// <param name="size">大きさ</param>
		private PixelChunkData MakeChunkFromNoise(float sx, float sy, float size) {
			float d = size / _pixelNum;
			byte[,] pixels = new byte[_pixelNum, _pixelNum];
			for(int x = 0; x < _pixelNum; ++x) {
				for(int y = 0; y < _pixelNum; ++y) {
					float v = Mathf.PerlinNoise(sx + d * x, sy + d * y);
					if(v >= 0.8f) {
						if(UnityEngine.Random.Range(0, 20) == 0) {
							//鉄鉱石
							pixels[x, y] = 10;
						} else {
							//硬い岩
							pixels[x, y] = 3;
						}
					} else if(v >= 0.6f) {
						//そこそこ硬い岩
						pixels[x, y] = 2;
					} else if(v >= 0.5){
						//普通の岩
						pixels[x, y] = 1;
					} else {
						//空間
						pixels[x, y] = 0;
					}
				}
			}
			return PixelChunkData.FromSquareArray(pixels, _pixelDB);
		}

		/// <summary>
		/// 指定したインデックスのチャンクデータをパーリンノイズから作成する
		/// </summary>
		/// <param name="index">インデックス</param>
		private PixelChunkData MakeChunkFromNoise(XYIndex index) {
			var chunkData = MakeChunkFromNoise(index.x, index.y, 1f);
			_chunks.Add(index, chunkData);
			return chunkData;
		}

		/// <summary>
		/// 指定したインデックスの指定したデータのPixelChunkを表示する
		/// </summary>
		/// <param name="index">添字</param>
		/// <param name="chunkData">chunkデータ</param>
		private void ShowChunk(XYIndex index, PixelChunkData chunkData) {
			var pixelChunk = _pixelChunkPool.Get();
			_showChunks.Add(index, new ActiveChunk(chunkData, pixelChunk));

			pixelChunk.pixelSize = _pixelSize;
			pixelChunk.gameObject.SetActive(true);
			pixelChunk.SetPixelChunkData(chunkData);
			pixelChunk.transform.localPosition = new Vector3(index.x * _chunkSize, index.y * _chunkSize);
		}

		/// <summary>
		/// 指定したインデックスのチャンクを表示する
		/// </summary>
		/// <param name="index">座標</param>
		public void ShowChunk(XYIndex index) {
			if(_chunks.ContainsKey(index)) {
				//読み込まれているチャンクを表示
				if(!_showChunks.ContainsKey(index)) {
					//すでに表示されていないチャンク以外
					var chunkData = _chunks[index];
					ShowChunk(index, chunkData);
				}
			} else {
				//読み込まれていないチャンクを表示
				ShowChunk(index, MakeChunkFromNoise(index));
			}
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
		/// 表示されているチャンクのインデックス配列を取得する
		/// </summary>
		/// <returns>インデックス配列</returns>
		public XYIndex[] ShowChunkIndex() {
			XYIndex[] indices = new XYIndex[_showChunks.Count];
			var i = 0;
			foreach(var index in _showChunks.Keys) {
				indices[i] = index;
				++i;
			}
			return indices;
		}

		/// <summary>
		/// 指定した座標の耐久値を減らす
		/// </summary>
		/// <param name="point">座標</param>
		/// <param name="excavation">減少値</param>
		public void Excavate(Vector2 point, int excavation) {
			XYIndex index = PointToIndex(point);
			if(!_chunks.ContainsKey(index)) {
				MakeChunkFromNoise(index);
			}
			PixelChunkData chunk = _chunks[index];
			int x = Mathf.FloorToInt(point.x / _pixelSize) % _pixelNum;
			int y = Mathf.FloorToInt(point.y / _pixelSize) % _pixelNum;
			x = x >= 0 ? x : x + _pixelNum;
			y = y >= 0 ? y : y + _pixelNum;
			//Debug.Log(string.Format("index {0}:{1}  point {2}:{3}",index.x, index.y, x, y));
			//chunk.AddDurability(x, y, add);
			byte id;
			if(chunk.Excavate(x, y, excavation, out id)) {
				_excavates.Add(new Excavated(point, id));
			}
		}

		/// <summary>
		/// 指定した円の範囲の耐久値を減らす
		/// </summary>
		/// <param name="point">中心座標</param>
		/// <param name="radius">半径</param>
		/// <param name="excavation">減少値</param>
		public void ExcavateCircle(Vector2 point, float radius, int excavation) {
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
						Excavate(p, excavation);
					}
				}
			}
		}

		/// <summary>
		/// 掘られた地形データ配列を返す
		/// </summary>
		/// <returns>地形データ配列</returns>
		public Excavated[] GetExcavated() {
			var temp = _excavates.ToArray();
			_excavates.Clear();
			return temp;
		}
	}
}