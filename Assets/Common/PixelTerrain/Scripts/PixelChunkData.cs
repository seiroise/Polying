using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.PixelTerrain {

	/// <summary>
	/// ピクセルの塊のデータ
	/// </summary>
	public class PixelChunkData {

		//それぞれ右、下、左、上の順番
		private static readonly int _expDir = 4;
		private static readonly int[,] _expDirs = { { 1, 0 }, { 0, -1 }, { -1, 0 }, { 0, 1 } };
		private static readonly float[,] _expOffset = { { 1f, 1f }, { 1f, 0f }, { 0f, 0f }, { 0f, 1f } };

		private int _size;				//二次元配列の辺の大きさ
		private int _draws;				//描画する必要のあるピクセル数
		private PixelData[,] _pixels;	//ピクセルごとのデータ
		private PixelDB _db;			//ピクセル情報のDB
		private bool _isUpdated;		//更新されているか

		public bool  isUpdated {
			get {
				return _isUpdated;
			}
		}

		private PixelChunkData(int size, int[,] pixels, PixelDB db) {
			_size = size;
			//一回り大きい2次元配列を作成する
			_pixels = new PixelData[size + 2, size + 2];
			for(int x = 0; x < size + 2; ++x) {
				for(int y = 0; y < size + 2; ++y) {
					_pixels[x, y] = db.IDToPixelData(0);
				}
			}
			//実際のピクセル値を設定
			int cnt = 0;
			for(int x = 0; x < size; ++x) {
				for(int y = 0; y < size; ++y) {
					_pixels[x + 1, y + 1] = db.IDToPixelData(pixels[x, y]);
					if(_pixels[x + 1, y + 1].isDraw) {
						++cnt;
					}
				}
			}
			_draws = cnt;
			_db = db;
		}

		#region Factory Function

		/// <summary>
		/// ランダムなピクセルチャンクを作成する
		/// </summary>
		/// <returns>ピクセルチャンク</returns>
		/// <param name="size">大きさ</param>
		public static PixelChunkData MakeRandom(int size, int min, int max, PixelDB db) {
			var pixels = new int[size, size];
			for(int x = 0; x < size; ++x) {
				for(int y = 0; y < size; ++y) {
					pixels[x, y] = UnityEngine.Random.Range(min, max);
				}
			}
			return new PixelChunkData(size, pixels, db);
		}

		/// <summary>
		/// 0で埋め尽くされたピクセルチャンクを作成する
		/// </summary>
		/// <returns>ピクセルチャンク</returns>
		/// <param name="size">大きさ</param>
		public static PixelChunkData MakeZero(int size, PixelDB db) {
			return new PixelChunkData(size, new int[size, size], db);
		}

		/// <summary>
		/// 1で埋め尽くされたピクセルチャンクを作成する
		/// </summary>
		/// <returns>ピクセルチャンク</returns>
		/// <param name="size">大きさ</param>
		public static PixelChunkData MakeOne(int size, PixelDB db) {
			var pixels = new int[size, size];
			for(int x = 0; x < size; ++x) {
				for(int y = 0; y < size; ++y) {
					pixels[x, y] = 1;
				}
			}
			return new PixelChunkData(size, pixels, db);
		}

		/// <summary>
		/// 正方配列からピクセルチャンクを作成する
		/// </summary>
		/// <returns>ピクセルチャンク</returns>
		/// <param name="squareArray">正方配列(x, y)</param>
		public static PixelChunkData MakeSquareArray(int[,] squareArray, PixelDB db) {
			if(squareArray.GetLength(0) != squareArray.GetLength(1)) {
				throw new ArgumentException("pixels is not square");
			}
			int size = squareArray.GetLength(0);
			return new PixelChunkData(size, squareArray, db);
		}

		#endregion

		#region Pixel Function

		/// <summary>
		/// 指定した座標のピクセルを返す
		/// </summary>
		/// <returns>ピクセル</returns>
		/// <param name="x">x座標</param>
		/// <param name="y">y座標</param>
		private PixelData GetPixel(int x, int y) {
			return _pixels[x + 1, y + 1];
		}

		/// <summary>
		/// 指定した座標のコピーしたピクセルを返す
		/// </summary>
		/// <returns>The copied pixel.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public PixelData GetCopiedPixel(int x, int y) {
			return _pixels[x + 1, y + 1].Clone();
		}

		/// <summary>
		/// 指定した座標のピクセルを変更する
		/// </summary>
		/// <param name="x">x座標</param>
		/// <param name="y">y座標</param>
		/// <param name="id">ピクセルID</param>
		public void SetPixel(int x, int y, int id) {
			_pixels[x + 1, y + 1] = _db.IDToPixelData(id);
			_isUpdated = true;
		}

		/// <summary>
		/// 指定した座標の識別番号を返す
		/// </summary>
		/// <returns>識別番号</returns>
		/// <param name="x">ｘ座標</param>
		/// <param name="y">y座標</param>
		public int GetPixelID(int x, int y) {
			return _pixels[x + 1, y + 1].id;
		}

		/// <summary>
		/// 指定した座標のピクセルの耐久値を変更する
		/// 耐久値の変更に伴ってその座標のピクセルが変化した場合はtrueを返す
		/// </summary>
		/// <returns>変化した場合はtrueを返す</returns>
		/// <param name="x">x座標</param>
		/// <param name="y">y座標</param>
		/// <param name="durability">耐久値</param>
		public bool SetPixelDurability(int x, int y, int durability) {
			var pixel = GetPixel(x, y);
			var temp = pixel.durability;
			pixel.durability = durability;

			//更新フラグ
			if(pixel.isDraw) {
				_isUpdated = true;
			}

			bool flag = false;
			if(temp > 0 && durability <= 0) {
				//ピクセルの状態を変化
				var toID = _db.GetCopiedRecord(pixel.id).toID;
				//idが変わるか確認
				if(pixel.id != toID) {
					var flagTemp = pixel.isDraw;
					pixel = _db.IDToPixelData(toID);
					if(flagTemp & !pixel.isDraw) {
						--_draws;
					} else if(!flagTemp & pixel.isDraw) {
						++_draws;
					}
					_pixels[x + 1, y + 1] = pixel;
					flag = true;
				}
			}
			return flag;
		}

		/// <summary>
		/// 指定した座標のピクセルの耐久値を減らす
		/// 耐久値の変更に伴ってその座標のピクセルが変化した場合はtrueを返す
		/// </summary>
		/// <returns>変化した場合はtrueを返す</returns>
		/// <param name="x">x座標</param>
		/// <param name="y">y座標</param>
		/// <param name="sub">減少量</param>
		public bool ReduceDurability(int x, int y, int sub, out int id) {
			var pixel = GetPixel(x, y);
			id = pixel.id;
			return SetPixelDurability(x, y, pixel.durability - sub);
		}

		#endregion

		#region Utility Function

		/// <summary>
		/// 指定したサイズのピクセルのメッシュに変換する
		/// </summary>
		/// <returns>メッシュ</returns>
		/// <param name="pixelSize">ピクセルの大きさ</param>
		public Mesh ToMesh(float pixelSize) {

			Vector3[] positions = new Vector3[_draws * 4];
			Vector2[] uvs = new Vector2[positions.Length];
			Color32[] colors = new Color32[positions.Length];
			int[] indices = new int[_draws * 12];

			int cnt = 0;
			Vector3 basePos = Vector3.zero;
			int baseI4, baseI6;
			PixelData pixel;

			PixelDBRecord pRec;
			Vector2[] pUvs;
			Color32 pColor;

			int x, y;
			for(x = 0; x < _size; ++x) {
				for(y = 0; y < _size; ++y) {
					pixel = GetPixel(x, y);
					if(pixel.isDraw) {

						basePos.x = x * pixelSize;
						basePos.y = y * pixelSize;

						baseI4 = cnt * 4;
						baseI6 = cnt * 6;

						pRec = _db.GetRecord(pixel.id);
						pColor = pRec.color;
						pUvs = pRec.uvs;

						positions[baseI4] = basePos;
						positions[baseI4 + 1] = basePos + new Vector3(0f, 1f) * pixelSize;
						positions[baseI4 + 2] = basePos + new Vector3(1f, 1f) * pixelSize;
						positions[baseI4 + 3] = basePos + new Vector3(1f, 0f) * pixelSize;

						uvs[baseI4] = pUvs[0];
						uvs[baseI4 + 1] = pUvs[1];
						uvs[baseI4 + 2] = pUvs[2];
						uvs[baseI4 + 3] = pUvs[3];

						colors[baseI4] = pColor;
						colors[baseI4 + 1] = pColor;
						colors[baseI4 + 2] = pColor;
						colors[baseI4 + 3] = pColor;

						indices[baseI6] = baseI4;
						indices[baseI6 + 1] = baseI4 + 1;
						indices[baseI6 + 2] = baseI4 + 2;

						indices[baseI6 + 3] = baseI4;
						indices[baseI6 + 4] = baseI4 + 2;
						indices[baseI6 + 5] = baseI4 + 3;

						++cnt;
					}
				}
			}

			Mesh mesh = new Mesh();
			mesh.name = "pixel chunk";
			mesh.vertices = positions;
			mesh.uv = uvs;
			mesh.colors32 = colors;
			mesh.SetIndices(indices, MeshTopology.Triangles, 0);

			mesh.RecalculateBounds();
			mesh.RecalculateNormals();

			_isUpdated = false;

			return mesh;
		}

		#endregion
	}
}