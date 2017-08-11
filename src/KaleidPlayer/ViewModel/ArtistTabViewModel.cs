using kaleidot725.Model;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Collections.ObjectModel;

namespace kaleidot725.ViewModel
{
    public class ArtistTabViewModel : BindableBase
    {
        /// <summary>
        /// モデル
        /// </summary>
        private Player player;
        private Searcher searcher;
        private AudioLibrary library;
        private Playlist playlist;

        /// <summary>
        /// データ
        /// </summary>
        public ReadOnlyReactiveProperty<ObservableCollection<IArtist>> Artists { get; private set; }
        public ReadOnlyReactiveProperty<ObservableCollection<IAlbum>> Albums { get; private set; }
        public ReadOnlyReactiveProperty<ObservableCollection<IAudioDetail>> Audios { get; private set; }

        /// <summary>
        ///  コマンド
        /// </summary>
        public DelegateCommand PlayCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        private IArtist selectedArtist;
        public IArtist SelectedArtist
        {
            get { return selectedArtist; }
            set { SetProperty(ref selectedArtist, value); }
        }

        private IAlbum selectedAlbum;
        public IAlbum SeletedAlbum
        {
            get { return selectedAlbum; }
            set { SetProperty(ref selectedAlbum, value); }
        }

        /// <summary>
        /// リスト選択した音楽ファイル情報
        /// </summary>
        private IAudioDetail selectedAudio;
        public IAudioDetail SeletedAudio
        {
            get { return selectedAudio; }
            set { SetProperty(ref selectedAudio, value); }
        }

        public ArtistTabViewModel()
        {
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
