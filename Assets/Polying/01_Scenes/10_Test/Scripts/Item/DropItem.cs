using UnityEngine;
using System;

namespace Polying.Test {

	/// <summary>
	/// そこらへんに落ちてるアイテム
	/// </summary>
	public abstract class DropItem : MonoBehaviour {

		[Header("Drop Item")]
		[SerializeField]
		private bool _autoPickUp = true;
		[SerializeField]
		private KeyCode _pickUpKey = KeyCode.E;

		private Entity _overedEntity;
		private PickUpItemEvent _onPickUp;

		public PickUpItemEvent onPickUp {
			get {
				return _onPickUp;
			}
		}

		private void Awake() {
			_onPickUp = new PickUpItemEvent();
		}

		private void Update() {
			if(_overedEntity) {
				if(Input.GetKeyDown(_pickUpKey)) {
					_onPickUp.Invoke(transform.position, this);
					PickUp(_overedEntity);
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D co) {
			var entity = co.GetComponent<Entity>();
			if(entity) {
				if(_autoPickUp) {
					_onPickUp.Invoke(transform.position, this);
					PickUp(entity);
				} else {
					_overedEntity = entity;
				}
			}
		}

		private void OnTriggerExit2D(Collider2D co) {
			_overedEntity = null;
		}

		/// <summary>
		/// 拾う
		/// </summary>
		/// <param name="entity">Entity.</param>
		protected abstract void PickUp(Entity entity);

		/// <summary>
		/// 拾った時のメッセージ
		/// </summary>
		/// <returns>The pick up message.</returns>
		public abstract string GetPickUpMsg();
	}
}