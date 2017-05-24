#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Text;

namespace Common.GamePad {

	/// <summary>
	/// 軸の種類
	/// </summary>
	public enum AxisType {
		KeyOrMouseButton = 0,
		MouseMoement = 1,
		JoystickAxis = 2
	}

	/// <summary>
	/// 入力軸の情報
	/// </summary>
	public class InputAxisData {
		
		public string name = "";
		public string descriptiveName = "";
		public string descriptiveNegativeName = "";
		public string negativeButton = "";
		public string positiveButton = "";
		public string altNegativeButton = "";
		public string altPositiveButton = "";

		public float gravity = 0f;
		public float dead = 0f;
		public float sensitivity = 0f;

		public bool snap = false;
		public bool invert = false;

		public AxisType type = AxisType.KeyOrMouseButton;

		public int axis = 1;

		public int joyNum = 0;

		private InputAxisData() { }

		/// <summary>
		/// 押すと1を返すキーの設定データを作成する
		/// </summary>
		/// <returns>設定データ</returns>
		/// <param name="name">設定データの名称</param>
		/// <param name="positiveButton">正の入力を行うボタン名</param>
		/// <param name="altPositiveButton">正の入力を行う代替ボタン名</param>
		public static InputAxisData MakeButton(string name, string positiveButton, string altPositiveButton) {
			var axis = new InputAxisData();
			axis.name = name;
			axis.positiveButton = positiveButton;
			axis.altPositiveButton = altPositiveButton;
			axis.gravity = 1000f;
			axis.dead = 0.001f;
			axis.sensitivity = 1000f;
			axis.type = AxisType.KeyOrMouseButton;
			return axis;
		}

		/// <summary>
		/// ゲームパッド用の軸の設定データを作成する
		/// </summary>
		/// <returns>設定データ</returns>
		/// <param name="name">設定データの名称</param>
		/// <param name="joystickNum">ゲームパッド番号</param>
		/// <param name="axisNum">ゲームパッドの軸番号</param>
		public static InputAxisData MakePadAxis(string name, int joystickNum, int axisNum) {
			var axis = new InputAxisData();
			axis.name = name;
			axis.dead = 0.2f;
			axis.sensitivity = 1f;
			axis.type = AxisType.JoystickAxis;
			axis.axis = axisNum;
			axis.joyNum = joystickNum;
			return axis;
		}

		/// <summary>
		/// キーボード用の軸の設定データを作成する
		/// </summary>
		/// <returns>設定データ</returns>
		/// <param name="name">設定データの名称</param>
		/// <param name="negativeButton">負の入力を行うボタン名</param>
		/// <param name="positiveButton">正の入力を行うボタン名</param>
		/// <param name="altPositiveButton">負の入力を行う代替ボタン名</param>
		/// <param name="altNegativeButton">正の入力を行う代替ボタン名</param>
		public static InputAxisData MakeKeyAxis(string name, string negativeButton, string positiveButton, string altPositiveButton, string altNegativeButton) {
			var axis = new InputAxisData();
			axis.name = name;
			axis.negativeButton = negativeButton;
			axis.positiveButton = positiveButton;
			axis.altNegativeButton = altNegativeButton;
			axis.altPositiveButton = altPositiveButton;
			axis.gravity = 3f;
			axis.sensitivity = 3f;
			axis.dead = 0.001f;
			axis.type = AxisType.KeyOrMouseButton;
			return axis;
		}

		/// <summary>
		/// SrializedPropertyから設定データを作成する
		/// </summary>
		/// <returns>設定データ</returns>
		/// <param name="prop">プロパティ</param>
		public static InputAxisData FromSerializedProperty(SerializedProperty prop) {
			var axis = new InputAxisData();

			axis.name = prop.FindPropertyRelative("m_Name").stringValue;
			axis.descriptiveName = prop.FindPropertyRelative("descriptiveName").stringValue;
			axis.descriptiveNegativeName = prop.FindPropertyRelative("descriptiveNegativeName").stringValue;
			axis.negativeButton = prop.FindPropertyRelative("negativeButton").stringValue;
			axis.positiveButton = prop.FindPropertyRelative("positiveButton").stringValue;
			axis.altNegativeButton = prop.FindPropertyRelative("altNegativeButton").stringValue;
			axis.altPositiveButton = prop.FindPropertyRelative("altPositiveButton").stringValue;
			axis.gravity = prop.FindPropertyRelative("gravity").floatValue;
			axis.dead = prop.FindPropertyRelative("dead").floatValue;
			axis.sensitivity = prop.FindPropertyRelative("sensitivity").floatValue;
			axis.snap = prop.FindPropertyRelative("snap").boolValue;
			axis.invert = prop.FindPropertyRelative("invert").boolValue;
			axis.type = (AxisType)prop.FindPropertyRelative("type").intValue;
			axis.axis = prop.FindPropertyRelative("axis").intValue - 1;
			axis.joyNum = prop.FindPropertyRelative("joyNum").intValue;

			return axis;
		}

		/// <summary>
		/// 指定したSerializedPropertyに値を設定する
		/// </summary>
		/// <param name="prop">値を設定するSerializedProperty</param>
		public void SetSerializedProperty(SerializedProperty prop) {
			prop.FindPropertyRelative("m_Name").stringValue = this.name;
			prop.FindPropertyRelative("descriptiveName").stringValue = this.descriptiveName;
			prop.FindPropertyRelative("descriptiveNegativeName").stringValue = this.descriptiveNegativeName;
			prop.FindPropertyRelative("negativeButton").stringValue = this.negativeButton;
			prop.FindPropertyRelative("positiveButton").stringValue = this.positiveButton;
			prop.FindPropertyRelative("altNegativeButton").stringValue = this.altNegativeButton;
			prop.FindPropertyRelative("altPositiveButton").stringValue = this.altPositiveButton;
			prop.FindPropertyRelative("gravity").floatValue = this.gravity;
			prop.FindPropertyRelative("dead").floatValue = this.dead;
			prop.FindPropertyRelative("sensitivity").floatValue = this.sensitivity;
			prop.FindPropertyRelative("snap").boolValue = this.snap;
			prop.FindPropertyRelative("invert").boolValue = this.invert;
			prop.FindPropertyRelative("type").intValue = (int)this.type;
			prop.FindPropertyRelative("axis").intValue = this.axis - 1;
			prop.FindPropertyRelative("joyNum").intValue = this.joyNum;
		}
	}
}
#endif