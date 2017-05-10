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
    /// <summary>
    /// アルバム詳細
    /// </summary>
    public class AlbumDetail : BindableBase
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
        /// カウント
        /// </summary>
        public int Count
        {
            get { return _auidos.Count; }
        }

        /// <summary>
        /// 曲情報
        /// </summary>
        private ObservableCollection<AudioDetailBase> _auidos;
        public ObservableCollection<AudioDetailBase> Audios
        {
            get { return _auidos; }
            private set { SetProperty(ref _auidos, value); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        public AlbumDetail(AudioDetailBase detail)
        {
            Name = detail.Album;
            Audios = new ObservableCollection<AudioDetailBase>();
        }

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="audio"></param>
        public void AddAudio(AudioDetailBase audio)
        {
            Audios.Add(audio);
        }

        /// <summary>
        /// 削除
        /// </summary>
        public void ClearAudio()
        {
            Audios.Clear();
        }
    }
}
