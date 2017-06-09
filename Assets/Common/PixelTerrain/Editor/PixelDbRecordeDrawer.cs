using UnityEngine;
using UnityEditor;
using System;

namespace Common.PixelTerrain {

	/// <summary>
	/// ピクセルDBレコードのインスペクタ上の描画
	/// </summary>
	[CustomPropertyDrawer(typeof(PixelDBRecord))]
	public class PixelDBRecordDrawer : PropertyDrawer {

		private static readonly float height = 48f; //合計の高さ

		private PixelDBRecord _record;
		private readonly float _lHeight = 16f;                      //一列の高さ
		private readonly Vector2 _margin = new Vector2(4f, 4f);     //隣り合う要素との距離

		private Sprite _sprite;

		public override void OnGUI(
				Rect rect,
				SerializedProperty property,
				GUIContent label) {

			using(new EditorGUI.PropertyScope(rect, label, property)) {
				var y = rect.y;
				//1段目(id, toID, name)
				var idRect = new Rect(
					x: rect.x,
					y: y,
					width: 100f,
					height: _lHeight
				);
				var toIDRect = new Rect(
					x: rect.x + 100 + _margin.x,
					y: y,
					width: 100f,
					height: _lHeight
				);
				var nameRect = new Rect(
					x: rect.x + 200 + _margin.x * 2f,
					y: y,
					width: rect.width - 200 - _margin.x * 2f,
					height: _lHeight
				);

				var idProp = property.FindPropertyRelative("id");
				var toIDProp = property.FindPropertyRelative("toID");
				var nameProp = property.FindPropertyRelative("name");

				EditorGUIUtility.labelWidth = 40;
				idProp.intValue = EditorGUI.IntField(idRect, idProp.displayName, idProp.intValue);

				toIDProp.intValue =	EditorGUI.IntField(toIDRect, ">>>", toIDProp.intValue);

				EditorGUIUtility.labelWidth = 60;
				nameProp.stringValue = EditorGUI.TextField(nameRect, nameProp.displayName, nameProp.stringValue);

				//2段目(isDraw, durability, color)
				y += _lHeight + _margin.y;
				var isDrawRect = new Rect(
					x: rect.x,
					y: y,
					width: 50f,
					height: _lHeight
				);
				var durabilityRect = new Rect(
					x: rect.x + 50f + _margin.x,
					y: y,
					width: 150f,
					height: _lHeight
				);
				var colorRect = new Rect(
					x: rect.x + 200f + _margin.x * 2f,
					y: y,
					width: rect.width - 200f - _margin.x * 2f,
					height: _lHeight
				);

				var isDrawProp = property.FindPropertyRelative("isDraw");
				var durabilityProp = property.FindPropertyRelative("durability");
				var colorProp = property.FindPropertyRelative("color");

				EditorGUIUtility.labelWidth = 40;
				isDrawProp.boolValue = EditorGUI.Toggle(isDrawRect, "Draw?", isDrawProp.boolValue);

				EditorGUIUtility.labelWidth = 80;
				durabilityProp.intValue = EditorGUI.IntField(durabilityRect, durabilityProp.displayName, durabilityProp.intValue);

				EditorGUIUtility.labelWidth = 60;
				colorProp.colorValue = EditorGUI.ColorField(colorRect, colorProp.displayName, colorProp.colorValue);

				//3~5段目(uvs)
				EditorGUIUtility.labelWidth = 40;
				y += _lHeight + _margin.y;
				var uvRect = new Rect(
					x: rect.x,
					y: y,
					width: rect.width,
					height: rect.height
				);

				y += _lHeight + _margin.y;
				var uvMargin = 16f;
				var uvWidth = (rect.width - uvMargin) / 2;
				var uv0Rect = new Rect(
					x: rect.x,
					y: y + _lHeight,
					width: uvWidth,
					height: _lHeight
				);
				var uv1Rect = new Rect(
					x: rect.x,
					y: y,
					width: uvWidth,
					height: _lHeight
				);
				var uv2Rect = new Rect(
					x: rect.x + uv0Rect.width + uvMargin,
					y: y,
					width: uvWidth,
					height: _lHeight
				);
				var uv3Rect = new Rect(
					x: rect.x + uv0Rect.width + uvMargin,
					y: y + _lHeight,
					width: uvWidth,
					height: _lHeight
				);

				var uvsProp = property.FindPropertyRelative("uvs");
				var uv0Prop = uvsProp.GetArrayElementAtIndex(0);
				var uv1Prop = uvsProp.GetArrayElementAtIndex(1);
				var uv2Prop = uvsProp.GetArrayElementAtIndex(2);
				var uv3Prop = uvsProp.GetArrayElementAtIndex(3);

				EditorGUI.LabelField(uvRect, "uvs");
				uv0Prop.vector2Value = EditorGUI.Vector2Field(uv0Rect, "", uv0Prop.vector2Value);
				uv1Prop.vector2Value = EditorGUI.Vector2Field(uv1Rect, "", uv1Prop.vector2Value);
				uv2Prop.vector2Value = EditorGUI.Vector2Field(uv2Rect, "", uv2Prop.vector2Value);
				uv3Prop.vector2Value = EditorGUI.Vector2Field(uv3Rect, "", uv3Prop.vector2Value);
			}
		}
	}
}