using UnityEngine;
using System.Collections;

namespace Common.PixelTerrain {

	/// <summary>
	/// アイテムの収集
	/// </summary>
	[RequireComponent(typeof(Animation))]
	public class ItemCollector : MonoBehaviour {

		[SerializeField]
		private ItemBar _itemBar;

		private Animation _animation;	//収集アニメーション
		private bool _isWorking;		//動作中

		private void Awake() {
			_animation = GetComponent<Animation>();
		}

		private void Update() {
			if(_isWorking) {
				if(!_animation.isPlaying) {
					_isWorking = false;
				}
			}
		}

		/// <summary>
		/// 収集
		/// </summary>
		public void Collect() {
			if(_isWorking) return;
			_isWorking = true;
			_animation.Play();
		}

		public void AddItem(string name, int num) {
			if(!_itemBar) return;
			_itemBar.AddItem(name, num);
		}
	}
}