using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace kaleidot725.Model
{
    public class Artist : BindableBase, IArtist
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
        /// アルバムリスト
        /// </summary>
        private ObservableCollection<IAlbum> albums;
        public ObservableCollection<IAlbum> Albums
        {
            get { return albums; }
            set { SetProperty(ref albums, value); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        public Artist(string name)
        {
            Name = name;
            Albums = new ObservableCollection<IAlbum>();
        }
    }
}
