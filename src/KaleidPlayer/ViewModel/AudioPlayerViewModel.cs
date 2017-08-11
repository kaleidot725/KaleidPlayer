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
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { SetProperty(ref fileName, value); }
        }

        /// <summary>
        /// 再生状態
        /// </summary>
        private bool isPlay;
        public bool IsPlay
        {
            get { return isPlay; }
            set { SetProperty(ref isPlay, value); }
        }

        /// <summary>
        /// リピート
        /// </summary>
        private bool isRepeat;
        public bool IsRepeat
        {
            get { return isRepeat; }
            set { SetProperty(ref isRepeat, value); }
        }

        /// <summary>
        /// シーク現在位置
        /// </summary>
        private int seekNow;
        public int SeekNow
        {
            get { return seekNow; }
            set { SetProperty(ref seekNow, value); }
        }

        /// <summary>
        /// シーク最大値
        /// </summary>
        private int seekMax;
        public int SeekMax
        {
            get { return seekMax; }
            set { SetProperty(ref seekMax, value); }
        }

        /// <summary>
        /// 音楽ファイル 再生位置
        /// </summary>
        private TimeSpan currentTime;
        public TimeSpan CurrentTime
        {
            get { return currentTime; }
            set { SetProperty(ref currentTime, value); }
        }

        /// <summary>
        /// 音声ファイル 合計時間
        /// </summary>
        private TimeSpan totalTime;
        public TimeSpan TotalTime
        {
            get { return totalTime; }
            set { SetProperty(ref totalTime, value); }
        }


        /// <summary>
        /// 再生している音楽ファイル情報
        /// </summary>
        private IAudioDetail nowPlayMusic;
        public IAudioDetail NowPlayMusic
        {
            get { return nowPlayMusic; }
            set { SetProperty(ref nowPlayMusic, value); }
        }

        private Player player;
        private Searcher searcher;
        private AudioLibrary library;
        private Playlist playlist;
        private Setting setting;

        private IObservable<long> timer;
        private IDisposable subscription;
        private Dispatcher dispatcher;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AudioPlayerViewModel()
        {
            // 再生状態
            isPlay = false;
            nowPlayMusic = new AudioNullDetail();
            timer = Observable.Interval(TimeSpan.FromMilliseconds(10));
            subscription = timer.Subscribe(i =>
            {
                UpdatePlayerUI();

            },
               ex => Console.WriteLine("OnError({0})", ex.Message),
               () => Console.WriteLine("Completed()"));

            // ライブラリ群
            player = SingletonModels.GetAudioPlayerInstance();
            searcher = SingletonModels.GetAudioSearcherInstance();
            library = SingletonModels.GetArtistListInstance();
            playlist = SingletonModels.GetAudioPlaylist();
            setting = SingletonModels.GetApplicationSetting();
            dispatcher = Dispatcher.CurrentDispatcher;

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
            if (player == null)
            {
                return;
            }

            var currentAudio = playlist.Current();
            if (nowPlayMusic.Equals(currentAudio) != true)
            {
                NowPlayMusic = currentAudio;
            }

            if (SeekNow != (int)CurrentTime.TotalSeconds)
            {
                player.Seek(TimeSpan.FromSeconds(seekNow));
                CurrentTime = TimeSpan.FromSeconds(seekNow);
            }

            if (CurrentTime.TotalSeconds != player.CurrentTime.TotalSeconds)
            {
                CurrentTime = player.CurrentTime;
                SeekNow = (int)CurrentTime.TotalSeconds;
            }

            if (TotalTime.TotalSeconds != player.TotalTime.TotalSeconds)
            {
                TotalTime = player.TotalTime;
                SeekMax = (int)TotalTime.TotalSeconds;
            }

            if (player.PlaybackState == PlaybackState.Playing)
            {
                IsPlay = true;
            }
            else
            {
                IsPlay = false;
            }

            if (player.PrePlaybackState == PlaybackState.Playing &&
                player.PlaybackState == PlaybackState.Stopped )
            {
                dispatcher.Invoke(new Action(() =>
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
            if (isPlay == false)
            {
                try
                {
                    player.Replay();
                }
                catch (System.IO.FileNotFoundException)
                {
                    System.Windows.MessageBox.Show("Replay Error.");
                }
                catch (NullReferenceException)
                {
                    System.Windows.MessageBox.Show("Replay Error.");
                }
            }
            else
            {
                try
                {
                    player.Pause();
                }
                catch (System.IO.FileNotFoundException)
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
                player.Seek(TimeSpan.FromSeconds(seekNow));
            }
            catch (System.IO.FileNotFoundException)
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
                player.Dispose();

                var nextAudio = playlist.Next();
                player.Play(nextAudio);
            }
            catch (System.IO.FileNotFoundException)
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
                player.Dispose();

                var forwardAudio = playlist.Forward();
                player.Play(forwardAudio);
            }
            catch (System.IO.FileNotFoundException)
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
