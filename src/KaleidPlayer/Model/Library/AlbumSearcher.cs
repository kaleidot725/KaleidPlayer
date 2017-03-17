using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace kaleidot725.Model.Library
{
    class AlbumSearcher : BindableBase
    {
        private ObservableCollection<AlbumDetail> _albums;

        public ObservableCollection<AlbumDetail> Albums
        {
            get { return _albums; }
            set { SetProperty(ref _albums, value); }
        }

        public AlbumSearcher()
        {
            Albums = new ObservableCollection<AlbumDetail>();
        }

        public void CreateList(List<AudioDetailBase> songs)
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
                    detail.AddSong(song);
                }
            }
        } 
    }
}
