using UnityEngine;
using System.Collections;
using UniFigLib;
using EditorUtil;
using System;
using System.Collections.Generic;

namespace Common.PlaneTerrain {

	/// <summary>
	/// 地形を構成するポリゴン。基本的に受け
	/// </summary>
	[RequireComponent(typeof(PolygonCollider2D))]
	[ExecuteInEditMode]
	public class TerrainPolygon : MonoBehaviour {

		[Header("Terrain Parameter")]
		[SerializeField, Button("Apply Points", "ApplyPoints")]
		private int _btn1;
		[SerializeField]
		private bool _awakeWithApply = false;   //awake時にAppluPoints()を呼び出すか

		[Header("Draw Parameter")]
		[SerializeField]
		private Material _material;
		[SerializeField]
		private Color _color;

		private Vector2[] _points;      //座標配列
		private Figure _figure;         //図形情報
		private Mesh _mesh;             //図形情報から作成したMesh

		private PolygonCollider2D _collider;

		public bool awakeWithApply {
			get {
				return _awakeWithApply;
			}
			set {
				_awakeWithApply = value;
			}
		}
		public Material material {
			get {
				return _material;
			}
			set {
				_material = value;
			}
		}
		public Color color {
			get {
				return _color;
			}
			set {
				_color = value;
			}
		}

		private void Start() {
			if(_awakeWithApply) {
				ApplyPoints();
			}
		}

		private void Update() {
			DrawMesh();
		}

		/// <summary>
		/// メッシュの描画
		/// </summary>
		private void DrawMesh() {
			if(_mesh) {
				Graphics.DrawMesh(_mesh, transform.localToWorldMatrix, _material, 0);
			}
		}

		/// <summary>
		/// 座標の更新手続き
		/// </summary>
		private void ApplyPoints() {
			if(!_collider) {
				_collider = GetComponent<PolygonCollider2D>();
			}
			_points = _collider.points;

			//回転方向を確認
			if(!PlaneTerrainUtil.CheckCW(_points)) {
				Array.Reverse(_points);
				_collider.points = _points;
			}
			_figure = Figure.FromPositions(_points, _color);
			_mesh = _figure.ToMesh();
		}

		/// <summary>
		/// 座標配列の設定
		/// </summary>
		/// <param name="points">設定する座標配列</param>
		public void SetPoints(Vector2[] points) {
			if(!_collider) {
				_collider = GetComponent<PolygonCollider2D>();
			}
			_collider.points = points;
			_points = points;
			_figure = Figure.FromPositions(points, _color);
			_mesh = _figure.ToMesh();
		}

		/// <summary>
		/// 論理和
		/// </summary>
		/// <returns>論理和の結果生成された新しい地形ポリゴン</returns>
		/// <param name="points">座標配列</param>
		/// <param name="transMat">姿勢行列</param>
		public List<TerrainPolygon> OR(Vector2[] points, Matrix4x4 transMat) {
			//入力の座標配列が反時計回り確認
			if(!PlaneTerrainUtil.CheckCW(points)) {
				Array.Reverse(points);
			}
			//姿勢行列の反映
			Matrix4x4 mat = transform.localToWorldMatrix;
			for(int i = 0; i < _points.Length; ++i) {
				_points[i] = mat.MultiplyPoint3x4(_points[i]);
			}
			Vector2[] transPoints = new Vector2[points.Length];
			for(int i = 0; i < points.Length; ++i) {
				transPoints[i] = transMat.MultiplyPoint3x4(points[i]);
			}
			//論理和
			List<Vector2[]> newPoints = PlaneTerrainUtil.OR(_points, transPoints);
			List<TerrainPolygon> newTerrains = new List<TerrainPolygon>();
			for(int i = 0; i < newPoints.Count; ++i) {
				if(newPoints[i].Length <= 0) continue;
				var newTerrain = Instantiate<TerrainPolygon>(this);
				newTerrain.transform.SetParent(transform.parent);
				newTerrain._awakeWithApply = false;
				newTerrain.SetPoints(newPoints[i]);
				newTerrains.Add(newTerrain);
			}
			Destroy(gameObject);
			return newTerrains;
		}

		/// <summary>
		/// 論理否定
		/// </summary>
		/// <returns>論理否定の結果生成された新しい地形ポリゴン</returns>
		/// <param name="points">座標配列</param>
		/// <param name="transMat">姿勢行列</param>
		public List<TerrainPolygon> NOT(Vector2[] points, Matrix4x4 transMat) {
			//入力の座標配列が時計回り確認
			if(PlaneTerrainUtil.CheckCW(points)) {
				Array.Reverse(points);
			}
			//姿勢行列の反映
			Matrix4x4 mat = transform.localToWorldMatrix;
			for(int i = 0; i < _points.Length; ++i) {
				_points[i] = mat.MultiplyPoint3x4(_points[i]);
			}
			Vector2[] transPoints = new Vector2[points.Length];
			for(int i = 0; i < points.Length; ++i) {
				transPoints[i] = transMat.MultiplyPoint3x4(points[i]);
			}
			//論理否定
			List<Vector2[]> newPoints = PlaneTerrainUtil.Not(_points, transPoints);
			List<TerrainPolygon> newTerrains = new List<TerrainPolygon>();
			for(int i = 0; i < newPoints.Count; ++i) {
				if(newPoints[i].Length <= 0) continue;
				var newTerrain = Instantiate<TerrainPolygon>(this);
				newTerrain.transform.SetParent(transform.parent);
				newTerrain._awakeWithApply = false;
				newTerrain.SetPoints(newPoints[i]);
				newTerrains.Add(newTerrain);
			}
			Destroy(gameObject);
			return newTerrains;
		}
	}
}