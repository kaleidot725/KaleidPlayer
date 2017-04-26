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
        private string _name;
        private ObservableCollection<AudioDetailBase> audios;

        /// <summary>
        /// 名前
        /// </summary>
        public string Name
        {
            get { return _name; }
            private set { SetProperty(ref _name, value); }
        }

        /// <summary>
        /// クアント
        /// </summary>
        public int Count
        {
            get { return audios.Count; }
        }

        /// <summary>
        /// 曲情報
        /// </summary>
        public ObservableCollection<AudioDetailBase> Audios
        {
            get { return audios; }
            private set { SetProperty(ref audios, value); }
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
