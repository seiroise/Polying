using System;

namespace Common.PixelTerrain {

	/// <summary>
	/// コピー可能
	/// </summary>
	public interface ICloneable<T> {

		/// <summary>
		/// コピーの作成
		/// </summary>
		/// <returns>The clone.</returns>
		T Clone();
	}
}