using System;

namespace EditorUtil {

	/// <summary>
	/// stringとfloatのペア
	/// </summary>
	[Serializable]
	public class StringFloat : KeyAndValue<string, float> { }

	/// <summary>
	/// stringとfloatのテーブル
	/// </summary>
	[Serializable]
	public class StringFloatTable : SerializableTable<string, float, StringFloat> { }

	/// <summary>
	/// stringとstringのペア
	/// </summary>
	[Serializable]
	public class StringString : KeyAndValue<string, string> { }

	/// <summary>
	/// stringとstringのテーブル
	/// </summary>
	[Serializable]
	public class StringStringTable : SerializableTable<string, string, StringString> { }
}