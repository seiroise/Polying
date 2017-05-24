using UnityEngine;
using System;
using System.Collections;

namespace EditorUtil {

	/// <summary>
	/// SerializableなKeyValuePair
	/// </summary>
	[Serializable]
	public class KeyAndValue<TKey, TValue> {

		[SerializeField]
		private TKey _key;
		public TKey key { get { return _key; } set { _key = value; } }

		[SerializeField]
		private TValue _value;
		public TValue value { get { return _value; } set { _value = value; } }
	}
}