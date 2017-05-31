using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.PixelTerrain {

	/// <summary>
	/// ピクセル
	/// </summary>
	public class Pixel {

		public byte id;
		public int durability;
		public Color32 color;
		public bool isDraw;

		public Pixel(byte type, int durability, Color32 color, bool isDraw) {
			this.id = type;
			this.durability = durability;
			this.color = color;
			this.isDraw = isDraw;
		}
	}

	/// <summary>
	/// ピクセルの塊のデータ
	/// </summary>
	public class PixelChunkData {

		//それぞれ右、下、左、上の順番
		private static readonly int _expDir = 4;
		private static readonly int[,] _expDirs = { { 1, 0 }, { 0, -1 }, { -1, 0 }, { 0, 1 } };
		private static readonly float[,] _expOffset = { { 1f, 1f }, { 1f, 0f }, { 0f, 0f }, { 0f, 1f } };

		private int _size;			//二次元配列の辺の大きさ
		private int _draws;			//描画する必要のあるピクセル数
		private Pixel[,] _pixels;	//ピクセルごとのデータ
		private PixelDB _db;		//ピクセル情報のDB
		private bool _isUpdated;	//更新されているか

		public bool  isUpdated {
			get {
				return _isUpdated;
			}
		}

		private PixelChunkData(int size, byte[,] pixels, PixelDB db) {
			_size = size;
			//一回り大きい2次元配列を作成する
			_pixels = new Pixel[size + 2, size + 2];
			for(int x = 0; x < size + 2; ++x) {
				for(int y = 0; y < size + 2; ++y) {
					_pixels[x, y] = db.ToPixel(0);
				}
			}
			//実際のピクセル値を設定
			int cnt = 0;
			for(int x = 0; x < size; ++x) {
				for(int y = 0; y < size; ++y) {
					_pixels[x + 1, y + 1] = db.ToPixel(pixels[x, y]);
					if(_pixels[x + 1, y + 1].isDraw) {
						++cnt;
					}
				}
			}
			_draws = cnt;
			_db = db;
		}

		/// <summary>
		/// 指定した座標のピクセルデータを返す
		/// </summary>
		/// <returns>ピクセルデータ</returns>
		/// <param name="x">x座標</param>
		/// <param name="y">y座標</param>
		private Pixel PixelAt(int x, int y) {
			return _pixels[x + 1, y + 1];
		}

		/// <summary>
		/// ランダムなピクセルチャンクを作成する
		/// </summary>
		/// <returns>ピクセルチャンク</returns>
		/// <param name="size">大きさ</param>
		public static PixelChunkData MakeRandom(int size, int min, int max, PixelDB db) {
			var pixels = new byte[size, size];
			for(int x = 0; x < size; ++x) {
				for(int y = 0; y < size; ++y) {
					pixels[x, y] = (byte)UnityEngine.Random.Range(min, max);
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
			return new PixelChunkData(size, new byte[size, size], db);
		}

		/// <summary>
		/// 1で埋め尽くされたピクセルチャンクを作成する
		/// </summary>
		/// <returns>ピクセルチャンク</returns>
		/// <param name="size">大きさ</param>
		public static PixelChunkData MakeOne(int size, PixelDB db) {
			var pixels = new byte[size, size];
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
		public static PixelChunkData FromSquareArray(byte[,] squareArray, PixelDB db) {
			if(squareArray.GetLength(0) != squareArray.GetLength(1)) {
				throw new ArgumentException("pixels is not square");
			}
			int size = squareArray.GetLength(0);
			return new PixelChunkData(size, squareArray, db);
		}

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
			Pixel pixel;
			Color32 color;

			int x, y;
			for(x = 0; x < _size; ++x) {
				for(y = 0; y < _size; ++y) {
					pixel = PixelAt(x, y);
					if(pixel.isDraw) {

						basePos.x = x * pixelSize;
						basePos.y = y * pixelSize;

						baseI4 = cnt * 4;
						baseI6 = cnt * 6;

						color = pixel.color;

						positions[baseI4] = basePos + new Vector3(0f, 1f) * pixelSize;
						positions[baseI4 + 1] = basePos + new Vector3(1f, 1f) * pixelSize;
						positions[baseI4 + 2] = basePos + new Vector3(1f, 0f) * pixelSize;
						positions[baseI4 + 3] = basePos + new Vector3(0f, 0f) * pixelSize;

						uvs[baseI4] = new Vector2(0f, 1f);
						uvs[baseI4 + 1] = new Vector2(1f, 1f);
						uvs[baseI4 + 2] = new Vector2(1f, 0f);
						uvs[baseI4 + 3] = new Vector2(0f, 0f);

						colors[baseI4] = color;
						colors[baseI4 + 1] = color;
						colors[baseI4 + 2] = color;
						colors[baseI4 + 3] = color;

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

		/// <summary>
		/// 指定した座標の識別番号を返す
		/// </summary>
		/// <returns>識別番号</returns>
		/// <param name="x">ｘ座標</param>
		/// <param name="y">y座標</param>
		public byte IDAt(int x, int y) {
			return _pixels[x + 1, y + 1].id;
		}

		/// <summary>
		/// 指定した座標の耐久値を返す
		/// </summary>
		/// <returns>耐久値</returns>
		/// <param name="x">x座標</param>
		/// <param name="y">y座標</param>
		public int DurabilityAt(int x, int y) {
			return _pixels[x + 1, y + 1].durability;
		}

		/// <summary>
		/// 指定した座標の耐久値を変更する。その座標のピクセルが変化した場合はtrueを返す。
		/// </summary>
		/// <returns>変化した場合はtrueを返す</returns>
		/// <param name="x">x座標</param>
		/// <param name="y">y座標</param>
		/// <param name="durability">新しい耐久値</param>
		public bool DurabilityAt(int x, int y, int durability) {
			var pixel = _pixels[x + 1, y + 1];
			var temp = pixel.durability;
			pixel.durability = durability;

			//更新フラグ
			if(pixel.isDraw) {
				_isUpdated = true;
			}

			bool flag = false;
			if(temp > 0 && durability <= 0) {
				//ピクセルの状態を変化
				var toID = _db.Get(pixel.id).toID;
				//idが変わるか確認
				if(pixel.id != toID) {
					var flagTemp = pixel.isDraw;
					pixel = _db.ToPixel(toID);
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
		/// 指定した座標の耐久値を指定した値だけ加算する
		/// </summary>
		/// <param name="x">x座標</param>
		/// <param name="y">y座標</param>
		/// <param name="add">加算値</param>
		public void AddDurability(int x, int y, int add) {
			DurabilityAt(x, y, DurabilityAt(x, y) + add);
		}

		/// <summary>
		/// 指定した座標の耐久値を指定した値だけ減らす
		/// </summary>
		/// <returns>座標の耐久値が0になった場合true</returns>
		/// <param name="x">x座標</param>
		/// <param name="y">y座標</param>
		/// <param name="excavation">減らす値</param>
		/// <param name="id">減らした座標の識別番号</param>
		public bool Excavate(int x, int y, int excavation, out byte id) {
			id = IDAt(x, y);
			return DurabilityAt(x, y, DurabilityAt(x, y) - excavation);
		}
	}
}