using UnityEngine;
using System;
using System.Linq;
using Polying.Common.Controller;
using Common.PixelTerrain;
using System.Collections.Generic;

namespace Polying.World.Explorer {

	/// <summary>
	/// 地形探索ver2
	/// </summary>
	[Serializable]
	public class TerrainExplorer2 {

		/// <summary>
		/// 方向データ
		/// </summary>
		private class DirData {

			public readonly float[] roundAngles;     //円形の角度
			public readonly Vector2[] roundDirs;     //円形の方向
			public readonly float[] narrowAngles;    //狭い角度
			public readonly Vector2[] narrowDirs;    //狭い方向

			public DirData() {
				roundAngles = Functions.FloatRange(0f, 355f, 360f / 16).ToArray();
				roundDirs = Functions.AngleToDirection(roundAngles);
				narrowAngles = Functions.FloatRange(-10f, 10f, 5f).ToArray();
				narrowDirs = Functions.AngleToDirection(narrowAngles);
			}

			/// <summary>
			/// 回転させた狭い方向ベクトルを返す
			/// </summary>
			/// <returns>The rotate narrows.</returns>
			public Vector2[] GetRotateNarrows(float angle) {
				Quaternion rotate = Quaternion.Euler(0f, 0f, angle);
				Vector2[] dirs = new Vector2[narrowDirs.Length];
				for(int i = 0; i < dirs.Length; ++i) {
					dirs[i] = rotate * narrowDirs[i];
				}
				return dirs;
			}
		}

		[Header("Terrain")]
		[SerializeField]
		private PixelTerrain _terrain;

		[Header("Exploring Parameter")]
		[SerializeField, Range(1f, 100f)]
		private float _rayDistance = 40f;           //レイの距離
		[SerializeField, Range(0.01f, 10f)]
		private float _nodeEqualDistance = 1f;      //同値とみなすノードの距離
		[SerializeField, Range(0f, 100f)]
		private float _adjustDistance = 5f;			//探索する時に本来の距離から減算する値

		private static DirData _dirData;            //方向データ

		private RigidbodyController _controller;

		private ExploringMap _map;                  //探索地図
		private ExploringNode _prevNode;            //前回のノード

		public TerrainExplorer2() {
			if(_dirData == null) {
				_dirData = new DirData();
			}
		}

		/// <summary>
		/// 始点と終点から終点にたどり着くようなルートを作成する
		/// </summary>
		/// <returns>The route.</returns>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		public ExploringRoute MakeRoute(Vector2 start, Vector2 end) {
			//始点ノードの作成
			var node = MakeNode(start);

			Vector2 dir = end - start;
			var joint = node.GetNearestJoint(dir);
			// 本当にリンクの方向にいけるか確認する
			var narrowDirs = _dirData.GetRotateNarrows(Functions.VectorToDeg(dir));
			var avgEval = GetAroundAvgEval(node.point, narrowDirs);

			return null;
		}

		/// <summary>
		/// 指定した座標のノードを作成する
		/// </summary>
		/// <returns>The node.</returns>
		/// <param name="point">Point.</param>
		private ExploringNode MakeNode(Vector2 point) {
			//地図に現在地のノードが含まれているか
			ExploringNode node;
			if(_map.TryGetValue(point, _nodeEqualDistance, out node)) {
				return node;
			}
			//レイを射つ
			node = new ExploringNode(point);
			return node;
		}

		/// <summary>
		/// 次に進むべき方向を返す
		/// </summary>
		/// <returns>The next move direction.</returns>
		/// <param name="point">Point.</param>
		private Vector2 GetNextMoveDirection(Vector2 point) {
			return Vector2.zero;
		}

		/// <summary>
		/// HitPixelInfoから評価値を求める
		/// </summary>
		/// <returns>The evaluate.</returns>
		/// <param name="hitInfo">Hit info.</param>
		private float Evaluate(HitPixelInfo hitInfo) {
			if(hitInfo != null) {
				return hitInfo.distance / _rayDistance;
			} else {
				return 1f;
			}
		}

		/// <summary>
		/// 指定した点から指定した周囲にレイキャストする
		/// </summary>
		/// <returns>The around.</returns>
		/// <param name="origin">Origin.</param>
		/// <param name="dirs">Dirs.</param>
		private HitPixelInfo[] RayAround(Vector2 origin, Vector2[] dirs) {
			HitPixelInfo[] hitInfos = new HitPixelInfo[dirs.Length];
			for(int i = 0; i < dirs.Length; ++i) {
				_terrain.Ray(origin, dirs[i], _rayDistance, out hitInfos[i]);
			}
			return hitInfos;
		}

		/// <summary>
		/// 指定した点から指定した周囲にレイキャストした結果から平均の距離に関する評価値を求める
		/// </summary>
		/// <returns>The around avg eval.</returns>
		/// <param name="origin">Origin.</param>
		/// <param name="dirs">Dirs.</param>
		private float GetAroundAvgEval(Vector2 origin, Vector2[] dirs) {
			float sum = 0f;
			var hitInfos = RayAround(origin, dirs);
			for(int i = 0; i < dirs.Length; ++i) {
				sum += Evaluate(hitInfos[i]);
			}
			return sum / dirs.Length;
		}
	}
}