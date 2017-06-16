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

		private LineRenderer _line;
		private Transform _trans;

		private void Awake() {
			_line = GetComponent<LineRenderer>();
			_line.SetVertexCount(2);
			_line.SetPosition(0, new Vector3(0f, 0f, 0f));
			_line.SetPosition(1, new Vector3(0f, 0f, 0f));
			_trans = transform;
		}

		private void Update() {
			DrawSite();
		}

		private void DrawSite() {
			RaycastHit2D hit;
			hit = Physics2D.Raycast(_trans.position + _trans.right, _trans.right, _distance);
			_line.SetPosition(0, _trans.position);
			_line.SetPosition(1, hit.point);
		}
	}
}