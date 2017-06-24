using UnityEngine;
using System;
using System.Collections.Generic;
using Common.Common;

namespace Polying.Test {

	/// <summary>
	/// Entityの管理
	/// </summary>
	public class EntityManager : SingletonMonoBehaviour<EntityManager> {

		private HashSet<Entity> _entitySet;
		private EntityEvent _onEntityDied;

		public EntityEvent onEntityDied {
			get {
				return _onEntityDied;
			}
		}

		protected override void SingletonAwake() {
			_entitySet = new HashSet<Entity>();
			_onEntityDied = new EntityEvent();
		}

		/// <summary>
		/// 管理下にあるエンティティの死亡
		/// </summary>
		/// <param name="entity">Entity.</param>
		private void OnEntityEied(Entity entity) {
			_entitySet.Remove(entity);
			_onEntityDied.Invoke(entity);
			Debug.Log(string.Format("{0} is died", entity.name));
		}

		/// <summary>
		/// 登録
		/// </summary>
		public void RegistEntity(Entity entity) {
			_entitySet.Add(entity);
			entity.onDied.RemoveListener(OnEntityEied);
			entity.onDied.AddListener(OnEntityEied);
		}
	}
}