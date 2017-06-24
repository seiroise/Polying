using UnityEngine;
using System;
using Common.ObjectPool;
using Polying.Common;
using TMPro;

namespace Polying.Test {

	/// <summary>
	/// プール可能な表示用テキスト
	/// </summary>
	[RequireComponent(typeof(SingleAnimation))]
	public class PoolableFlowText : MonoBehaviour, IPoolable {

		[SerializeField]
		private TextMeshPro _textMesh;

		private SingleAnimation _anim;

		public TextMeshPro textMesh {
			get {
				return _textMesh;
			}
		}

		private void Awake() {
			_anim = GetComponent<SingleAnimation>();
			_anim.onStopped.AddListener(OnAnimStopped);
		}

		/// <summary>
		/// アニメーション終了時のイベント
		/// </summary>
		private void OnAnimStopped() {
			gameObject.SetActive(false);
		}

		/// <summary>
		/// 起動
		/// </summary>
		public void AwakePoolable() {
			_anim.Play();
		}
	}
}