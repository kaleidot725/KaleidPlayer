using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Commands;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using kaleidot725.Model;
using kaleidot725.Model.Library;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace kaleidot725.ViewModel
{
    class SongTabViewModel :BindableBase
    {
        private Player player;
        private Searcher searcher;
        private AudioLibrary library;
        private Playlist playlist;

        public ReadOnlyReactiveProperty<ObservableCollection<IArtist>> Artists { get; private set; }
        public ReadOnlyReactiveProperty<ObservableCollection<IAlbum>> Albums { get; private set; }
        public ReadOnlyReactiveProperty<ObservableCollection<IAudioDetail>> Audios { get; private set; }

        /// <summary>
        ///  コマンド
        /// </summary>
        public DelegateCommand PlayCommand { get; }

        /// <summary>
        /// リスト選択した音楽ファイル情報
        /// </summary>
        private IAudioDetail selectedAudio;
        public IAudioDetail SelectedAudio
        {
            get { return selectedAudio; }
            set { SetProperty(ref selectedAudio, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public SongTabViewModel()
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
                playlist.Create(Audios.Value);
                playlist.SetPosition(SelectedAudio);

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
