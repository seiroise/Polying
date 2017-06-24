using UnityEngine;

namespace Polying.Test {

	/// <summary>
	/// Entityに関するアイテムドロップ
	/// </summary>
	public class DropItemManager : MonoBehaviour {

		[SerializeField]
		private DropItem[] _normalEnemyDrops;
		[SerializeField]
		private FlowTextPool _textPool;

		private void Start() {
			var em = EntityManager.instance;
			em.onEntityDied.RemoveListener(OnEntityDied);
			em.onEntityDied.AddListener(OnEntityDied);
		}

		/// <summary>
		/// Entityの死亡
		/// </summary>
		/// <param name="entity">Entity.</param>
		private void OnEntityDied(Entity entity) {
			switch(entity.type) {
			case Entity.EntityType.NormalEnemy:
				DropNormalEnemy(entity.transform.position);
				break;
			}
		}

		/// <summary>
		/// 通常の敵のドロップ
		/// </summary>
		private void DropNormalEnemy(Vector3 position) {
			if(Random.Range(0, 4) == 0) {
				var item = Instantiate(_normalEnemyDrops[Random.Range(0, _normalEnemyDrops.Length)], position, Quaternion.identity);
				item.onPickUp.AddListener(OnPickUp);
			}
		}

		/// <summary>
		/// アイテムを拾った時のイベント
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="item">Item.</param>
		private void OnPickUp(Vector3 position, DropItem item) {
			if(_textPool) {
				_textPool.Show(position, item.GetPickUpMsg());
			}
		}
	}
}