using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UniFigLib;

namespace Common.PlaneTerrain {

	/// <summary>
	/// 地形生成あたりのテストクラス
	/// </summary>
	public class PolygonTest2 : MonoBehaviour {

		public TerrainPolygon terrain;
		public Camera cam;

		public Vector2[] testPoints = {Vector2.zero, Vector2.one};

		private Vector2[] circlePoints;

		private void Start() {
			circlePoints = PlaneTerrainUtil.MakeCirclePoints(0.5f, 4, Matrix4x4.identity, false);
			Reset();
		}

		private void Update() {
			if(Input.GetMouseButtonDown(0)) {
				for(int i = 0; i < testPoints.Length; ++i) {
					var hit = Physics2D.Raycast(testPoints[i], Vector2.zero, 10f);
					Vector3 mPos = testPoints[i];
					mPos.z = 0f;
					if(hit.collider) {
						hit.collider.GetComponent<TerrainPolygon>().
						NOT(circlePoints, Matrix4x4.TRS(mPos, Quaternion.identity, Vector3.one));
					}
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