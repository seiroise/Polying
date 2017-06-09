using System;

namespace Common.PixelTerrain {

	/// <summary>
	/// ピクセル地形の作成を行う人
	/// </summary>
	public abstract class PixelTerrainBuilder {

		protected readonly PixelTerrain _terrain;
		protected readonly float _chunkSize;
		protected readonly int _pixelNum;

		public PixelTerrainBuilder(PixelTerrain terrain) {
			_terrain = terrain;
			_chunkSize = _terrain.chunkSize;
			_pixelNum = _terrain.pixelNum;
		}

		/// <summary>
		/// 指定されたインデックスのチャンクの識別番号配列を作成する
		/// </summary>
		/// <returns>作成したチャンクの識別番号配列</returns>
		/// <param name="index">インデックス</param>
		public abstract int[,] MakeIDArray(XYIndex index);
	}
}