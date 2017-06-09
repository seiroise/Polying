using UnityEngine;
using System.Collections.Generic;

namespace Common.PixelTerrain {

	/// <summary>
	/// ピクセル地形のローダー
	/// </summary>
	[RequireComponent(typeof(PixelTerrain))]
	public class PixelTerrainLoader : MonoBehaviour {

		[Header("Target Parameter")]
		[SerializeField]
		private Transform _targetObj;
		[SerializeField]
		private Vector2 _activeArea;

		private PixelTerrain _terrain;

		private Vector2 _targetPrevPoint;
		private Vector2 _targetMovement;

		private int _chunkx, _chunky;	//現在のチャンy
		private int _tempx, _tempy;	//一時チャンク座標

		private void Awake() {
			_terrain = GetComponent<PixelTerrain>();
		}

		private void Start() {
			if(_targetObj) {
				SetTargetObj(_targetObj);
			}
		}

		private void Update() {
			UpdateActiveChunk();
		}

		/// <summary>
		/// アクティブなチャンクの更新
		/// </summary>
		private void UpdateActiveChunk() {
			if(!_targetObj) return;
			Vector3 pos = _targetObj.position; 
			_terrain.PointToIndex(pos.x, pos.y, out _tempx, out _tempy);
			if(_chunkx != _tempx || _chunky != _tempy) {
				_chunkx = _tempx;
				_chunky = _tempy;
				ShowAreaChunk(pos);
			}
		}

		/// <summary>
		/// 目標オブジェクトからアクティブな範囲のチャンクを表示する
		/// </summary>
		private void ShowAreaChunk(Vector3 pos) {
			//表示チャンクの更新
			int minx, miny, maxx, maxy;
			_terrain.PointToIndex(pos.x - _activeArea.x, pos.y - _activeArea.y, out minx, out miny);
			_terrain.PointToIndex(pos.x + _activeArea.x, pos.y + _activeArea.y, out maxx, out maxy);

			ShowAreaChunk(minx, miny, maxx, maxy);
		}

		/// <summary>
		/// 指定した範囲のチャンクを表示する
		/// </summary>
		/// <param name="minx">始点x</param>
		/// <param name="miny">始点y</param>
		/// <param name="maxx">終点x</param>
		/// <param name="maxy">終点y</param>
		private void ShowAreaChunk(int minx, int miny, int maxx, int maxy) {
			//表示する添字の集合を作成
			var showIndices = new HashSet<XYIndex>();
			for(int x = minx; x < maxx; ++x) {
				for(int y = miny; y < maxy; ++y) {
					showIndices.Add(new XYIndex(x, y));
				}
			}

			//現在表示中のチャンクから被らない添字を非表示に
			var indices = _terrain.GetShowChunkIndices();
			for(int i = 0; i < indices.Length; ++i) {
				if(showIndices.Contains(indices[i])) {
					//すでに表示している添字は取り除く
					showIndices.Remove(indices[i]);
				} else {
					//表示しない添字は非表示にする
					_terrain.HideChunk(indices[i]);
				}
			}

			//新しく表示する添字を表示
			foreach(var index in showIndices) {
				_terrain.ShowChunk(index);
			}
		}

		/// <summary>
		/// 地形を更新する中心となるオブジェクトの設定
		/// </summary>
		/// <param name="targetObj">ターゲットオブジェクト</param>
		public void SetTargetObj(Transform targetObj) {
			_targetObj = targetObj;
			_terrain.PointToIndex(_targetObj.position.x, _targetObj.position.y, out _chunkx, out _chunky);
			ShowAreaChunk(_targetObj.position);
		}
	}
}