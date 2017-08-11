using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace kaleidot725.Model
{
    /// <summary>
    /// アルバム詳細
    /// </summary>
    public class Album : BindableBase, IAlbum 
    {
        /// <summary>
        /// 名前
        /// </summary>
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        /// <summary>
        /// 曲情報
        /// </summary>
        private ObservableCollection<IAudioDetail> tracks;
        public ObservableCollection<IAudioDetail> Tracks
        {
            get { return tracks; }
            set { SetProperty(ref tracks, value); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        public Album(string name)
        {
            Name = name;
            Tracks = new ObservableCollection<IAudioDetail>();
        }
    }
}
