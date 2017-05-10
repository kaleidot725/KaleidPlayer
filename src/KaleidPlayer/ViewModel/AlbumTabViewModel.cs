using System;
using System.Collections.Generic;
using System.Linq;
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
    class AlbumTabViewModel : BindableBase
    {
        private AudioPlayer _audioPlayer;
        private AudioSearcher _songsSearcher;
        private ArtistList _aritstList;
        private AlbumList _albumList;
        private AudioPlaylist _playList;

        public ReactiveProperty<ObservableCollection<ArtistDetail>> Artists { get; private set; }
        public ReactiveProperty<ObservableCollection<AlbumDetail>> Albums { get; private set; }
        public ReactiveProperty<ObservableCollection<AudioDetailBase>> Audios { get; private set; }

        /// <summary>
        ///  コマンド
        /// </summary>
        public DelegateCommand PlayCommand { get; }

        /// <summary>
        /// 選択したアルバム
        /// </summary>
        private AlbumDetail _seletedAlbum;
        public AlbumDetail SeletedAlbum
        {
            get { return _seletedAlbum; }
            set { SetProperty(ref _seletedAlbum, value); }
        }

        /// <summary>
        /// 選択した音楽ファイル情報
        /// </summary>
        private AudioDetailBase _seletedAudio;
        public AudioDetailBase SeletedAudio
        {
            get { return _seletedAudio; }
            set { SetProperty(ref _seletedAudio, value); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AlbumTabViewModel()
        {
            // 初期化
            _audioPlayer = SingletonModels.GetAudioPlayerInstance();
            _songsSearcher = SingletonModels.GetAudioSearcherInstance();
            _aritstList = SingletonModels.GetArtistListInstance();
            _albumList = SingletonModels.GetAlbumListInstance();
            _playList = SingletonModels.GetAudioPlaylist();

            Artists = _aritstList.ToReactivePropertyAsSynchronized(m => m.Artists).ToReactiveProperty();
            Albums = _albumList.ToReactivePropertyAsSynchronized(m => m.Albums).ToReactiveProperty();
            Audios = _songsSearcher.ToReactivePropertyAsSynchronized(m => m.Audios).ToReactiveProperty();

            PlayCommand = new DelegateCommand(Play);
        }

        /// <summary>
        /// 再生
        /// </summary>
        private void Play()
        {
            try
            {
                _playList.CreatePlaylist(SeletedAlbum.Audios, SeletedAudio);
                var playAudio = _playList.Current();

                _audioPlayer.Dispose();
                _audioPlayer.Play(playAudio);
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
