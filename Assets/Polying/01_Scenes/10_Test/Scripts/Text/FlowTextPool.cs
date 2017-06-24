using UnityEngine;
using System;
using Common.ObjectPool;

namespace Polying.Test {

	public class FlowTextPool : ObjectPool<PoolableFlowText> {

		[SerializeField]
		private PoolableFlowText _flowText;

		private string _textName;

		protected override void Awake() {
			base.Awake();
			_textName = _flowText.name;
			RegistObject(_flowText);
		}

		/// <summary>
		/// 表示
		/// </summary>
		/// <returns>The show.</returns>
		/// <param name="text">Text.</param>
		/// <param name="position">Position.</param>
		public void Show(Vector3 position, string text) {
			var obj = GetPool(_textName).GetObject(position);
			obj.textMesh.text = text;
		}
	}
}