using UnityEngine;
using System.Collections;
using Polying.World.Explorer;
using Polying.Common;

namespace Polying.Test {

	/// <summary>
	/// エネミー
	/// </summary>
	[RequireComponent(typeof(RigidbodyController2D))]
	public class Enemy : ShooterEntity {

		[SerializeField, Range(1, 500)]
		private int _shotInterval = 200;
		[SerializeField]
		private EntityDetector _detector;

		private RigidbodyController2D _controller;
		private Transform _target;

		protected override void Awake() {
			base.Awake();
			_controller = GetComponent<RigidbodyController2D>();
			if(_detector) {
				_detector.onEntered.RemoveListener(OnEntityEntered);
				_detector.onEntered.AddListener(OnEntityEntered);
				_detector.onExited.RemoveListener(OnEntityExited);
				_detector.onExited.AddListener(OnEntityExited);
			}
		}

		protected override void Start() {
			base.Start();
			StartCoroutine(SequentiallyAct());
		}

		private void Update() {
			//一定時間ごとに攻撃
			if(_target) {
				_controller.SetGazeAngle(Functions.VectorToDeg((_target.position - transform.position).normalized));
				if(Random.Range(0, _shotInterval) == 0) {
					PullTrigger();
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D co) {
			if(!co.tag.Equals("Player")) return;
			Detected(co.transform);
		}

		private void OnTriggerExit2D(Collider2D co) {
			if(co.transform != _target) return;
			Release();
		}

		/// <summary>
		/// ターゲットの検出
		/// </summary>
		/// <returns>The detected.</returns>
		/// <param name="target">Target.</param>
		private void Detected(Transform target) {
			_target = target;
		}

		/// <summary>
		/// ターゲットの解放
		/// </summary>
		private void Release() {
			_target = null;
		}

		/// <summary>
		/// エンティティの侵入
		/// </summary>
		/// <param name="entity">Entity.</param>
		private void OnEntityEntered(Entity entity) {
			Detected(entity.transform);
		}

		/// <summary>
		/// エンティティの離脱
		/// </summary>
		/// <param name="entity">Entity.</param>
		private void OnEntityExited(Entity entity) {
			Release();
		}

		/// <summary>
		/// ランダムに行動
		/// </summary>
		/// <returns>The act.</returns>
		private IEnumerator RandomAct() {
			while(true) {
				if(Random.Range(0, 2) == 0) {
					Debug.Log("Stay");
					yield return StartCoroutine(ActStay(Random.Range(1f, 5f)));
				} else {
					Debug.Log("Random walk");
					yield return StartCoroutine(ActRandomWalk(Random.Range(3f, 10f)));
				}
			}
		}

		/// <summary>
		/// 順次行動
		/// </summary>
		/// <returns>The act.</returns>
		private IEnumerator SequentiallyAct() {
			while(true) {
				yield return StartCoroutine(ActStay(Random.Range(1f, 3f)));
				yield return StartCoroutine(ActRandomWalk(Random.Range(2f, 4f)));
			}
		}

		/// <summary>
		/// ターゲットの追跡
		/// </summary>
		/// <returns>The tracking.</returns>
		private IEnumerator ActTracking(float time) {
			Vector3 dir;
			float timer = 0f;
			while(_target && timer < time) {
				timer += Time.deltaTime;
				dir = _target.position - transform.position;
				_controller.SetMoveDir(dir.x, dir.y);
				yield return 0f;
			}
		}

		/// <summary>
		/// 一定の距離を保って周囲を移動
		/// </summary>
		/// <returns>The around.</returns>
		private IEnumerator ActWalkAround(float time, float distance) {
			Vector3 dir;
			float timer = 0f;
			float dis;
			Quaternion axis;
			if(Random.Range(0, 2) == 0) {
				axis = Quaternion.AngleAxis(90f, new Vector3(0f, 0f, 1f));
			} else {
				axis = Quaternion.AngleAxis(-90f, new Vector3(0f, 0f, 1f));
			}
			while(_target && timer < time) {
				timer += Time.deltaTime;
				dir = _target.position - transform.position;
				dis = Vector3.Distance(transform.position, _target.position);
				if(dis > distance) {
					dir = axis * dir + dir;
				} else {
					dir = axis * dir - dir;
				}
				_controller.SetMoveDir(dir.x, dir.y);
				yield return 0f;
			}
		}
	
		/// <summary>
		/// ランダムに歩く
		/// </summary>
		/// <returns>The random walk.</returns>
		/// <param name="time">Time.</param>
		private IEnumerator ActRandomWalk(float time) {
			float timer = 0f;
			float x, y;
			x = Random.Range(-1f, 1f);
			y = Random.Range(-1f, 1f);
			while(timer < time) {
				timer += Time.deltaTime;
				_controller.SetMoveDir(x, y);
				yield return 0f;
			}
		}
	
		/// <summary>
		/// 待機する
		/// </summary>
		/// <returns>The stay.</returns>
		/// <param name="time">Time.</param>
		private IEnumerator ActStay(float time) {
			_controller.SetMoveDir(0f, 0f);
			yield return new WaitForSeconds(time);
		}
	}
}