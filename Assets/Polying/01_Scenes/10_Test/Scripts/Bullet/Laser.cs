using UnityEngine;
using System.Collections;
using Polying.Test;

namespace Polying.Test {

	/// <summary>
	/// レーザー
	/// </summary>
	[RequireComponent(typeof(SpriteRenderer))]
	public class Laser : TriggerAttacker {

		[SerializeField, Range(0f, 10f)]
		private float _width = 1f;

		[Header("Trail Effect Parameter")]
		[SerializeField, Range(0.01f, 2f)]
		private float _trailEffectDensity = 0.5f;
		[SerializeField]
		private EffectParams _trailEffectParams;

		private SpriteRenderer _laserSprite;
		private ParticleSystem _emitter;
		private float _laserScale;

		private void Awake() {
			_laserSprite = GetComponent<SpriteRenderer>();
			_emitter = FindObjectOfType<ParticleSystem>();
			_laserScale = _laserSprite.sprite.pixelsPerUnit / _laserSprite.sprite.rect.width;
		}

		private void OnDestroy() {
			if(_emitter) {
				float distance = transform.localScale.x * 0.25f;
				Vector3 from = transform.position + transform.right * distance;
				Vector3 to = transform.position + transform.right * -distance;
				EffectFunctions.EmitParticle(_emitter, _trailEffectParams, from, to, _trailEffectDensity);
			}
		}

		/// <summary>
		/// 距離を設定
		/// </summary>
		/// <param name="distance">Distance.</param>
		public void SetDistance(float distance) {
			transform.localScale = new Vector3(distance * _laserScale, _width, 1f);
		}
	}
}