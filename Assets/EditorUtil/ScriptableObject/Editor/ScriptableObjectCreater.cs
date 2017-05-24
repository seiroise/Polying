using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

public class ScriptableObjectCreater : MonoBehaviour {

	#region Function

	/// <summary>
	/// ScriptableObjectの作成
	/// </summary>
	[MenuItem("ScriptableObject/Create")]
	public static void Create() {

		var instance = ScriptableObject.CreateInstance<ScriptableObject>();
		var directory = AssetDatabase.GetAssetPath(Selection.activeObject);

		if (string.IsNullOrEmpty(directory)) {
			directory = "Assets";
		}

		var extention = Path.GetExtension(directory);

		if (!string.IsNullOrEmpty(extention)) {
			var filename = Path.GetFileName(directory);
			var startIndex = directory.LastIndexOf(filename) - 1;
			var count = filename.Length + 1;
			directory = directory.Remove(startIndex, count);
		}

		var path = directory + "/" + "NewScriptableObject.asset";
		var uniquePath = AssetDatabase.GenerateUniqueAssetPath(path);

		AssetDatabase.CreateAsset(instance, uniquePath);
		AssetDatabase.SaveAssets();
		Selection.activeObject = instance;
	}

	#endregion
}