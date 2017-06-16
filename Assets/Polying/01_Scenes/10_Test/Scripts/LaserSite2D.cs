using UnityEngine;
using System.Collections;

namespace Polying.Test {

	/// <summary>
	/// レーザーサイト
	/// </summary>
	public class LaserSite2D : MonoBehaviour {

		[SerializeField, Range(10f, 1000f)]
		private float _distance = 100f;

		[Header("Laser Effect")]
		[SerializeField]
		private SpriteRenderer _laserSprite;
		[SerializeField, Range(0f, 10f)]
		private float width = 1f;

		[Header("Laser Hit Effect")]
		[SerializeField]
		private Transform _laserHitSprite;

		private Transform _trans;
		private Transform _laserTrans;

		private float _laserScale;

		private void Awake() {
			_trans = transform;
			_laserTrans = _laserSprite.transform;
			_laserScale = _laserSprite.sprite.pixelsPerUnit / _laserSprite.sprite.rect.width;
		}

		private void Update() {
			UpdateRaser();
		}

		private void UpdateRaser() {
			RaycastHit2D hit;
			hit = Physics2D.Raycast(_trans.position, _trans.right, _distance);
			if(_laserSprite) {
				_laserTrans.localPosition = new Vector3(hit.distance * 0.5f, 0f, 0f);
				_laserTrans.localScale = new Vector3(hit.distance * _laserScale, width, 1f);
			}
			if(_laserHitSprite) {
				_laserHitSprite.position = hit.point;
			}
		}
	}
}