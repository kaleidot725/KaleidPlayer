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
    /// アルバム・リスト
    /// </summary>
    public class AlbumList : BindableBase
    {
        /// <summary>
        /// アルバム・コレクション
        /// </summary>
        private ObservableCollection<AlbumDetail> _albums;
        public ObservableCollection<AlbumDetail> Albums
        {
            get { return _albums; }
            set { SetProperty(ref _albums, value); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AlbumList()
        {
            Albums = new ObservableCollection<AlbumDetail>();
        }

        /// <summary>
        /// リスト作成
        /// </summary>
        /// <param name="songs"></param>
        public void Create(List<IAudioDetail> songs)
        {
            foreach (var song in songs)
            {
                AlbumDetail detail = null;

                string album = song.Album;
                try
                {
                    detail = Albums.First(m => m.Name == album);
                }
                catch (InvalidOperationException e)
                {
                    detail = new AlbumDetail(song);
                    Albums.Add(detail);
                }
                finally
                {
                    detail.AddAudio(song);
                }
            }
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            Albums.Clear();
        }
    }
}
