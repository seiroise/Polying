using UnityEngine;
using System;

namespace Common.PixelTerrain {

	/// <summary>
	/// ドロップアイテム(アイテムのドロップ版)
	/// </summary>
	[RequireComponent(typeof(SpriteRenderer))]
	public class DropItem : MonoBehaviour {

		[SerializeField, Range(1f, 20f)]
		private float _speed = 10f;

		private int _id;
		private string _itemName;
		private SpriteRenderer _sprite;
		private ItemCollector _targetCollector;

		public int id {
			get {
				return _id;
			}
			set {
				_id = value;
			}
		}
		public string itemName {
			get {
				return _itemName;
			}
			set {
				_itemName = value;
			}
		}
		public SpriteRenderer sprite {
			get {
				return _sprite;
			}
		}

		private void Awake() {
			_sprite = GetComponent<SpriteRenderer>();
		}

		private void Update() {
			if(_targetCollector) {
				var tPos = _targetCollector.transform.position;
				var pos = Vector3.Lerp(transform.position, tPos, _speed * Time.deltaTime);
				transform.position = pos;
				if(Vector3.Distance(transform.position, tPos) < 1f) {
					_targetCollector.AddItem(_itemName, 1);
					Destroy(gameObject);
				}
			}
		}

		private void OnTriggerEnter(Collider co) {
			var collector = co.GetComponent<ItemCollector>();
			if(collector) {
				DetectedCollector(collector);
			}
		}

		/// <summary>
		/// アイテムコレクタの検出範囲に触れる
		/// </summary>
		/// <param name="collector">アイテムコレクタ</param>
		public void DetectedCollector(ItemCollector collector) {
			_targetCollector = collector;
		}
	}
}