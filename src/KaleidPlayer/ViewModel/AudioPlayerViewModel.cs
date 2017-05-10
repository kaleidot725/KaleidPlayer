﻿using System;
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

namespace kaleidot725.ViewModel
{
    class AudioPlayerViewModel : BindableBase
    {
        public DelegateCommand PlayCommand { get; }
        public DelegateCommand ReplayCommand { get; }
        public DelegateCommand SeekCommand { get; }
        public DelegateCommand NextCommand { get; }
        public DelegateCommand ForwardCommand { get; }

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
        private AudioDetailBase _nowPlayMusic;
        public AudioDetailBase NowPlayMusic
        {
            get { return _nowPlayMusic; }
            set { SetProperty(ref _nowPlayMusic, value); }
        }

        private AudioPlayer _audioPlayer;
        private AudioSearcher _songsSearcher;
        private ArtistList _artistList;
        private AlbumList _albumList;
        private AudioPlaylist _playlist;

        private IObservable<long> _timer;
        private IDisposable _subscription;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AudioPlayerViewModel()
        {
            // 再生状態
            _isPlay = false;
            _nowPlayMusic = new AudioNullDetail();
            _timer = Observable.Interval(TimeSpan.FromMilliseconds(10));
            _subscription = _timer.Subscribe(i => UpdateTime(),
               ex => Console.WriteLine("OnError({0})", ex.Message),
               () => Console.WriteLine("Completed()"));

            // ライブラリ群
            _audioPlayer = SingletonModels.GetAudioPlayerInstance();
            _songsSearcher = SingletonModels.GetAudioSearcherInstance();
            _artistList = SingletonModels.GetArtistListInstance();
            _albumList = SingletonModels.GetAlbumListInstance();
            _playlist = SingletonModels.GetAudioPlaylist();

            // コマンド作成
            this.ReplayCommand = new DelegateCommand(Replay);
            this.SeekCommand = new DelegateCommand(Seek);
            this.NextCommand = new DelegateCommand(Next);
            this.ForwardCommand = new DelegateCommand(Forward);

            // ライブラリ作成
            AsyncCreateLibrary();
        }

        /// <summary>
        /// 時刻更新
        /// </summary>
        private async void UpdateTime()
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

            // シーク値と現在時刻が違う場合
            if (SeekNow != (int)CurrentTime.TotalSeconds)
            {
                _audioPlayer.Seek(TimeSpan.FromSeconds(_seekNow));
                CurrentTime = TimeSpan.FromSeconds(_seekNow);
            }

            // 変化があるときのみ更新する
            if (CurrentTime.TotalSeconds != _audioPlayer.CurrentTime.TotalSeconds)
            {
                CurrentTime = _audioPlayer.CurrentTime;
                SeekNow = (int)CurrentTime.TotalSeconds;
            }

            // 変化があるときのみ更新する
            if (TotalTime.TotalSeconds != _audioPlayer.TotalTime.TotalSeconds)
            {
                TotalTime = _audioPlayer.TotalTime;
                SeekMax = (int)TotalTime.TotalSeconds;
            }

            // 変化があるときのみ更新する
            if (IsPlay != _audioPlayer.IsPlay)
            {
                IsPlay = _audioPlayer.IsPlay;
            }
        }

        /// <summary>
        /// ライブラリ作成
        /// </summary>
        private async void AsyncCreateLibrary()
        {
            await Task.Run(() =>
            {
                _songsSearcher.AddFolder("D:\\MUSIC\\FlacMP3\\02_洋楽");
                _songsSearcher.Search();
            });

            _artistList.Create(_songsSearcher.Audios.ToList());
            _albumList.Create(_songsSearcher.Audios.ToList());
        }

        /// <summary>
        /// リプレイ
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
    }
}
