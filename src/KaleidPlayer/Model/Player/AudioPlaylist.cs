using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace kaleidot725.Model
{
    public class AudioPlaylist : BindableBase
    {
        private ObservableCollection<AudioDetailBase> _playlist;
        private int _index = 0;

        /// <summary>
        /// 
        /// </summary>
        public AudioPlaylist()
        {
            var nullAudio = new AudioNullDetail();
            _playlist = new ObservableCollection<AudioDetailBase>();
            _playlist.Add(nullAudio);
            _index = 0;
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="audio"></param>
        public void CreatePlaylist(ObservableCollection<AudioDetailBase> playlist, AudioDetailBase setAudio)
        {
            _index = playlist.IndexOf(setAudio);
            _playlist = new ObservableCollection<AudioDetailBase>(playlist);
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AudioDetailBase Current()
        {
           return _playlist[_index];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AudioDetailBase Next()
        {
            var index = _playlist.Count - 1;
            if (_index < index)
            {
                _index++;
            }
            else
            {
                _index = 0;
            }

            return _playlist[_index];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AudioDetailBase Forward()
        {
            if (0 < _index)
            {
                _index--;
            }
            else
            {
                var index = _playlist.Count - 1;
                _index = index;
            }

            return _playlist[_index];
        }
    }
}
