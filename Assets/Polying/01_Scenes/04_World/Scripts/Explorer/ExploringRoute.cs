using System;
using System.Collections.Generic;

namespace Polying.World.Explorer {

	/// <summary>
	/// 探索ルート
	/// </summary>
	public class ExploringRoute {

		public List<ExploringNode> routeNodes;	//ルートを構成するノード

		public ExploringRoute() {
			routeNodes = new List<ExploringNode>();
		}
	}
}