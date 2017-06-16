using UnityEngine;
using System.Collections.Generic;

namespace Polying.World.Explorer {

	/// <summary>
	/// 探索リンク
	/// </summary>
	public class ExploringLink {

		public ExploringNode[] nodes;	//探索ノード
		public float eval;				//リンクの評価値
		public float distance;			//ノード間の距離
		public bool explored;			//探索済みフラグ

		public ExploringLink(ExploringNode node, float eval) {
			this.nodes = new ExploringNode[2];
			this.nodes[0] = node;
			this.eval = eval;
			this.explored = false;
		}
	}
}