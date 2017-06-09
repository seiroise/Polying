using UnityEngine;
using System.Collections.Generic;

namespace Polying.World {

	/// <summary>
	/// 探索ノード
	/// </summary>
	public class ExploringNode {

		public Vector2 point;					//座標
		public List<ExploringNode> neighbours;	//隣接ノード

		public ExploringNode(Vector2 point) {
			this.point = point;
			neighbours = new List<ExploringNode>();
		}
	}
}