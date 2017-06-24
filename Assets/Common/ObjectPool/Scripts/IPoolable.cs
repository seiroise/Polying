using System;

namespace Common.ObjectPool {

	/// <summary>
	/// プール可能インタフェース
	/// </summary>
	public interface IPoolable {

		/// <summary>
		/// 起動
		/// </summary>
		void AwakePoolable();
	}
}