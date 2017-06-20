using UnityEngine;
using System.Collections;

namespace Common.PixelTerrain {

	/// <summary>
	/// アニメーションなエフェクト
	/// </summary>
	[RequireComponent(typeof(Animation))]
	public class AnimationEffect : MonoBehaviour {

		[SerializeField]
		private bool _startWithPlay;

		private Animation _animation;
		private bool _played;

		private void Awake() {
			_animation = GetComponent<Animation>();
		}

		private void Start() {
			if(_startWithPlay) {
				Play();
			}
		}

		public void Play() {
			_animation.Play();
			Destroy(gameObject, _animation.clip.length);
		}
	}
}