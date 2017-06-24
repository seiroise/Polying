using UnityEngine;
using System.Collections;

namespace Polying.Test {

	/// <summary>
	/// Transformを揺らす
	/// </summary>
	public class ShakeTransform : MonoBehaviour {

		[SerializeField]
		private AnimationCurve _shakeForceCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

		private float _force = 0f;
		private float _time = 0f;
		private float _cycle;

		private float _timer = 0f;
		private Vector3 _shakeDir;

		private void Update() {
			if(_timer < _time) {
				float eval = _shakeForceCurve.Evaluate(_timer / _time);
				transform.localPosition = _shakeDir * Mathf.Sin(_timer * _cycle) * _force * eval;
				_timer += Time.deltaTime;
			}
		}

		/// <summary>
		/// 揺らす
		/// </summary>
		/// <returns>The shake.</returns>
		/// <param name="force">Force.</param>
		public void Shake(float force, float cycle, float time) {
			_shakeDir.x = Random.Range(-1f, 1f);
			_shakeDir.y = Random.Range(-1f, 1f);
			_shakeDir.Normalize();
			_force = force;
			_time = time;
			_cycle = cycle;
			_timer = 0f;
		}
	}
}