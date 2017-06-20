using UnityEngine;
using UnityEngine.Events;
using System;

namespace Polying.Test {

	/// <summary>
	/// エンティティの発見器
	/// </summary>
	[RequireComponent(typeof(Collider2D))]
	public class EntityDetector : MonoBehaviour {

		public class OnDetected : UnityEvent<Entity> { }

		[SerializeField]
		private string _filterTag;

		private OnDetected _onEntered;
		private OnDetected _onExited;

		private Entity _detected;
		private Collider2D _detectedCollider;

		public OnDetected onEntered {
			get {
				return _onEntered;
			}
		}
		public OnDetected onExited {
			get {
				return _onExited;
			}
		}

		private void Awake() {
			_onEntered = new OnDetected();
			_onExited = new OnDetected();
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