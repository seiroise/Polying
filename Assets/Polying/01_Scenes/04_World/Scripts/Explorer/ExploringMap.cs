using UnityEngine;
using System.Collections.Generic;

namespace Polying.World.Explorer {

	/// <summary>
	/// 探索地図
	/// </summary>
	public class ExploringMap {
		
		private HashSet<ExploringNode> _nodeSet;

		public ExploringMap() {
			_nodeSet = new HashSet<ExploringNode>();
		}

		/// <summary>
		/// 指定した座標のノードが含まれているか確認する
		/// </summary>
		/// <returns>The contains.</returns>
		public bool Contains(Vector2 point, float distance) {
			foreach(var node in _nodeSet) {
				if(Vector2.Distance(point, node.point) < distance) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 指定した座標のノードを取得する
		/// </summary>
		/// <returns><c>true</c>, if get value was tryed, <c>false</c> otherwise.</returns>
		/// <param name="point">Point.</param>
		/// <param name="distance">Distance.</param>
		public bool TryGetValue(Vector2 point, float distance, out ExploringNode node) {
			node = null;
			float minDis = distance, dis;
			foreach(var n in _nodeSet) {
				dis = Vector2.Distance(point, n.point);
				if(dis < minDis) {
					minDis = dis;
					node = n;
				}
			}
			return node != null;
		}
	}
}