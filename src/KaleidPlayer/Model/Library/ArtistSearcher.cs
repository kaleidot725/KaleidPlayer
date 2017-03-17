using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace kaleidot725.Model.Library
{
    class ArtistSearcher:BindableBase
    {
        private ObservableCollection<ArtistDetail> _artists;

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ArtistDetail> Artists
        {
            get { return _artists; }
            set { SetProperty(ref _artists, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ArtistSearcher()
        {
            Artists = new ObservableCollection<ArtistDetail>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="songs"></param>
        public void CreateList(List<AudioDetailBase> songs)
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
