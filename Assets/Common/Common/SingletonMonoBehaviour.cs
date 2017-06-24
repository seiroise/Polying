using UnityEngine;
using System;

namespace Common.Common {

	/// <summary>
	/// シングルトン
	/// </summary>
	public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component {

		private static T _instance;
		public static T instance {
			get {
				if(!_instance) {
					_instance = FindObjectOfType<T>();
					if(!_instance) {
						Type t = typeof(T);
						Debug.LogError(string.Format("{0} is not exit!", t.Name));
						var obj = new GameObject(t.Name, t);
						_instance = obj.GetComponent<T>();
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
			if(_isDontDestroy) {
				DontDestroyOnLoad(this);
			}
			SingletonAwake();
		}

		/// <summary>
		/// サブクラスではこちらのAwakeを継承すること
		/// </summary>
		protected abstract void SingletonAwake();
	}
}