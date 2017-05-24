using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UniFigLib;

namespace Common.PlaneTerrain {

	/// <summary>
	/// 地形生成あたりのテストクラス
	/// </summary>
	public class PolygonTest1 : MonoBehaviour {

		public TerrainPolygon terrain;
		public Camera cam;

		private Vector2[] notCirclePoints;
		private Vector2[] orCirclePoints;

		private void Start() {
			notCirclePoints = PlaneTerrainUtil.MakeCirclePoints(0.5f, 12, Matrix4x4.identity, false);
			orCirclePoints = PlaneTerrainUtil.MakeCirclePoints(0.5f, 12, Matrix4x4.identity, true);
			Reset();
		}

		private void Update() {
			if(Input.GetMouseButtonDown(0)) {
				var ray = cam.ScreenPointToRay(Input.mousePosition);
				var hit = Physics2D.Raycast(ray.origin, ray.direction, 10f);
				var mPos = ray.origin;
				mPos.z = 0f;
				if(hit.collider) {
					hit.collider.GetComponent<TerrainPolygon>().
					   NOT(notCirclePoints, Matrix4x4.TRS(mPos, Quaternion.identity, Vector3.one));
				}
			} else if(Input.GetMouseButtonDown(1)) {
				var ray = cam.ScreenPointToRay(Input.mousePosition);
				var hit = Physics2D.Raycast(ray.origin, ray.direction, 10f);
				var mPos = ray.origin;
				mPos.z = 0f;
				if(hit.collider) {
					hit.collider.GetComponent<TerrainPolygon>().
					   OR(orCirclePoints, Matrix4x4.TRS(mPos, Quaternion.identity, Vector3.one));
				}
			}
			if(Input.GetKeyDown(KeyCode.Space)) {
				Reset();
			}
		}

		public void Reset() {
			foreach(Transform child in transform) {
				Destroy(child.gameObject);
			}
			var terrainObj = Instantiate(terrain.gameObject);
			terrainObj.transform.SetParent(transform);
		}
	}
}