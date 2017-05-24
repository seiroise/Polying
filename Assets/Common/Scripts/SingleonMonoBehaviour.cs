using UnityEngine;
using System;

namespace Common {

	/// <summary>
	/// Unity用シングルトン
	/// </summary>
	public abstract class SingleonMonoBehaviour<T> : MonoBehaviour where T : Component {

		private static T _instance;
		public static T instance {
			get {
				if(!_instance) {
					_instance = FindObjectOfType<T>();
					if(!_instance) {
						throw new Exception(typeof(T) + " is nothing.");
					}
				}
				return _instance;
			}
		}

		[SerializeField]
		private bool _isDontDestroy = false;
		public bool isDontDestroy { get { return _isDontDestroy; } set { _isDontDestroy = value; } }

		/// <summary>
		/// サブクラスではAwakeを定義しないこと
		/// </summary>
		private void Awake() {
			if(_instance != this && _instance != null) {
				Destroy(this);
				return;
			}

			SingletonAwake();
		}

		/// <summary>
		/// サブクラスではこちらのAwakeを継承すること
		/// </summary>
		protected abstract void SingletonAwake();
	}
}