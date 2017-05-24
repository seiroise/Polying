using UnityEngine;
using System.Collections;

namespace EditorUtil {

	/// <summary>
	/// EditorUtilデモ
	/// </summary>
	public class Demo : MonoBehaviour {

		[MinMaxRange]
		public MinMax minmax01;
		[MinMaxRange(0, 100)]
		public MinMax minmax;

		[Button("Hello World!", "OnButtonPush")]
		public int btn1;
		[Button("Hello!", "OnHello", 100)]
		public int btn2;

		[Disable]
		public string fixedName = "Hello World!";

		public void OnButtonPush() {
			Debug.Log("Hello World!");
		}

		public void OnHello(int d) {
			Debug.Log(string.Format("Hello {0}", d));
		}
	}
}