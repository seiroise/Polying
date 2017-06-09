using UnityEngine;
using System;

namespace Common.PixelTerrain {

	/// <summary>
	/// ピクセルデータ
	/// </summary>
	public class PixelData : ICloneable<PixelData> {

		public int id;
		public int durability;
		public Color32 color;
		public bool isDraw;

		public PixelData(int id, int durability, Color32 color, bool isDraw) {
			this.id = id;
			this.durability = durability;
			this.color = color;
			this.isDraw = isDraw;
		}

		public PixelData Clone() {
			return new PixelData(id, durability, color, isDraw);
		}
	}
}