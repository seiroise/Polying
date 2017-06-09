using UnityEngine;
using System.Collections.Generic;

namespace Polying.World {

	/// <summary>
	/// 探索地図
	/// </summary>
	public class ExploringMap {
		
		private HashSet<ExploringNode> _nodeSet;

		public ExploringMap() {
			_nodeSet = new HashSet<ExploringNode>();
		}

		/// <summary>
		/// ノードの追加
		/// </summary>
		/// <param name="node">Node.</param>
		public void AddNode(ExploringNode node) {
			
		}
	}
}