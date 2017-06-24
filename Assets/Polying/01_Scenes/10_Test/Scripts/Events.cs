using UnityEngine;
using UnityEngine.Events;
using System;

namespace Polying.Test {

	/// <summary>
	/// Entityを引数にとるイベント
	/// </summary>
	public class EntityEvent : UnityEvent<Entity> { }

	/// <summary>
	/// intとintを引数にとるHP関連のイベント
	/// </summary>
	public class HPEvent : UnityEvent<int, int> { }

	/// <summary>
	/// AmmoとAmmoを引数にとる弾薬関連のイベント
	/// </summary>
	public class AmmoEvent : UnityEvent<Ammo, Ammo> { }

	/// <summary>
	/// Vector3とDropItemを引数にとるテキスト表示関連のイベント
	/// </summary>
	public class PickUpItemEvent : UnityEvent<Vector3, DropItem> { }
}