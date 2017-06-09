using UnityEngine;
using System;
using System.Collections;

namespace Common.PixelTerrain {

	/// <summary>
	/// ピクセルの集まり
	/// </summary>
	[RequireComponent(typeof(MeshCollider))]
	public class PixelChunk : MonoBehaviour {

		[Header("Rendering Parameter")]
		[SerializeField, Range(0.1f, 1f)]
		private float _pixelSize = 0.25f;			//描画時のピクセルの大きさ

		private Material _material;					//描画マテリアル
		private MeshCollider _meshCollider;			//当たり判定
		private PixelChunkData _chunkData;
		private Mesh _mesh;

		private bool _needUpdate;					//更新が必要か

		public float pixelSize {
			get {
				return _pixelSize;
			}
			set {
				_pixelSize = value;
			}
		}
		public bool needUpdate {
			get {
				return _needUpdate;
			}
		}

		private void Awake() {
			_meshCollider = GetComponent<MeshCollider>();
		}

		private void Update() {
			if(_chunkData.isUpdated) {
				UpdateMesh();
			}
			if(_mesh) {
				Graphics.DrawMesh(_mesh, transform.localToWorldMatrix, _material, 0);
			}
		}

		/// <summary>
		/// メッシュの更新を行う
		/// </summary>
		public void UpdateMesh() {
			if(_chunkData != null) {
				_mesh = _chunkData.ToMesh(_pixelSize);
				if(!_meshCollider) {
					_meshCollider = GetComponent<MeshCollider>();
				}
				_meshCollider.sharedMesh = _mesh;
			}
		}

		/// <summary>
		/// PixelChunkDataを設定する
		/// </summary>
		/// <param name="data">データ</param>
		public void SetPixelChunkData(PixelChunkData data, Material material) {
			this._chunkData = data;
			this._material = material;
			UpdateMesh();
		}
	}
}