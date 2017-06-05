using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace kaleidot725.Model.Library
{
    public class ArtistDetail : BindableBase
    {
        /// <summary>
        /// 名前
        /// </summary>
        private string _name;
        public string Name
        {
            get { return _name; }
            private set { SetProperty(ref _name, value); }
        }

        /// <summary>
        /// アルバムリスト
        /// </summary>
        private ObservableCollection<AlbumDetail> _albums;
        public ObservableCollection<AlbumDetail> Albums
        {
            get { return _albums; }
            private set { SetProperty(ref _albums, value); }
        }

        /// <summary>
        /// アルバムカウント
        /// </summary>
        public int AlbumCount
        {
            get { return _albums.Count; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        public ArtistDetail(IAudioDetail detail)
        {
            Name = detail.Artist;
            Albums = new ObservableCollection<AlbumDetail>();
        }

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="albumName"></param>
        public void AddAlbum(IAudioDetail detail)
        {
            AlbumDetail album = null;
            try
            {
                album = Albums.First(m => m.Name == detail.Album);
            }
            catch (InvalidOperationException e)
            {
                album = new AlbumDetail(detail);
                Albums.Add(album);
            }
            finally
            {
                album.AddAudio(detail);
            }
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void ClearAlbums()
        {
            Albums.Clear();
        }
    }
}
