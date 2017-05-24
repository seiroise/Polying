using UnityEngine;
using System.Collections.Generic;

namespace Common.PlaneTerrain {

	/// <summary>
	/// 地形生成のテストクラス
	/// </summary>
	public class PolygonTest3 : MonoBehaviour {

		public TerrainPolygon baseTerrain;	//基本地形

		public Transform shotPoint;			//打ち出し場所
		public float shotForce;				//打ち出し力
		public TerrainPreparater orBullet;	//or弾
		public TerrainPreparater notBullet;	//not弾

		private List<GameObject> shotedBullets;	//うちずみ

		private void Start() {
			shotedBullets = new List<GameObject>();
			Reset();
		}

		private void Update() {
			if(Input.GetKeyDown(KeyCode.Z)) {
				ShotORBullet();
			} else if(Input.GetKeyDown(KeyCode.X)) {
				ShotNOTBullet();
			} else if(Input.GetKeyDown(KeyCode.Space)) {
				Reset();
			}
		}

		private void Reset() {
			foreach(Transform child in transform) {
				Destroy(child.gameObject);
			}
			var terrainObj = Instantiate(baseTerrain.gameObject);
			terrainObj.transform.SetParent(transform);

			for(int i = 0; i < shotedBullets.Count; ++i) {
				if(shotedBullets[i]) {
					Destroy(shotedBullets[i].gameObject);
				}
			}
			shotedBullets.Clear();
		}

		private void ShotORBullet() {
			var rBody2D = Instantiate<TerrainPreparater>(orBullet).GetComponent<Rigidbody2D>();
			rBody2D.transform.position = shotPoint.position;
			rBody2D.AddForce(new Vector2(0.6f, 0.45f) * shotForce);
			shotedBullets.Add(rBody2D.gameObject);
		}

		private void ShotNOTBullet() {
			var rBody2D = Instantiate<TerrainPreparater>(notBullet).GetComponent<Rigidbody2D>();
			rBody2D.transform.position = shotPoint.position;
			rBody2D.AddForce(new Vector2(0.6f, 0.45f) * shotForce);
			shotedBullets.Add(rBody2D.gameObject);
		}
	}
}