using UnityEngine;
using System;
using System.Collections;

namespace EditorUtil {

	/// <summary>
	/// ボタン表示属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public class ButtonAttribute : PropertyAttribute {

		public readonly string btnName;
		public readonly string funcName;
		public readonly object[] parameters;

		public ButtonAttribute(string btnName, string funcName, params object[] parameters) {
			this.btnName = btnName;
			this.funcName = funcName;
			this.parameters = parameters;
		}
	}
}