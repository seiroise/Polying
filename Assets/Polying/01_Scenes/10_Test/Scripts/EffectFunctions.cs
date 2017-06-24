using UnityEngine;
using System.Collections;
using System;
using EditorUtil;
using Polying.World.Explorer;

namespace Polying.Test {

	/// <summary>
	/// エフェクトパラメータ
	/// </summary>
	[Serializable]
	public class EffectParams {
		[SerializeField, MinMaxRange(0.01f, 100f)]
		public MinMax speed;
		[SerializeField, MinMaxRange(-90f, 360f)]
		public MinMax angle;
		[SerializeField, MinMaxRange(0.01f, 100f)]
		public MinMax size;
		[SerializeField, MinMaxRange(0.01f, 100f)]
		public MinMax lifetime;
		[SerializeField]
		public Color32 color;
	}

	/// <summary>
	/// エフェクト関連の関数
	/// </summary>
	public class EffectFunctions {

		/// <summary>
		/// パーティクルの生成
		/// </summary>
		/// <param name="emitter">Emitter.</param>
		/// <param name="param">Parameter.</param>
		/// <param name="position">Position.</param>
		/// <param name="baseAngle">Base angle.</param>
		public static void EmitParticle(ParticleSystem emitter, EffectParams param, Vector3 position, float baseAngle) {
			emitter.Emit(
				position:	position,
				velocity:	Functions.DegToVector(param.angle.random + baseAngle) * param.speed.random,
				size:		param.size.random,
				lifetime:	param.lifetime.random,
				color:		param.color);
		}

		/// <summary>
		/// パーティクルの生成
		/// </summary>
		/// <param name="emitter">Emitter.</param>
		/// <param name="param">Parameter.</param>
		/// <param name="tex">Tex.</param>
		public static void EmitParticle(ParticleSystem emitter, EffectParams param, Transform transform, Sprite sprite) {
			Texture2D tex = sprite.texture;
			Rect rect = sprite.textureRect;
			Color[] colors = tex.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
			Vector3 position = transform.position;

			for(int i = 0; i < colors.Length; ++i) {
				if(colors[i].a > 0f) {
					emitter.Emit(
						position:	position,
						velocity:	Functions.DegToVector(param.angle.random) * param.speed.random,
						size:		param.size.random,
						lifetime:	param.lifetime.random,
						color:		colors[i]);
				}
			}
		}

		/// <summary>
		/// 線状にパーティクルを生成
		/// </summary>
		/// <param name="emitter">Emitter.</param>
		/// <param name="param">Parameter.</param>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		/// <param name="density">Density.</param>
		public static void EmitParticle(ParticleSystem emitter, EffectParams param, Vector3 from, Vector3 to, float density) {
			float distance = Vector3.Distance(from, to);
			Vector3 direction = (to - from).normalized * density;
			while(distance > 0f) {
				distance -= density;
				from.x += direction.x;
				from.y += direction.y;
				emitter.Emit(
					position:	from,
					velocity:	Functions.DegToVector(param.angle.random) * param.speed.random,
					size:		param.size.random,
					lifetime:	param.lifetime.random,
					color:		param.color);
			}
		}
	}
}