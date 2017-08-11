using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace kaleidot725.Model
{
    public class Playlist : BindableBase
    {
        private ObservableCollection<IAudioDetail> audios;
        private int position = 0;

        /// <summary>
        /// 
        /// </summary>
        public Playlist()
        {
            position = 0;
            audios = new ObservableCollection<IAudioDetail>();
            audios.Add(new AudioNullDetail());
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="audio"></param>
        public void Create(ObservableCollection<IAudioDetail> list)
        {
            position = 0;
            this.audios = new ObservableCollection<IAudioDetail>(list);
            return;
        }

          /// <summary>
        /// 
        /// </summary>
        public void Delete()
        {
            position = 0;
            audios = null;
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IAudioDetail Current()
        {
           return audios[position];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="audio"></param>
        public void SetPosition(IAudioDetail audio)
        {
            position = audios.IndexOf(audio);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IAudioDetail Next()
        {
            var index = audios.Count - 1;
            if (this.position < index)
            {
                this.position++;
            }
            else
            {
                this.position = 0;
            }

            return audios[this.position];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IAudioDetail Forward()
        {
            if (0 < position)
            {
                position--;
            }
            else
            {
                var index = audios.Count - 1;
                this.position = index;
            }

            return audios[position];
        }
    }
}
