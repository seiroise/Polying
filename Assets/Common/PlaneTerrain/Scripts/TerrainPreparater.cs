using UnityEngine;
using EditorUtil;
using UniFigLib;
using System;
using System.Collections.Generic;

namespace Common.PlaneTerrain {

	/// <summary>
	/// 地形の整地(掘ったり、盛ったり)をおこなう。基本的に攻め
	/// </summary>
	[RequireComponent(typeof(Rigidbody2D))]
	[ExecuteInEditMode]
	public class TerrainPreparater : MonoBehaviour {

		/// <summary>
		/// 処理タイプ
		/// </summary>
		public enum BooleanType {
			OR,
			NOT
		}

		[Header("Terrain Parameter")]
		[SerializeField, Range(0.01f, 10f)]
		private float _radius = 1f;

		[Header("Boolean Parameter")]
		[SerializeField]
		private BooleanType _booleanType = BooleanType.OR;
		[Header("Boolean OR Parameter")]
		[SerializeField]
		private Material _orObjMaterial;
		[SerializeField]
		private Color _orObjeColor;

		private HashSet<TerrainPolygon> detectedTerrainPolygons;

		private void Awake() {
			detectedTerrainPolygons = new HashSet<TerrainPolygon>();
		}

		private void OnTriggerEnter2D(Collider2D co) {
			var terrain = co.GetComponent<TerrainPolygon>();
			if(terrain) {
				detectedTerrainPolygons.Add(terrain);
			}
		}

		private void OnTriggerExit2D(Collider2D co) {
			var terrain = co.GetComponent<TerrainPolygon>();
			if(terrain) {
				detectedTerrainPolygons.Remove(terrain);
			}
		}

		private void OnCollisionEnter2D(Collision2D co) {
			switch(_booleanType) {
			case BooleanType.OR:
				OR();
				break;
			case BooleanType.NOT:
				NOT();
				break;
			}
		}

		/// <summary>
		/// 論理和
		/// </summary>
		private void OR() {
			//接触している可能性のある全ての地形ポリゴンと論理否定を行い
			//最終的にTerrainPolygonを作成する
			NOT();
			var gObj = new GameObject();
			var terrain = gObj.AddComponent<TerrainPolygon>();
			terrain.material = _orObjMaterial;
			terrain.color = _orObjeColor;
			terrain.SetPoints(PlaneTerrainUtil.MakeCirclePoints(_radius, 12, transform.localToWorldMatrix, true));
			terrain.awakeWithApply = false;
		}

		/// <summary>
		/// 論理否定
		/// </summary>
		private void NOT() {
			//接触している可能性のある全ての地形ポリゴンと論理否定をおこなう
			Vector2[] points = PlaneTerrainUtil.MakeCirclePoints(_radius, 12, Matrix4x4.identity, false);
			foreach(var t in detectedTerrainPolygons) {
				t.NOT(points, transform.localToWorldMatrix);
			}
			detectedTerrainPolygons.Clear();
			Destroy(gameObject);
		}
	}
}
