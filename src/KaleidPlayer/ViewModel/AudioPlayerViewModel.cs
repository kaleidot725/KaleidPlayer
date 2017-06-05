using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using kaleidot725.Model;
using kaleidot725.Model.Library;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace kaleidot725.ViewModel
{
    class AudioPlayerViewModel : BindableBase
    {
        public DelegateCommand PlayCommand { get; }
        public DelegateCommand ReplayCommand { get; }
        public DelegateCommand SeekCommand { get; }
        public DelegateCommand NextCommand { get; }
        public DelegateCommand ForwardCommand { get; }
        public DelegateCommand RepeatCommand { get; }

        /// <summary>
        /// ファイル名
        /// </summary>
        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set { SetProperty(ref _fileName, value); }
        }

        /// <summary>
        /// 再生状態
        /// </summary>
        private bool _isPlay;
        public bool IsPlay
        {
            get { return _isPlay; }
            set { SetProperty(ref _isPlay, value); }
        }

        /// <summary>
        /// リピート
        /// </summary>
        private bool _isRepeat;
        public bool IsRepeat
        {
            get { return _isRepeat; }
            set { SetProperty(ref _isRepeat, value); }
        }

        /// <summary>
        /// シーク現在位置
        /// </summary>
        private int _seekNow;
        public int SeekNow
        {
            get { return _seekNow; }
            set { SetProperty(ref _seekNow, value); }
        }

        /// <summary>
        /// シーク最大値
        /// </summary>
        private int _seekMax;
        public int SeekMax
        {
            get { return _seekMax; }
            set { SetProperty(ref _seekMax, value); }
        }

        /// <summary>
        /// 音楽ファイル 再生位置
        /// </summary>
        private TimeSpan _currentTime;
        public TimeSpan CurrentTime
        {
            get { return _currentTime; }
            set { SetProperty(ref _currentTime, value); }
        }

        /// <summary>
        /// 音声ファイル 合計時間
        /// </summary>
        private TimeSpan _totaltime;
        public TimeSpan TotalTime
        {
            get { return _totaltime; }
            set { SetProperty(ref _totaltime, value); }
        }


        /// <summary>
        /// 再生している音楽ファイル情報
        /// </summary>
        private IAudioDetail _nowPlayMusic;
        public IAudioDetail NowPlayMusic
        {
            get { return _nowPlayMusic; }
            set { SetProperty(ref _nowPlayMusic, value); }
        }

        private AudioPlayer _audioPlayer;
        private SongSearcher _songsSearcher;
        private ArtistList _artistList;
        private AlbumList _albumList;
        private AudioPlaylist _playlist;
        private ApplicationSetting _setting;

        private IObservable<long> _timer;
        private IDisposable _subscription;
        private Dispatcher _uiDispatcher;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AudioPlayerViewModel()
        {
            // 再生状態
            _isPlay = false;
            _nowPlayMusic = new AudioNullDetail();
            _timer = Observable.Interval(TimeSpan.FromMilliseconds(10));
            _subscription = _timer.Subscribe(i =>
            {
                UpdatePlayerUI();

            },
               ex => Console.WriteLine("OnError({0})", ex.Message),
               () => Console.WriteLine("Completed()"));

            // ライブラリ群
            _audioPlayer = SingletonModels.GetAudioPlayerInstance();
            _songsSearcher = SingletonModels.GetAudioSearcherInstance();
            _artistList = SingletonModels.GetArtistListInstance();
            _albumList = SingletonModels.GetAlbumListInstance();
            _playlist = SingletonModels.GetAudioPlaylist();
            _setting = SingletonModels.GetApplicationSetting();
            _uiDispatcher = Dispatcher.CurrentDispatcher;

            // コマンド作成
            this.ReplayCommand = new DelegateCommand(Replay);
            this.SeekCommand = new DelegateCommand(Seek);
            this.NextCommand = new DelegateCommand(Next);
            this.ForwardCommand = new DelegateCommand(Forward);
            this.RepeatCommand = new DelegateCommand(ToggleRepeat);
        }

        /// <summary>
        /// 時刻更新
        /// </summary>
        private void UpdatePlayerUI()
        {
            if (_audioPlayer == null)
            {
                return;
            }

            var currentAudio = _playlist.Current();
            if (_nowPlayMusic.Equals(currentAudio) != true)
            {
                NowPlayMusic = currentAudio;
            }

            if (SeekNow != (int)CurrentTime.TotalSeconds)
            {
                _audioPlayer.Seek(TimeSpan.FromSeconds(_seekNow));
                CurrentTime = TimeSpan.FromSeconds(_seekNow);
            }

            if (CurrentTime.TotalSeconds != _audioPlayer.CurrentTime.TotalSeconds)
            {
                CurrentTime = _audioPlayer.CurrentTime;
                SeekNow = (int)CurrentTime.TotalSeconds;
            }

            if (TotalTime.TotalSeconds != _audioPlayer.TotalTime.TotalSeconds)
            {
                TotalTime = _audioPlayer.TotalTime;
                SeekMax = (int)TotalTime.TotalSeconds;
            }

            if (_audioPlayer.PlaybackState == AudioPlaybackState.Playing)
            {
                IsPlay = true;
            }
            else
            {
                IsPlay = false;
            }

            if (_audioPlayer.PrePlaybackState == AudioPlaybackState.Playing &&
                _audioPlayer.PlaybackState == AudioPlaybackState.Stopped )
            {
                _uiDispatcher.Invoke(new Action(() =>
                {
                    PlayerStoppedEvent();
                }));
            }
         }

        /// <summary>
        /// 再開
        /// </summary>
        private void Replay()
        {
            if (_isPlay == false)
            {
                try
                {
                    _audioPlayer.Replay();
                }
                catch (System.IO.FileNotFoundException e)
                {
                    System.Windows.MessageBox.Show("Replay Error.");
                }
                catch (NullReferenceException e)
                {
                    System.Windows.MessageBox.Show("Replay Error.");
                }
            }
            else
            {
                try
                {
                    _audioPlayer.Pause();
                }
                catch (System.IO.FileNotFoundException e)
                {
                    System.Windows.MessageBox.Show("Pause Error.");
                }
            }
        }

        /// <summary>
        /// シーク操作
        /// </summary>
        private void Seek()
        {
            try
            {
                _audioPlayer.Seek(TimeSpan.FromSeconds(_seekNow));
            }
            catch (System.IO.FileNotFoundException e)
            {
                System.Windows.MessageBox.Show("Seek Error.");
            }
        }

        /// <summary>
        /// プレイヤー自動停止イベント
        /// </summary>
        private void PlayerStoppedEvent()
        {
            if (IsRepeat == true)
            {
                Replay();
            }
            else
            {
                Next();
            }
        }

        /// <summary>
        /// 次の曲へ
        /// </summary>
        private void Next()
        {
            try
            {
                _audioPlayer.Dispose();

                var nextAudio = _playlist.Next();
                _audioPlayer.Play(nextAudio);
            }
            catch (System.IO.FileNotFoundException e)
            {
                System.Windows.MessageBox.Show("Play Error.");
            }
            catch (System.ArgumentException e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 前の曲へ
        /// </summary>
        private void Forward()
        {
            try
            {
                _audioPlayer.Dispose();

                var forwardAudio = _playlist.Forward();
                _audioPlayer.Play(forwardAudio);
            }
            catch (System.IO.FileNotFoundException e)
            {
                System.Windows.MessageBox.Show("Play Error.");
            }
            catch (System.ArgumentException e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// リピート
        /// </summary>
        private void ToggleRepeat()
        {
            IsRepeat = (true == IsRepeat) ? (false) : (true);
            return;
        }
    }
}
