using UnityEngine;
using System;

namespace Common.PixelTerrain {

	/// <summary>
	/// ノイズからピクセル地形の作成を行う人
	/// </summary>
	public class NoiseBuilder : PixelTerrainBuilder {

		public NoiseBuilder(PixelTerrain terrain) : base(terrain) { }

		/// <summary>
		/// 指定したインデックスのチャンクデータをパーリンノイズから作成する
		/// </summary>
		/// <returns>PixelChunk</returns>
		/// <param name="sx">開始x座標</param>
		/// <param name="sy">開始y座標</param>
		/// <param name="size">大きさ</param>
		private int[,] MakeChunkFromNoise(float sx, float sy, float size) {
			float d = size / _pixelNum;
			var idArray = new int[_pixelNum, _pixelNum];
			for(int x = 0; x < _pixelNum; ++x) {
				for(int y = 0; y < _pixelNum; ++y) {
					float v = Mathf.PerlinNoise(sx + d * x, sy + d * y);
					if(v >= 0.8f) {
						//if(UnityEngine.Random.Range(0, 20) == 0) {
						//	//鉄鉱石
						//	pixels[x, y] = 10;
						//} else {
						//硬い岩
						idArray[x, y] = 12;
						//}
					} else if(v >= 0.6f) {
						//そこそこ硬い岩
						idArray[x, y] =11;
					} else if(v >= 0.5) {
						//普通の岩
						idArray[x, y] = 10;
					} else {
						//空気
						idArray[x, y] = 1;
					}
				}
			}
			return idArray;
		}

		/// <summary>
		/// 指定されたインデックスのチャンクデータを作成する
		/// </summary>
		/// <returns>作成したチャンクデータ</returns>
		/// <param name="index">インデックス</param>
		public override int[,] MakeIDArray(XYIndex index) {
			return MakeChunkFromNoise(index.x, index.y, 1f);
		}
	}
}