using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Reflection;
using System.Linq;

namespace EditorUtil {

	/// <summary>
	/// ボタン属性の描画
	/// </summary>
	[CustomPropertyDrawer(typeof(ButtonAttribute))]
	public class ButtonAttrDrawer : PropertyDrawer {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			var btnAttr = (ButtonAttribute)attribute;

			if (GUI.Button(position, btnAttr.btnName)) {
				var objectReferenceValue = property.serializedObject.targetObject;
				var type = objectReferenceValue.GetType();
				var bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
				var method = type.GetMethod(btnAttr.funcName, bindingAttr);

				try {
					method.Invoke(objectReferenceValue, btnAttr.parameters);
				} catch (AmbiguousMatchException) {
					//複数存在
					var format = @"{0}.{1} 関数がオーバーロードされているため関数を特定できません。{0}.{1} 関数のオーバーロードを削除してください";
					var message = string.Format(format, type.Name, btnAttr.funcName);

					Debug.LogError(message, objectReferenceValue);
				} catch (ArgumentException) {
					//不適切な引数
					var parameters = string.Join(", ", btnAttr.parameters.Select(c => c.ToString()).ToArray());
					var format = @"{0}.{1} 関数に引数 {2} を渡すことができません。{0}.{1} 関数の引数の型が正しいかどうかを確認してください";
					var message = string.Format(format, type.Name, btnAttr.funcName, parameters);

					Debug.LogError(message, objectReferenceValue);
				} catch (NullReferenceException) {
					//関数未定義
					var format = @"{0}.{1} 関数は定義されていません。{0}.{1} 関数が定義されているかどうかを確認してください";
					var message = string.Format(format, type.Name, btnAttr.funcName);

					Debug.LogError(message, objectReferenceValue);
				} catch(TargetParameterCountException) {
					//パラメータの不適切
					var parameters = string.Join(", ", btnAttr.parameters.Select(c => c.ToString()).ToArray());
					var format = @"{0}.{1} 関数に引数 {2} を渡すことができません。{0}.{1} 関数の引数の数が正しいかどうかを確認してください";
					var message = string.Format(format, type.Name, btnAttr.funcName, parameters);

					Debug.LogError(message, objectReferenceValue);
				}
			}
		}
	}
}