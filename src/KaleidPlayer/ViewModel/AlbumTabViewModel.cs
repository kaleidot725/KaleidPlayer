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
        private Player player;
        private Playlist playlist;
        private Searcher searcher;
        private AudioLibrary library;

        public ReadOnlyReactiveProperty<ObservableCollection<IArtist>> Artists { get; private set; }
        public ReadOnlyReactiveProperty<ObservableCollection<IAlbum>> Albums { get; private set; }
        public ReadOnlyReactiveProperty<ObservableCollection<IAudioDetail>> Audios { get; private set; }

        /// <summary>
        ///  コマンド
        /// </summary>
        public DelegateCommand PlayCommand { get; }

        /// <summary>
        /// 選択したアルバム
        /// </summary>
        private Album seletedAlbum;
        public Album SeletedAlbum
        {
            get { return seletedAlbum; }
            set { SetProperty(ref seletedAlbum, value); }
        }

        /// <summary>
        /// 選択した音楽ファイル情報
        /// </summary>
        private IAudioDetail selectedAudio;
        public IAudioDetail SeletedAudio
        {
            get { return selectedAudio; }
            set { SetProperty(ref selectedAudio, value); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AlbumTabViewModel()
        {
            // 初期化
            player = SingletonModels.GetAudioPlayerInstance();
            searcher = SingletonModels.GetAudioSearcherInstance();
            library = SingletonModels.GetArtistListInstance();
            playlist = SingletonModels.GetAudioPlaylist();

            Artists = library.ToReactivePropertyAsSynchronized(m => m.Artists).ToReadOnlyReactiveProperty();
            Albums = library.ToReactivePropertyAsSynchronized(m => m.Albums).ToReadOnlyReactiveProperty();
            Audios = library.ToReactivePropertyAsSynchronized(m => m.Audios).ToReadOnlyReactiveProperty();

            PlayCommand = new DelegateCommand(Play);
        }

        /// <summary>
        /// 再生
        /// </summary>
        private void Play()
        {
            try
            {
                playlist.Create(SeletedAlbum.Tracks);
                playlist.SetPosition(SeletedAudio);

                player.Dispose();
                player.Play(playlist.Current());
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
    }
}
