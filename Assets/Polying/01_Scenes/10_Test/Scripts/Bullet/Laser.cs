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

		private SpriteRenderer _laserSprite;
		private float _laserScale;

		private void Awake() {
			_laserSprite = GetComponent<SpriteRenderer>();
			_laserScale = _laserSprite.sprite.pixelsPerUnit / _laserSprite.sprite.rect.width;
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