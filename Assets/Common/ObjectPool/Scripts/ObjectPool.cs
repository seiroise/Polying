using UnityEngine;
using System;
using System.Collections.Generic;

namespace Common.ObjectPool {

	/// <summary>
	/// オブジェクトプール
	/// </summary>
	public abstract class ObjectPool<T> : MonoBehaviour where T : Component, IPoolable {

		/// <summary>
		/// 内部プール
		/// </summary>
		[Serializable]
		public class InnerPool {
			
			[SerializeField]
			private T origin;
			private int currentIndex;
			private int currentSize;
			private int addNum;
			private Transform parent;
			private List<T> pool;

			public InnerPool(T origin, int initNum, int addNum, Transform parent) {
				this.origin = origin;
				this.currentIndex = 0;
				this.currentSize = 0;
				this.addNum = addNum;
				this.parent = parent;
				this.pool = new List<T>();
				AddObjects(initNum);
			}

			/// <summary>
			/// オブジェクトの追加
			/// </summary>
			/// <param name="num">Number.</param>
			private void AddObjects(int num) {
				for (int i = 0; i < num; ++i) {
					GameObject obj = Instantiate(origin.gameObject);
					obj.name = origin.name;
					obj.transform.SetParent(parent);
					obj.SetActive(false);
					pool.Add(obj.GetComponent<T>());
				}
				currentSize = pool.Count;
			}

			/// <summary>
			/// オブジェクトの追加
			/// </summary>
			/// <returns>The object.</returns>
			/// <param name="position">Position.</param>
			public T GetObject(Vector3 position) {
				T obj;
				for (int i = 0; i < currentSize; ++i) {
					currentIndex = (currentIndex + 1) % currentSize;
					obj = pool[currentIndex];
					if (!obj.gameObject.activeInHierarchy) {
						return ActivateObj(obj, position); ;
					}
				}
				//要素の追加
				currentIndex = currentSize;
				AddObjects(addNum);
				return ActivateObj(pool[currentIndex], position); ;
			}

			/// <summary>
			/// オブジェクトの有効化
			/// </summary>
			/// <returns>The object.</returns>
			/// <param name="obj">Object.</param>
			/// <param name="position">Position.</param>
			private T ActivateObj(T obj, Vector3 position) {
				obj.gameObject.SetActive(true);
				obj.transform.position = position;
				obj.AwakePoolable();
				return obj;
			}

			/// <summary>
			/// 複数のオブジェクトの取得
			/// </summary>
			/// <returns>The objects.</returns>
			/// <param name="num">Number.</param>
			/// <param name="position">Position.</param>
			public T[] GetObjects(int num, Vector3 position) {
				T[] objs = new T[num];
				for (int i = 0; i < num; ++i) {
					objs[i] = GetObject(position);
				}
				return objs;
			}
		}

		[SerializeField, Range(1, 128)]
		private int initNum = 16;
		[SerializeField, Range(1, 128)]
		private int addNum = 16;

		private Dictionary<string, InnerPool> poolDic;

		protected virtual void Awake() {
			poolDic = new Dictionary<string, InnerPool>();
		}

		/// <summary>
		/// オブジェクトの登録
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="obj">Object.</param>
		public InnerPool RegistObject(T obj) {
			if (obj == null) return null;
			InnerPool pool;
			if(!poolDic.TryGetValue(obj.name, out pool)) {
				pool = new InnerPool(obj, initNum, addNum, transform);
				poolDic.Add(obj.name, pool);
			}
			return pool;
		}

		/// <summary>
		/// プールの取得
		/// </summary>
		/// <returns>The pool.</returns>
		/// <param name="obj">Object.</param>
		public InnerPool GetPool(T obj) {
			InnerPool pool;
			poolDic.TryGetValue(obj.name, out pool);
			return pool;
		}

		/// <summary>
		/// プールの取得
		/// </summary>
		/// <returns>The pool.</returns>
		/// <param name="name">Name.</param>
		public InnerPool GetPool(string name) {
			InnerPool pool;
			poolDic.TryGetValue(name, out pool);
			return pool;
		}
	}
}