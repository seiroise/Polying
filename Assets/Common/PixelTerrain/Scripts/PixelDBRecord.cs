using UnityEngine;
using System;

namespace Common.PixelTerrain {

	/// <summary>
	/// ピクセルDBのレコード
	/// </summary>
	[Serializable]
	public class PixelDBRecord : ICloneable<PixelDBRecord> {

		public int id;			//識別番号
		public string name;		//名前
		public int durability;	//耐久値
		public int toID;		//耐久値が0以下になった場合の識別番号
		public Color32 color;	//色
		public bool isDraw;		//描画する必要があるか
		public Vector2[] uvs;	//描画するテクスチャのuv

		private PixelDBRecord(int id, string name) {
			this.id = id;
			this.name = name;
			this.durability = 0;
			this.toID = 0;
			this.color = new Color32(0, 0, 0, 0);
			this.isDraw = false;
			this.uvs = null;
		}

		/// <summary>
		/// 描画の必要ないピクセル
		/// </summary>
		/// <returns>The pixel.</returns>
		/// <param name="id">Identifier.</param>
		/// <param name="name">Name.</param>
		public static PixelDBRecord MakePixel(int id, string name) {
			return new PixelDBRecord(id, name);
		}

		/// <summary>
		/// 描画が必要なピクセル
		/// </summary>
		/// <returns>The draw pixel.</returns>
		/// <param name="id">Identifier.</param>
		/// <param name="name">Name.</param>
		/// <param name="durability">Durability.</param>
		/// <param name="toID">To identifier.</param>
		/// <param name="color">Color.</param>
		public static PixelDBRecord MakeDrawPixel(int id, string name, int durability, int toID, Color32 color) {
			var rec = new PixelDBRecord(id, name);
			rec.durability = durability;
			rec.toID = toID;
			rec.color = color;
			rec.isDraw = true;
			rec.SetUV(Vector2.zero, Vector2.one);

			return rec;
		}

		/// <summary>
		/// 描画が必要なピクセル
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="name">Name.</param>
		/// <param name="durability">Durability.</param>
		/// <param name="toID">To identifier.</param>
		/// <param name="color">Color.</param>
		/// <param name="uvStart">Uv start.</param>
		/// <param name="uvSize">Uv size.</param>
		public static PixelDBRecord MakeDrawPixel(int id, string name, int durability, int toID, Color32 color, Vector2 uvStart, Vector2 uvSize) {
			var rec = new PixelDBRecord(id, name);
			rec.durability = durability;
			rec.toID = toID;
			rec.color = color;
			rec.isDraw = true;
			rec.SetUV(uvStart, uvSize);

			return rec;
		}

		/// <summary>
		/// UVの設定
		/// </summary>
		/// <param name="start">始点</param>
		/// <param name="size">大きさ</param>
		public void SetUV(Vector2 start, Vector2 size) {
			if(uvs == null || uvs.Length != 4) {
				uvs = new Vector2[4];
			}
			uvs[0] = start;
			uvs[1] = start + new Vector2(size.x, 0f);
			uvs[2] = start + new Vector2(0f, size.y);
			uvs[3] = start + size;
		}

		/// <summary>
		/// コピーを返す
		/// </summary>
		/// <returns>コピー</returns>
		public PixelDBRecord Clone() {
			return (PixelDBRecord)MemberwiseClone();
		}
	}
}