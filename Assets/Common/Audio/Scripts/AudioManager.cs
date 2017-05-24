using UnityEngine;
using System;
using System.Collections.Generic;

namespace Common.Audio {

	/// <summary>
	/// 音声再生の管理
	/// </summary>
	public sealed class AudioManager : MonoBehaviour {

		/// <summary>
		/// 主にBGM向けのフェード処理を行う
		/// </summary>
		private class BGMFader {

			private AudioSource _src;           //管理する音源
			private float _mainVolume;          //主音量

			private bool _isFading;             //フェード処理中
			private float _nVolume;             //正規化された音量
			private float _nTargetVolume;       //正規化された目標音量
			private float _nDeltaVolume;        //正規化された目標音量へ指定時間で到達するための差分音量

			private FadeOption _fadeOpt;        //フェードオプション
			private Action _fadedCallback;      //フェード処理の終了コールバック

			private bool _isTimeMeasuring;		//時間計測中
			private float _timer;				//ダウンタイマー
			private Action _timerCallback;		//タイマー終了時のコールバック

			private AudioClip _nextClip;        //次に再生する音声クリップ

			//Acceser
			public float mainVolume {
				get {
					return _mainVolume;
				}
				set {
					value = Mathf.Clamp01(value);
					_mainVolume = value;
					_src.volume = _mainVolume * _nVolume;
				}
			}
			public float nVolume {
				get {
					return _nVolume;
				}
			}
			public bool isFading {
				get {
					return _isFading;
				}
			}
			public FadeOption fadeOpt {
				get {
					return _fadeOpt;
				}
				set {
					_fadeOpt = value;
				}
			}

			public BGMFader(AudioSource src, FadeOption opt) {
				_src = src;
				_mainVolume = src.volume;
				_nVolume = 0f;
				_fadeOpt = opt;
			}

			/// <summary>
			/// 指定した音量になるまで音量差分だけ音量を変化させる
			/// </summary>
			/// <param name="nTargetVolume">目標音量</param>
			/// <param name="nDeltaVolume">音量差分</param>
			private void FadeVolume(float nTargetVolume, float nDeltaVolume) {
				_nVolume += nDeltaVolume * Time.deltaTime;
				if((nDeltaVolume < 0f && _nVolume < nTargetVolume) || (nDeltaVolume > 0f && _nVolume > nTargetVolume)) {
					_nVolume = nTargetVolume;
					_isFading = false;
				}
				_src.volume = _nVolume * _mainVolume;

				if(!_isFading && _fadedCallback != null) _fadedCallback();
			}

			/// <summary>
			/// タイマーによる計測を開始
			/// </summary>
			/// <param name="time">計測時間</param>
			/// <param name="callback">終了時のコールバック</param>
			private void StartTimer(float time, Action callback) {
				_timer = time;
				_timerCallback = callback;
				_isTimeMeasuring  =true;
			}

			/// <summary>
			/// タイマーの更新
			/// </summary>
			private void UpdateTimer() {
				_timer -= Time.deltaTime;
				if(_timer <= 0f) {
					if(_timerCallback != null) _timerCallback();
					_isTimeMeasuring = false;
				}
			}

			/// <summary>
			/// 音量の更新処理
			/// メインループから毎フレーム呼ぶようにしてください
			/// </summary>
			public void Update() {
				if(_isTimeMeasuring) {
					UpdateTimer();
				}
				if(_isFading) {
					FadeVolume(_nTargetVolume, _nDeltaVolume);
				}
			}

			/// <summary>
			/// BGMを指定した音量に指定した時間をかけて徐々に変化させる
			/// </summary>
			/// <param name="targetVolume">目標音量(0 ~ 1)</param>
			/// <param name="fadeTime">フェード時間</param>
			/// <param name="callback">フェード終了時に呼ばれるコールバック</param>
			public void Fade(float targetVolume, float fadeTime, Action callback) {
				Mathf.Clamp01(targetVolume);
				_nDeltaVolume = (targetVolume - _nTargetVolume) / fadeTime;
				_nTargetVolume = targetVolume;
				_isFading = true;
				_fadedCallback = callback;
			}

			/// <summary>
			/// 音源のならす音声クリップを設定し鳴らす
			/// </summary>
			/// <param name="nextClip">次に再生する音声クリップ</param>
			public void Play(AudioClip nextClip) {
				if(_src.isPlaying) {
					//すでに何かしらの音声クリップが再生されている場合はフェード処理を行い変更する
					Change(nextClip);
				} else {
					_src.clip = nextClip;
					_src.Play();
					Fade(1f, _fadeOpt.inTime, null);
				}
			}

			/// <summary>
			/// 音源のならす音声クリップを変更する
			/// </summary>
			/// <param name="nextClip">次に再生する音声クリップ</param>
			public void Change(AudioClip nextClip) {
				_nextClip = nextClip;

				//フェードイン関数
				Action fadeIn = () => {
					_src.clip = _nextClip;
					_src.Play();
					Fade(1f, _fadeOpt.inTime, null);
				};
				//フェード処理
				Fade(0f, _fadeOpt.outTime, () => {
					if(_fadeOpt.intervalTime > 0f) {
						//インターバルが必要な場合はさらにコールバックをつなぐ
						StartTimer(_fadeOpt.intervalTime, fadeIn);
					} else {
						//インターバルが不要な場合はそのままフェードイン
						fadeIn();
					}
				});
			}
		}

		/// <summary>
		/// フェードに関するオプション
		/// </summary>
		[Serializable]
		public struct FadeOption {

			[SerializeField, Range(0f, 10f)]
			private float _inTime;              //フェードイン時間
			[SerializeField, Range(0f, 10f)]
			private float _outTime;             //フェードアウト時間
			[SerializeField, Range(0f, 10f)]
			private float _intervalTime;        //フェードイン/アウト間の時間

			//Acceser
			public float outTime { get { return _outTime; } }
			public float inTime { get { return _inTime; } }
			public float intervalTime { get { return _intervalTime; } }

			public FadeOption(float inTime, float outTime, float intervalTime) {
				_inTime = inTime;
				_outTime = outTime;
				_intervalTime = intervalTime;
			}
		}

		/// <summary>
		/// SE再生タスクのスケジュール管理を行う
		/// </summary>
		private class SETaskScheduler {

			/// <summary>
			/// SE再生用のタスク
			/// </summary>
			private class SETask {
				public AudioSource playSrc;         //再生用の音源
				public AudioClip playClip;          //再生するクリップ

				public SETask(AudioSource src, AudioClip clip) {
					playSrc = src;
					playClip = clip;
				}
			}

			private AudioSource[] _srcs;            //音源
			private int _srcNum;                    //音源数
			private int _nextSrcIndex;              //次に音声をならす予定の音源番号

			private float _volume;                  //音源のボリューム

			private Queue<AudioSource> _seSrcQueue; //SE再生キュー

			private Dictionary<string, SETask> _seTasks;	//タスク

			//Acceser
			public float volume {
				get {
					return _volume;
				}
				set {
					_volume = value;
					foreach(var s in _srcs) s.volume = value;
				}
			}

			public SETaskScheduler(AudioSource[] srcs, float volume) {
				_srcs = srcs;
				_srcNum = srcs.Length;

				_volume = volume;

				_seSrcQueue = new Queue<AudioSource>();
				for(int i = 0; i < _srcNum; ++i) {
					_seSrcQueue.Enqueue(srcs[i]);
				}
				_seTasks = new Dictionary<string, SETask>();

				_nextSrcIndex = 0;
			}

			/// <summary>
			/// 空いているSE用のソースを返す。
			/// 空いているSE用のソースがない場合はnullを返す
			/// </summary>
			/// <returns>取得したSE用の音源</returns>
			private AudioSource FindFreeSEAudioSource() {
				for(int i = 0; i < _srcNum; i++) {
					if(!_srcs[_nextSrcIndex].isPlaying) {
						return _srcs[_nextSrcIndex];
					}
					_nextSrcIndex = (_nextSrcIndex + 1) % _srcNum;
				}
				return null;
			}

			/// <summary>
			/// SE再生タスクに仕事を割振る
			/// </summary>
			/// <param name="src">再生させる音源</param>
			/// <param name="clip">再生する音声</param>
			private void IssueSETask(AudioSource src, AudioClip clip) {
				var clipName = clip.name;
				if(!_seTasks.ContainsKey(clipName)) {
					_seTasks.Add(clipName, new SETask(src, clip));
				}
			}

			/// <summary>
			/// SE再生タスクの実行
			/// </summary>
			private void ExeSETask() {
				foreach(var task in _seTasks.Values) {
					task.playSrc.PlayOneShot(task.playClip);
					_seSrcQueue.Enqueue(task.playSrc);
				}
				_seTasks.Clear();
			}

			/// <summary>
			/// 更新処理
			/// メインループから毎フレーム呼ぶようにしてください
			/// </summary>
			public void Update() {
				if(_seTasks.Count > 0) {
					ExeSETask();
				}
			}

			/// <summary>
			/// 指定したクリップをSEとして再生
			/// </summary>
			/// <param name="clip">音声クリップ</param>
			public void Play(AudioClip clip) {
				//空いているSE音源の取得
				var src = FindFreeSEAudioSource();
				if(!src) {
					//最も再生されてから時間の経っているSE音源を使用する
					src = _seSrcQueue.Peek();
				}
				IssueSETask(src, clip);
			}
		}

		[Header("BGM Parameter")]
		[SerializeField, Range(0f, 1f)]
		private float _initBGMVolume = 0.75f;

		[SerializeField]
		private FadeOption _initFadeOpt;

		[Header("SE Parameter")]
		[SerializeField, Range(0f, 1f)]
		private float _initSEVolume = 0.75f;
		[SerializeField, Range(1, 20)]
		private int _initSESourceNum = 10;

		//BGm Private Parameter
		private BGMFader _bgm;

		//SE Private Parameter
		private SETaskScheduler _se;

		//Acceser
		public FadeOption fadeOpt {
			get {
				return _bgm.fadeOpt;
			}
			set {
				_bgm.fadeOpt = value;
			}
		}
		public float bgmVolume {
			get {
				return _bgm.mainVolume;
			}
			set {
				_bgm.mainVolume = value;
			}
		}
		public float seVolume {
			get {
				return _se.volume;
			}
			set {
				_se.volume = value;
			}
		}

		private void Awake() {
			//BGM
			_bgm = new BGMFader(AddAudioSource(_initBGMVolume, 128, true), _initFadeOpt);

			//SE
			var srcs = new AudioSource[_initSESourceNum];
			for(int i = 0; i < _initSESourceNum; ++i) {
				srcs[i] = AddAudioSource(_initSEVolume, 128, false);
			}
			_se = new SETaskScheduler(srcs, _initSEVolume);
		}

		private void Update() {
			_bgm.Update();
			_se.Update();
		}

		/// <summary>
		/// GameObjectへのAudioSourceの追加
		/// </summary>
		/// <returns>追加した</returns>
		/// <param name="volume">音量(0-1)</param>
		/// <param name="priority">優先度(0-255)</param>
		/// <param name="isLoop">ループ処理</param>
		private AudioSource AddAudioSource(float volume, int priority, bool isLoop) {
			var source = gameObject.AddComponent<AudioSource>();
			source.volume = volume;
			source.priority = priority;
			source.loop = isLoop;

			return source;
		}

		/// <summary>
		/// 指定したクリップをBGMとして再生
		/// </summary>
		/// <param name="clip">音声クリップ</param>
		public void PlayBGM(AudioClip clip) {
			_bgm.Play(clip);
		}

		/// <summary>
		/// フェードオプションの設定
		/// </summary>
		/// <param name="opt">フェードオプション</param>
		public void SetFadeOpt(FadeOption opt) {
			_bgm.fadeOpt = opt;
		}

		/// <summary>
		/// 指定したクリップをSEとして再生
		/// </summary>
		/// <param name="clip">音声クリップ</param>
		public void PlaySE(AudioClip clip) {
			_se.Play(clip);
		}
	}
}