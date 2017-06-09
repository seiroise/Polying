using UnityEngine;
using System;

namespace Common.PixelTerrain {

	/// <summary>
	/// ピクセル地形からのドロップアイテムの管理
	/// </summary>
	[RequireComponent(typeof(PixelTerrain))]
	public class PixelTerrainItemDropper : MonoBehaviour {

		[Header("Drop Item Parameter")]
		[SerializeField]
		private DropItem _itemPrefab;

		private PixelTerrain _terrain;

		private void Awake() {
			_terrain = GetComponent<PixelTerrain>();
		}

		private void Update() {
			DropItemFromTerrain();
		}

		/// <summary>
		/// 地形からのドロップアイテムを生成する
		/// </summary>
		private void DropItemFromTerrain() {
			if(_terrain && _terrain.isExcavated) {
				var excavated = _terrain.GetChanges();
				for(int i = 0; i < excavated.Length; ++i) {
					var item = Instantiate<DropItem>(_itemPrefab);
					item.transform.position = excavated[i].point;
					var record = _terrain.pixelDB.GetCopiedRecord(excavated[i].id);
					item.itemName = record.name;
					item.id = excavated[i].id;
					item.sprite.color = record.color;
				}
			}
		}
	}
}