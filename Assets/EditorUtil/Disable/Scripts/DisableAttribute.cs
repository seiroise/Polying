using UnityEngine;
using System;
using System.Collections;

namespace EditorUtil {

	/// <summary>
	/// 固定値属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public class DisableAttribute : PropertyAttribute { }
}