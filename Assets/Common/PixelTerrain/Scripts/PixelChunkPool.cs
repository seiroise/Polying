using UnityEngine;
using System.Collections.Generic;

namespace Common.PixelTerrain {

	/// <summary>
	/// PixelChunkのプール
	/// </summary>
	public class PixelChunkPool : MonoBehaviour {

		[SerializeField]
		private PixelChunk _pixelChunkPrefab;
		[SerializeField, Range(4, 512)]
		private int _initPoolNum = 16;

		private List<PixelChunk> _pool;
		private int _nextIndex;

		private void Awake() {
			InitPool();
		}

		/// <summary>
		/// プールの初期化
		/// </summary>
		private void InitPool() {
			_pool = new List<PixelChunk>();
			for(int i = 0; i < _initPoolNum; ++i) {
				var pixelChunk = Instantiate<PixelChunk>(_pixelChunkPrefab);
				pixelChunk.transform.SetParent(transform);
				pixelChunk.gameObject.SetActive(false);
				_pool.Add(pixelChunk);
			}
			_nextIndex = 0;
		}

		/// <summary>
		/// 空いているPixelChunkを一つ返す。空きがないならnullを返す
		/// </summary>
		/// <returns>空いているPixelChunk</returns>
		public PixelChunk Get() {
			int size = _pool.Count;
			PixelChunk chunk;
			for(int i = 0; i < size; ++i) {
				chunk = _pool[_nextIndex];
				_nextIndex = (_nextIndex + 1) % size;
				if(!chunk.gameObject.activeInHierarchy) {
					return chunk;
				}
			}
			return null;
		}
	}
}