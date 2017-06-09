using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

namespace Common.PixelTerrain {

	/// <summary>
	/// ピクセルDBデータオブジェクトのインスペクタ拡張
	/// </summary>
	[CustomEditor(typeof(PixelDBDataObject))]
	public class PixelDBDataObjectIns : Editor {

		private ReorderableList _recordsRList;

		private SerializedProperty _recordsProp;
		private SerializedProperty _materialProp;
		private SerializedProperty _jsonFileProp;

		private Texture2D _bgTex;

		private void OnEnable() {
			_recordsProp = serializedObject.FindProperty("_records");
			_materialProp = serializedObject.FindProperty("_material");
			_jsonFileProp = serializedObject.FindProperty("_jsonFile");

			if(!_bgTex) SetBGTex();

			_recordsRList = new ReorderableList(serializedObject, _recordsProp);
			_recordsRList.elementHeight = 96f + 8f;

			//描画時のコールバック
			_recordsRList.drawElementCallback = OnDrawElement;
			_recordsRList.drawHeaderCallback = OnDrawHeader;
			_recordsRList.drawElementBackgroundCallback = OnDrawElementBG;
		}

		public override void OnInspectorGUI() {

			serializedObject.Update();

			//material
			_materialProp.objectReferenceValue = 
				(Material)EditorGUILayout.ObjectField(_materialProp.displayName, _materialProp.objectReferenceValue, typeof(Material), false);

			EditorGUILayout.Space();

			//jsonFile
			_jsonFileProp.stringValue = 
				EditorGUILayout.TextField(_jsonFileProp.displayName, _jsonFileProp.stringValue);

			//Save/Load
			var t = (PixelDBDataObject)target;
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Save")) {
				t.SaveJson();
			}
			if(GUILayout.Button("Load")) {
				t.LoadJson();
			}
			GUILayout.EndHorizontal();

			EditorGUILayout.Space();

			//Records
			_recordsRList.DoLayoutList();

			serializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		/// 背景画像の作成
		/// </summary>
		private void SetBGTex() {
			_bgTex = new Texture2D(1, 1);
			_bgTex.SetPixel(0, 0, new Color(0.75f, 0.75f, 0.75f));
			_bgTex.hideFlags = HideFlags.DontSave;
			_bgTex.wrapMode = TextureWrapMode.Clamp;
			_bgTex.Apply();
		}

		/// <summary>
		/// 要素描画時のコールバック
		/// </summary>
		/// <param name="rect">Rect.</param>
		/// <param name="index">Index.</param>
		/// <param name="isActive">If set to <c>true</c> is active.</param>
		/// <param name="isFocused">If set to <c>true</c> is focused.</param>
		private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused) {
			var element = _recordsProp.GetArrayElementAtIndex(index);
			rect.y += 2f;
			rect.height -= 4f;
			EditorGUI.PropertyField(rect, element);
		}

		/// <summary>
		/// ヘッダー描画時のコールバック
		/// </summary>
		/// <param name="rect">Rect.</param>
		private void OnDrawHeader(Rect rect) {
			EditorGUI.LabelField(rect, _recordsProp.displayName);
		}

		/// <summary>
		/// 要素の背景の描画時のコールバック
		/// </summary>
		/// <param name="rect">Rect.</param>
		/// <param name="index">Index.</param>
		/// <param name="isActive">If set to <c>true</c> is active.</param>
		/// <param name="isFocused">If set to <c>true</c> is focused.</param>
		private void OnDrawElementBG(Rect rect, int index, bool isActive, bool isFocused) {
			if(Event.current.type == EventType.Repaint) {
				if(index % 2 == 0) {
					rect.x += 4f;
					rect.width -= 8f;
					EditorGUI.DrawTextureTransparent(rect, _bgTex, ScaleMode.ScaleAndCrop);
				}
			}
		}
	}
}