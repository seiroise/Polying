using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Common.PlaneTerrain {

	/// <summary>
	/// ユーティリティクラス
	/// </summary>
	public class PlaneTerrainUtil {

		/// <summary>
		/// 辺
		/// </summary>
		public class Edge {

			public Vector2 a;
			public Vector2 b;
			public Vector2 atob;

			public int weight;

			public float xCenter {
				get {
					return a.x + atob.x * 0.5f;
				}
			}
			public float yCenter {
				get {
					return a.y + atob.y * 0.5f;
				}
			}
			public float xMax {
				get {
					return Mathf.Max(a.x, b.x);
				}
			}
			public float yMax {
				get {
					return Mathf.Max(a.y, b.y);
				}
			}
			public float xMin {
				get {
					return Mathf.Min(a.x, b.x);
				}
			}
			public float yMin {
				get {
					return Mathf.Min(a.y, b.y);
				}
			}

			public Edge(Vector2 a, Vector2 b) {
				this.a = a;
				this.b = b;
				this.atob = b - a;
				this.weight = 0;
			}

			/// <summary>
			/// 指定したy座標の場合のx座標を求める
			/// </summary>
			/// <returns>x座標</returns>
			/// <param name="y">y座標</param>
			public float X(float y) {
				return a.x + atob.x * ((y - a.y) / atob.y);
			}
		}

		/// <summary>
		/// 辺同士のペア
		/// </summary>
		private class EdgePair {

			public Edge left;
			public Edge right;

			public bool isMarged;

			public EdgePair(Edge left, Edge right) {
				this.left = left;
				this.right = right;
				this.isMarged = false;
			}
		}

		/// <summary>
		/// 指定した頂点配列が時計回り(Clock wise)か確認する
		/// </summary>
		/// <returns>時計回りならtrueを反時計周りならfalseを返す</returns>
		/// <param name="points">頂点配列</param>
		public static bool CheckCW(Vector2[] points) {

			int len = points.Length;
			float sumTheta = 0f;

			Vector2 p1, p2, p3;
			float theta1, theta2, theta;
			for(int i = 0; i < len; ++i) {

				p1 = points[i];
				p2 = points[(i + 1) % len];
				p3 = points[(i + 2) % len];
				theta1 = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x);
				theta2 = Mathf.Atan2(p3.y - p2.y, p3.x - p2.x);
				theta = theta2 - theta1;

				if(theta > Mathf.PI) {
					theta -= Mathf.PI;
				} else if(theta < -Mathf.PI) {
					theta += Mathf.PI;
				}
				sumTheta += theta;
			}
			if(sumTheta < -Mathf.PI + 0.0001f) {
				return true;
			} else if(sumTheta > Mathf.PI - 0.0001f) {
				return false;
			}

			throw new Exception("positions is twist");
		}

		/// <summary>
		/// 二つのベクトルから外積を計算する
		/// </summary>
		/// <returns>外積</returns>
		/// <param name="a">ベクトルa</param>
		/// <param name="b">ベクトルb</param>
		public static float Cross(Vector2 a, Vector2 b) {
			return (a.x * b.y) - (a.y * b.x);
		}

		/// <summary>
		/// 二つの線分の交点を求める。交差している場合はtrueを返す
		/// </summary>
		/// <returns>交差しているならtrue</returns>
		/// <param name="l1">線分1</param>
		/// <param name="l2">線分2</param>
		/// <param name="pos">交点</param>
		public static bool Intersection(Edge l1, Edge l2, out Vector2 pos) {

			pos.y = pos.x = 0f;

			Vector2 v = l2.a - l1.a;
			float cross_l1_l2 = Cross(l1.atob, l2.atob);
			if(cross_l1_l2 == 0f) {
				//平行状態
				return false;
			}

			float cross_v_l1 = Cross(v, l1.atob);
			float cross_v_l2 = Cross(v, l2.atob);

			float t1 = cross_v_l2 / cross_l1_l2;
			float t2 = cross_v_l1 / cross_l1_l2;

			if(t1 + Mathf.Epsilon < 0f || t1 - Mathf.Epsilon > 1f ||
			   t2 + Mathf.Epsilon < 0f || t2 - Mathf.Epsilon > 1f) {
				//交差していない
				return false;
			}

			pos = l1.a + l1.atob * t1;
			return true;
		}

		/// <summary>
		/// 座標リストから冗長性(重複する座標、直線上の座標)を取り除く
		/// </summary>
		/// <param name="points">冗長性ｗｐ取り除く座標配列</param>
		public static void EliminateRedundancy(ref List<Vector2> points) {

			int len = points.Count;
			Vector2 p1, p2, p3, tmp = Vector2.zero;
			float angle;

			for(int i = len - 1; i >= 0; --i) {
				//座標の重複する頂点を取り除く
				if(Vector2.Distance(tmp, points[i]) < 0.001f) {
					points.RemoveAt(i);
					--len;
					continue;
				}
				tmp = points[i];
			}
			for(int i = len - 1; i >= 0; --i) {
				//直線上の点を取り除く
				p1 = points[i];
				p2 = points[(i + 1) % len];
				p3 = points[(i + 2) % len];
				angle = TwoVectorAngle(p2, p1, p3);
				if(angle > 179f || float.IsNaN(angle)) {
					points.RemoveAt((i + 1) % len);
					--len;
					continue;
				}
			}
		}

		/// <summary>
		/// 二つのベクトルがなす角を求める(deg)
		/// </summary>
		public static float TwoVectorAngle(Vector2 from, Vector2 to1, Vector2 to2) {
			Vector2 v0 = to1 - from, v1 = to2 - from;
			return Mathf.Acos(Vector2.Dot(v0, v1) / (v0.magnitude * v1.magnitude)) * Mathf.Rad2Deg;
		}

		/// <summary>
		/// 円形に並ぶ頂点配列を返す
		/// </summary>
		/// <returns>頂点配列</returns>
		/// <param name="radius">半径</param>
		/// <param name="div">分割数(3〜)</param>
		/// <param name="transMat">姿勢行列</param>
		/// <param name="cw">trueなら時計回り、falseなら反時計回り</param>
		public static Vector2[] MakeCirclePoints(float radius, int div, Matrix4x4 transMat, bool cw = true) {
			Vector2[] points = new Vector2[div];
			float dTheta = Mathf.PI / div * 2 * (cw ? -1f : 1f);
			for(int i = 0; i < div; ++i) {
				points[i] = (Vector2)transMat.MultiplyPoint3x4(new Vector3(Mathf.Cos(dTheta * i), Mathf.Sin(dTheta * i), 0f) * radius);
			}
			return points;
		}

		#region Boolean Operation

		/// <summary>
		/// ２つの平面図形から論理和を求める。
		/// それぞれの平面図形を構成する頂点は時計周りである必要がある。
		/// またそれぞれの頂点配列は自己交差していない必要がある。
		/// </summary>
		/// <returns>論理和の結果生成された平面図形</returns>
		/// <param name="a">平面図形aを構成する頂点配列</param>
		/// <param name="b">平面図形bを構成する頂点配列</param>
		public static List<Vector2[]> OR(Vector2[] a, Vector2[] b) {
			//辺を分割したものを取得する
			SortedList<float, List<Edge>> sortedEdges = SliceSortedEdges(a, b);
			//辺同士のペアを見つける
			List<List<EdgePair>> edgePairs = FindEdgesPairUp1Down0(sortedEdges);
			//マージ
			return MargePairs(edgePairs);
		}

		/// <summary>
		/// 平面図形aに対して平面図形bによる論理否定を求める
		/// 平面図形aの頂点は時計回り、平面図形bの頂点は反時計周りである必要がある
		/// またそれぞれの頂点配列は自己交差していない必要がある。
		/// </summary>
		/// <returns>論理否定の結果生成された平面図形</returns>
		/// <param name="a">平面図形aを構成する頂点配列</param>
		/// <param name="b">平面図形bを構成する頂点配列</param>
		public static List<Vector2[]> Not(Vector2[] a, Vector2[] b) {

			//辺を分割したものを取得する
			SortedList<float, List<Edge>> sortedEdges = SliceSortedEdges(a, b);
			//辺同士のペアを見つける
			List<List<EdgePair>> edgePairs = FindEdgesPairUp1Down0(sortedEdges);
			//マージ
			List<Vector2[]> margePoints = MargePairs(edgePairs);
			return margePoints;
		}

		/// <summary>
		/// 重み付けされた辺のなかから適した組み合わせの辺を見つける
		/// </summary>
		/// <param name="sortedEdges">座標値ごとに集められた対応する辺のペア</param>
		private static List<List<EdgePair>> FindEdgesPairUp1Down0(SortedList<float, List<Edge>> sortedEdges) {
			//リスト毎にx座標値が昇順に並ぶようにソートする
			//ソートした辺毎に重み付けを行い辺の特定の重み毎のペアを作成する
			int weight;
			int pairStartIndex = 0;
			List<List<EdgePair>> sortedEdgePairs = new List<List<EdgePair>>();
			List<EdgePair> edgePairs;
			foreach(var list in sortedEdges.Values) {
				list.Sort((x, y) => {
					if(x.xCenter > y.xCenter) {
						return 1;
					} else if(x.xCenter < y.xCenter) {
						return -1;
					} else {
						return 0;
					}
				});

				weight = 0;
				edgePairs = new List<EdgePair>();
				for(int i = 0; i < list.Count; ++i) {
					if(list[i].atob.y > 0f) {
						//上向き
						list[i].weight = ++weight;
						if(weight == 1) {
							pairStartIndex = i;
						}
					} else {
						//下向き
						list[i].weight = --weight;
						if(weight == 0) {
							edgePairs.Add(new EdgePair(list[pairStartIndex], list[i]));
						}
					}
				}
				sortedEdgePairs.Add(edgePairs);
			}

			return sortedEdgePairs;
		}

		/// <summary>
		/// 辺のペア同士のマージを行い新しいポリゴンのリストを返す
		/// </summary>
		/// <returns>マージされたポリゴンのリスト</returns>
		/// <param name="sortedEdgePairs">ソートされた辺のペアの配列のリスト</param>
		private static List<Vector2[]> MargePairs(List<List<EdgePair>> sortedEdgePairs) {

			List<Vector2[]> polygonPoints = new List<Vector2[]>();

			while(true) {
				var points = MargePair(sortedEdgePairs);
				if(points == null) break;
				polygonPoints.Add(points);
			}

			return polygonPoints;
		}

		/// <summary>
		/// 辺のペア同士をマージを行い新しいポリゴンを返す
		/// </summary>
		/// <returns>マージされたポリゴン</returns>
		/// <param name="sortedEdgePairs">ソートされた辺のペア配列のリスト</param>
		private static Vector2[] MargePair(List<List<EdgePair>> sortedEdgePairs) {

			EdgePair pair = null;
			List<EdgePair> buildPairs = new List<EdgePair>();

			//下から順に辺のペアを積み上げていく
			for(int i = 0; i < sortedEdgePairs.Count; ++i) {
				if(pair == null) {
					var pairs = sortedEdgePairs[i];
					for(int j = 0; j < pairs.Count; ++j) {
						if(!pairs[j].isMarged) {
							pair = pairs[j];
							pair.isMarged = true;
							buildPairs.Add(pair);
							break;
						}
					}
				} else {
					var tmp = GetConnectedEdgePair(pair, sortedEdgePairs[i]);
					if(tmp != null) {
						pair = tmp;
						pair.isMarged = true;
						buildPairs.Add(pair);
					} else {
						break;
					}
				}
			}

			//積み上げたものがなければnullを返す
			if(buildPairs.Count == 0) {
				return null;
			}
			//頂点群に変換する
			List<Vector2> points = new List<Vector2>();
			int k = 0;
			while(k < buildPairs.Count) {
				points.Add(buildPairs[k].left.a);
				points.Add(buildPairs[k].left.b);
				++k;
			}
			while(k > 0) {
				--k;
				points.Add(buildPairs[k].right.a);
				points.Add(buildPairs[k].right.b);
			}
			EliminateRedundancy(ref points);

			return points.ToArray();
		}

		/// <summary>
		/// 指定したペアの上部にあるマージ済みでない隣接するペアを一つ見つける。存在しない場合はnullを返す
		/// </summary>
		/// <returns>隣接するペア</returns>
		/// <param name="pair">ペア</param>
		/// <param name="upperPairs">上部のペア配列</param>
		private static EdgePair GetConnectedEdgePair(EdgePair pair, List<EdgePair> upperPairs) {

			float upperXMin = pair.left.b.x;
			float upperXMax = pair.right.a.x;
			float lowerXMin, lowerXMax;

			for(int i = 0; i < upperPairs.Count; ++i) {
				if(upperPairs[i].isMarged) continue;
				lowerXMin = upperPairs[i].left.a.x;
				lowerXMax = upperPairs[i].right.b.x;

				if((upperXMin <= lowerXMin && lowerXMin <= upperXMax) ||
				   (upperXMin <= lowerXMax && lowerXMax <= upperXMax)) {
					return upperPairs[i];
				}
			}

			return null;
		}

		/// <summary>
		/// ２つの平面図形を構成する頂点情報を辺に変換し、
		/// それぞれの頂点のy座標及び交点で分割する
		/// </summary>
		/// <returns>分割された辺</returns>
		/// <param name="aPoints">平面図形aを構成する頂点配列</param>
		/// <param name="bPoints">平面図形bを構成する頂点配列</param>
		private static SortedList<float, List<Edge>> SliceSortedEdges(Vector2[] aPoints, Vector2[] bPoints) {

			//重複しない頂点と交点のy座標を求める
			HashSet<float> ySet = new HashSet<float>();
			Edge[] aEdges = PointsToEdges(aPoints);
			Edge[] bEdges = PointsToEdges(bPoints);
			AddYPositions(ref ySet, aPoints);
			AddYPositions(ref ySet, bPoints);
			AddIntersectionYPositions(ref ySet, aEdges, bEdges);

			//y座標は配列に直し昇順に並べる
			float[] yDivs = ySet.ToArray();
			Array.Sort(yDivs);

			//y座標値にしたがって辺を分割する
			Edge[] sliceEdges1 = SliceEdges(aEdges, yDivs);
			Edge[] sliceEdges2 = SliceEdges(bEdges, yDivs);

			//分割された辺をy座標値毎に昇順になるようにリストに格納する
			SortedList<float, List<Edge>> sortedEdges = new SortedList<float, List<Edge>>();
			AddEdges(ref sortedEdges, sliceEdges1);
			AddEdges(ref sortedEdges, sliceEdges2);

			//誤差が考えられるのでy値が近いものはまとめる
			for(int i = sortedEdges.Count - 1; i >= 1; --i) {
				var a = sortedEdges.ElementAt(i);
				var b = sortedEdges.ElementAt(i - 1);
				if(a.Key - b.Key < 0.01f) {
					//bにまとめる
					for(int j = 0; j < a.Value.Count; ++j) {
						b.Value.Add(a.Value[j]);
					}
					//aは取り除く
					sortedEdges.RemoveAt(i);
				}
			}


			return sortedEdges;
		}

		/// <summary>
		/// 座標配列から辺の配列を作成する
		/// </summary>
		/// <returns>辺の配列</returns>
		/// <param name="positions">座標配列</param>
		private static Edge[] PointsToEdges(Vector2[] positions) {
			List<Edge> e = new List<Edge>();
			int len = positions.Length;
			Vector2 p1, p2;
			for(int i = 0; i < len; ++i) {
				p1 = positions[i];
				p2 = positions[(i + 1) % len];
				//y座標の差がそこまで離れていない場合は水平
				if(Mathf.Abs(p1.y - p2.y) <= 0.001f) continue;
				e.Add(new Edge(p1, p2));
			}
			return e.ToArray();
		}

		/// <summary>
		/// 集合に重複しないようy座標値の値を追加する
		/// </summary>
		/// <param name="ySet">y座標値の集合</param>
		/// <param name="positions">座標</param>
		private static void AddYPositions(ref HashSet<float> ySet, Vector2[] positions) {
			for(int i = 0; i < positions.Length; ++i) {
				ySet.Add(positions[i].y);
			}
		}

		/// <summary>
		/// 指定した集合に二つの平面図形の交点のy座標値を追加する
		/// </summary>
		/// <param name="ySet">座標値を追加する集合</param>
		/// <param name="edge1">辺配列1</param>
		/// <param name="edge2">辺配列2</param>
		private static void AddIntersectionYPositions(ref HashSet<float> ySet, Edge[] edge1, Edge[] edge2) {
			int len = edge2.Length;
			Vector2 intPos;
			for(int i = 0; i < edge1.Length; ++i) {
				for(int j = 0; j < len; ++j) {
					if(Intersection(edge1[i], edge2[j], out intPos)) {
						ySet.Add(intPos.y);
					}
				}
			}
		}

		/// <summary>
		/// 指定した辺配列を指定した分割値で分割する
		/// </summary>
		/// <returns>分割された辺</returns>
		/// <param name="edges">分割する辺配列</param>
		/// <param name="yDivs">辺を分割する分割値。昇順にそーとされていること</param>
		private static Edge[] SliceEdges(Edge[] edges, float[] yDivs) {
			List<Edge> divEdges = new List<Edge>();
			for(int i = 0; i < edges.Length; ++i) {
				divEdges.AddRange(SliceEdge(edges[i], yDivs));
			}
			return divEdges.ToArray();
		}

		/// <summary>
		/// 指定した辺を指定した分割値で分割する
		/// </summary>
		/// <returns>分割された辺</returns>
		/// <param name="edge">分割される辺</param>
		/// <param name="yDivs">辺を分割する分割値。昇順にソートされていること</param>
		private static Edge[] SliceEdge(Edge edge, float[] yDivs) {

			//辺の最小値,最大値
			float yMin = edge.yMin, yMax = edge.yMax;

			//分割値の辺の最小値-最大値内のそれぞれのインデックス
			int yMinIndex = -1, yMaxIndex = -1;
			int i = 0, j = 0;

			for(; i < yDivs.Length; ++i) {
				if(yMin < yDivs[i] && yDivs[i] < yMax) {
					yMinIndex = i;
					yMaxIndex = i;
					break;
				}
			}
			for(; i < yDivs.Length; ++i) {
				if(yMax <= yDivs[i]) {
					yMaxIndex = i - 1;
					break;
				}
			}

			//分割できない場合はそのまま返す
			if(yMinIndex == -1) return new Edge[] { edge };

			int divs = yMaxIndex - yMinIndex + 2;
			Edge[] divEdges = new Edge[divs];
			float y1, y2;

			//辺の上向き、下向きを考慮
			if(edge.atob.y > 0f) {
				//上向き
				y2 = yDivs[yMinIndex];
				divEdges[0] = new Edge(edge.a, new Vector2(edge.X(y2), y2));

				for(i = yMinIndex, j = 1; i < yMaxIndex; ++i, ++j) {
					y1 = yDivs[i];
					y2 = yDivs[i + 1];
					divEdges[j] = new Edge(new Vector2(edge.X(y1), y1), new Vector2(edge.X(y2), y2));
				}

				y1 = yDivs[yMaxIndex];
				divEdges[divs - 1] = new Edge(new Vector2(edge.X(y1), y1), edge.b);
			} else {
				//下向き
				y2 = yDivs[yMaxIndex];
				divEdges[0] = new Edge(edge.a, new Vector2(edge.X(y2), y2));

				for(i = yMaxIndex, j = 1; i >= yMinIndex; --i, ++j) {
					y1 = yDivs[i];
					y2 = yDivs[i - 1];
					divEdges[j] = new Edge(new Vector2(edge.X(y1), y1), new Vector2(edge.X(y2), y2));
				}

				y1 = yDivs[yMinIndex];
				divEdges[divs - 1] = new Edge(new Vector2(edge.X(y1), y1), edge.b);
			}
			return divEdges;
		}

		/// <summary>
		/// 指定したEdgesBandの集合に辺を追加していく
		/// </summary>
		/// <param name="sortedEdges">EdgesBandの集合</param>
		/// <param name="edges">追加する辺配列</param>
		private static void AddEdges(ref SortedList<float, List<Edge>> sortedEdges, Edge[] edges) {
			float center;
			for(int i = 0; i < edges.Length; ++i) {
				center = edges[i].yCenter;
				if(!sortedEdges.ContainsKey(center)) {
					sortedEdges.Add(center, new List<Edge>());
				}
				sortedEdges[center].Add(edges[i]);
			}
		}

		#endregion
	}
}