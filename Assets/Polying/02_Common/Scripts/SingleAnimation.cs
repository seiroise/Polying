using UnityEngine;
using UnityEngine.Events;
using System;

namespace Polying.Common {

	/// <summary>
	/// 単品のアニメーション
	/// </summary>
	[RequireComponent(typeof(Animation))]
	public class SingleAnimation : MonoBehaviour {

		[SerializeField]
		private bool _startWithPlay;
		[SerializeField]
		private bool _stopWithDestroy;

		private Animation _animation;
		private bool _played;
		private UnityEvent _onStopped;

		public UnityEvent onStopped {
			get {
				return _onStopped;
			}
		}

		private void Awake() {
			_animation = GetComponent<Animation>();
			_onStopped = new UnityEvent();
		}

		private void Start() {
			if(_startWithPlay) {
				Play();
			}
		}

		private void Update() {
			if(_played) {
				if(!_animation.isPlaying) {
					_played = false;
					_onStopped.Invoke();
					if(_stopWithDestroy) {
						Destroy(gameObject);
					}
				}
			}
		}

		/// <summary>
		/// アニメーションの再生
		/// </summary>
		public void Play() {
			_animation.Play();
			_played = true;
		}

		/// <summary>
		/// アニメーションの停止
		/// </summary>
		public void Stop() {
			if(_animation.isPlaying) {
				_animation.Stop();
				_played = false;
			}
		}
	}
}