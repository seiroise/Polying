using UnityEngine;
using System.Collections.Generic;

namespace Polying.World.Explorer {

	/// <summary>
	/// 探索ノード
	/// </summary>
	public class ExploringNode {

		/// <summary>
		/// リンクのノードとの接続情報
		/// </summary>
		public class LinkJoint {
			private ExploringLink _link;	//リンク
			private Vector2 _dir;			//方向

			public ExploringLink link {
				get {
					return _link;
				}
			}
			public Vector2 dir {
				get {
					return _dir;
				}
			}

			public LinkJoint(ExploringLink link, Vector2 dir) {
				_link = link;
				_dir = dir;
			}
		}

		private Vector2 _point;				//座標
		private List<LinkJoint>	_joints;	//リンクとの接続情報

		public Vector2 point {
			get {
				return _point;
			}
		}

		public ExploringNode(Vector2 point) {
			this._point = point;
			this._joints = new List<LinkJoint>();
		}

		/// <summary>
		/// リンクの追加
		/// </summary>
		/// <param name="link">Link.</param>
		/// <param name="linkDir">Link dir.</param>
		public void AddLink(ExploringLink link, Vector2 linkDir) {
			_joints.Add(new LinkJoint(link, linkDir));
		}

		/// <summary>
		/// 指定した方向に最も近いジョイントを返す
		/// </summary>
		/// <returns>The dir joint.</returns>
		/// <param name="dir">Dir.</param>
		public LinkJoint GetNearestJoint(Vector2 dir) {
			float min = float.MaxValue, temp;
			int minI = -1;

			for(int i = 0; i < _joints.Count; ++i) {
				temp = (_joints[i].dir - dir).magnitude;
				if(min > temp) {
					min = temp;
					minI = i;
				}
			}

			if(minI == -1) return null;
			return _joints[minI];
		}
	}
}