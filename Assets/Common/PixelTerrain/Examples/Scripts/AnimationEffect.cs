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

		private void Update() {
			if(_played) {
				if(!_animation.isPlaying) {
					Destroy(gameObject);
				}
			}
		}

		public void Play() {
			_animation.Play();
			_played = true;
		}
	}
}