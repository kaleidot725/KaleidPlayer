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

namespace kaleidot725.ViewModel
{
    class AudioPlayerViewModel : BindableBase
    {
        public DelegateCommand PlayCommand { get; }
        public DelegateCommand ReplayCommand { get; }
        public DelegateCommand SeekCommand { get; }
        public ReactiveProperty<ObservableCollection<AudioDetailBase>> Songs { get; private set; }
        public ReactiveProperty<ObservableCollection<ArtistDetail>> Artists { get; private set; }
        public ReactiveProperty<ObservableCollection<AlbumDetail>> Albums { get; private set; }

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
        /// リスト選択した音楽ファイル情報
        /// </summary>
        private AudioDetailBase _selectedMusic;
        public AudioDetailBase SelectedMusic
        {
            get { return _selectedMusic; }
            set { SetProperty(ref _selectedMusic, value); }
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
        private TrackSearcher _songsSearcher;
        private ArtistSearcher _artistSearcher;
        private AlbumSearcher _albumSearcher;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AudioPlayerViewModel()
        {
            // 初期化
            _isPlay = false;
            _songsSearcher = new TrackSearcher();
            _artistSearcher = new ArtistSearcher();
            _albumSearcher = new AlbumSearcher();

            // コマンド作成
            this.PlayCommand = new DelegateCommand(Play);
            this.ReplayCommand = new DelegateCommand(Replay);
            this.SeekCommand = new DelegateCommand(Seek);

            Songs = _songsSearcher.ToReactivePropertyAsSynchronized(m => m.Songs).ToReactiveProperty();
            Artists = _artistSearcher.ToReactivePropertyAsSynchronized(m => m.Artists).ToReactiveProperty();
            Albums = _albumSearcher.ToReactivePropertyAsSynchronized(m => m.Albums).ToReactiveProperty();

            Songs.Subscribe(i => UpdateLibrary(),
                ex => Console.WriteLine("OnError({0})", ex.Message),
                () => Console.WriteLine("Completed()"));

            var source = Observable.Interval(TimeSpan.FromMilliseconds(10));
            var _timerSubscrition = source.Subscribe(i => UpdateTime(),
                ex => Console.WriteLine("OnError({0})", ex.Message),
                () => Console.WriteLine("Completed()"));

            _songsSearcher.AddFolder("D:\\MUSIC\\FlacMP3\\02_洋楽");
            _songsSearcher.SearchMusicFiles();
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
        }

        /// <summary>
        /// ライブラリ更新 
        /// </summary>
        private async void UpdateLibrary()
        {
            // FIXME:Model側に非同期処理を実装する
            _artistSearcher.CreateList(_songsSearcher.Songs.ToList());
            _albumSearcher.CreateList(_songsSearcher.Songs.ToList());
        }

        /// <summary>
        /// 再生
        /// </summary>
        private void Play()
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.Dispose();
            }

            try
            {
                _audioPlayer = new AudioPlayer();
                _audioPlayer.Play(SelectedMusic.FilePath);
                IsPlay = true;
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
        /// リプレイ
        /// </summary>
        private void Replay()
        {
            if (IsPlay == false)
            {
                try
                {
                    _audioPlayer.Replay();
                    IsPlay = true;
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
                    IsPlay = false;
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
    }
}
