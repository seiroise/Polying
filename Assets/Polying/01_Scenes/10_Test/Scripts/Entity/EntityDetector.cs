using UnityEngine;
using UnityEngine.Events;
using System;

namespace Polying.Test {

	/// <summary>
	/// エンティティの発見器
	/// </summary>
	[RequireComponent(typeof(Collider2D))]
	public class EntityDetector : MonoBehaviour {

		[SerializeField]
		private string _filterTag;

		private EntityEvent _onEntered;
		private EntityEvent _onExited;

		private Entity _detected;
		private Collider2D _detectedCollider;

		public EntityEvent onEntered {
			get {
				return _onEntered;
			}
		}
		public EntityEvent onExited {
			get {
				return _onExited;
			}
		}

		private void Awake() {
			_onEntered = new EntityEvent();
			_onExited = new EntityEvent();
		}

		private void OnTriggerEnter2D(Collider2D co) {
			if(string.IsNullOrEmpty(_filterTag) || co.tag.Equals(_filterTag)) {
				_detected = co.GetComponent<Entity>();
				if(_detected) {
					_detectedCollider = co;
					_onEntered.Invoke(_detected);
				}
			}
		}

		private void OnTriggerExit2D(Collider2D co) {
			if(_detectedCollider == co) {
				_onExited.Invoke(_detected);
				_detected = null;
			}
		}
	}
}