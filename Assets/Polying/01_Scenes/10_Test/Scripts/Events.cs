using UnityEngine.Events;
using System;

namespace Polying.Test {

	/// <summary>
	/// intとintを引数にとるHP関連のイベント
	/// </summary>
	public class HPEvent : UnityEvent<int, int> { }

	/// <summary>
	/// AmmoとAmmoを引数にとる弾薬関連のイベント
	/// </summary>
	public class AmmoEvent : UnityEvent<Ammo, Ammo> { }
}