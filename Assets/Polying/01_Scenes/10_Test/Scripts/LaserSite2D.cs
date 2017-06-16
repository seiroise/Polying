using UnityEngine;
using System.Collections;

namespace Polying.Test {

	/// <summary>
	/// レーザーサイト
	/// </summary>
	[RequireComponent(typeof(LineRenderer))]
	public class LaserSite2D : MonoBehaviour {

		[SerializeField, Range(10f, 1000f)]
		private float _distance = 100f;
		[SerializeField]
		private SpriteRenderer _laserSprite;
		[SerializeField]
		private Transform _laserHitSprite;

		private LineRenderer _line;
		private Transform _trans;
		private Transform _laserTrans;

		private float _laserScale;

		private void Awake() {
			_line = GetComponent<LineRenderer>();
			_line.SetVertexCount(2);
			_line.SetPosition(0, new Vector3(0f, 0f, 0f));
			_line.SetPosition(1, new Vector3(0f, 0f, 0f));
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
			_line.SetPosition(0, _trans.position);
			_line.SetPosition(1, hit.point);
			Debug.Log(hit.distance);
			if(_laserSprite) {
				_laserTrans.localPosition = new Vector3(hit.distance, 0f, 0f);
				_laserTrans.localScale = new Vector3(hit.distance * _laserScale * 2f, 1f, 1f);
			}
			if(_laserHitSprite) {
				_laserHitSprite.position = hit.point;
			}
		}
	}
}