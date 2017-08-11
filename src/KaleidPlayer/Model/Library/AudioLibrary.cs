using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace kaleidot725.Model
{
    /// <summary>
    /// アーティストリスト
    /// </summary>
    public class AudioLibrary :BindableBase
    {
        private ObservableCollection<IArtist> artists;
        public ObservableCollection<IArtist> Artists
        {
            get { return artists; }
            set { SetProperty(ref artists, value); }
        }

        private ObservableCollection<IAlbum> albums;
        public ObservableCollection<IAlbum> Albums
        {
            get { return albums; }
            set { SetProperty(ref albums, value); }
        }

        private ObservableCollection<IAudioDetail> audios;
        public ObservableCollection<IAudioDetail> Audios
        {
            get { return audios; }
            set { SetProperty(ref audios, value); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AudioLibrary()
        {
            artists = new ObservableCollection<IArtist>();
            albums = new ObservableCollection<IAlbum>();
            audios = new ObservableCollection<IAudioDetail>();
        }

        /// <summary>
        /// リスト作成
        /// </summary>
        /// <param name="audios"></param>
        public void Create(List<IAudioDetail> audios)
        {
            foreach (var audio in audios)
            {
                IArtist artist = null;
                IAlbum album = null;

                try
                {
                    artist = Artists.First(m => m.Name == audio.Artist);
                }
                catch (Exception)
                {
                    artist = new Artist(audio.Artist);
                    this.artists.Add(artist);
                }

                try
                {
                    album = artist.Albums.First(m => m.Name == audio.Album);
                }
                catch (Exception)
                {
                    album = new Album(audio.Album);
                    artist.Albums.Add(album);
                    this.albums.Add(album);
                }

                album.Tracks.Add(audio);
                this.audios.Add(audio);
            }
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Delete()
        {
            Artists.Clear();
            Albums.Clear();
            Audios.Clear();
        }
    }
}
