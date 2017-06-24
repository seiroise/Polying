using System;
using System.Collections;
using System.Collections.Generic;
using Common.PixelTerrain;
using UnityEngine;

namespace Polying.Test {

	/// <summary>
	/// ピクセル地形の作成を行う人
	/// </summary>
	public class RogueTerrainBuilder : PixelTerrainBuilder {

		public RogueTerrainBuilder(PixelTerrain terrain) : base(terrain) {
			
		}

		private void MakeChamber() {
			
		}

		public override int[,] MakeIDArray(XYIndex index) {
			return null;
		}
	}
}