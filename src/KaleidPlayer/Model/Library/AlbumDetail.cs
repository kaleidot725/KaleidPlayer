using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using kaleidot725.Model;

namespace kaleidot725.Model.Library
{
    class AlbumDetail : BindableBase
    {
        private string _name;
        private ObservableCollection<AudioDetailBase> _songs;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _name; }
            private set { SetProperty(ref _name, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<AudioDetailBase> Songs
        {
            get { return _songs; }
            private set { SetProperty(ref _songs, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SongCount
        {
            get { return _songs.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public AlbumDetail(AudioDetailBase detail)
        {
            Name = detail.Album;
            Songs = new ObservableCollection<AudioDetailBase>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="songDetail"></param>
        public void AddSong(AudioDetailBase songDetail)
        {
            Songs.Add(songDetail);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearSongs()
        {
            Songs.Clear();
        }
    }
}
