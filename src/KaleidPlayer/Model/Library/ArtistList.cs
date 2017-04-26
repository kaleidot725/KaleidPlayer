using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace kaleidot725.Model.Library
{
    /// <summary>
    /// アーティストリスト
    /// </summary>
    public class ArtistList :BindableBase
    {
        /// <summary>
        /// アーティスト・コレクション
        /// </summary>
        private ObservableCollection<ArtistDetail> _artists;
        public ObservableCollection<ArtistDetail> Artists
        {
            get { return _artists; }
            set { SetProperty(ref _artists, value); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ArtistList()
        {
            Artists = new ObservableCollection<ArtistDetail>();
        }

        /// <summary>
        /// リスト作成
        /// </summary>
        /// <param name="songs"></param>
        public void Create(List<AudioDetailBase> songs)
        {
            foreach (var song in songs)
            {
                ArtistDetail detail = null;

                string artist = song.Artist;
                try
                {
                    detail = Artists.First(m => m.Name == artist);
                }
                catch(InvalidOperationException e)
                {
                    detail = new ArtistDetail(song);
                    Artists.Add(detail);
                }
                finally
                {
                    detail.AddAlbum(song);
                }
            }
        }
    }
}
