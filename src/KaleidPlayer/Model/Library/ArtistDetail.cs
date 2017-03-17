using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace kaleidot725.Model.Library
{
    class ArtistDetail : BindableBase
    {
        private string _name;
        private ObservableCollection<string> _albums;

        /// <summary>
        /// 名前
        /// </summary>
        public string Name
        {
            get { return _name; }
            private set { SetProperty(ref _name, value); }
        }

        /// <summary>
        /// アルバムリスト
        /// </summary>
        public ObservableCollection<string> Albums
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
        public ArtistDetail(AudioDetailBase detail)
        {
            Name = detail.Artist;
            Albums = new ObservableCollection<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="albumName"></param>
        public void AddAlbum(AudioDetailBase detail)
        {
            try
            {
                Albums.First(name => name == detail.Album);
            }
            catch (InvalidOperationException e)
            {
                Albums.Add(detail.Album);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearAlbums()
        {
            Albums.Clear();
        }
    }
}
