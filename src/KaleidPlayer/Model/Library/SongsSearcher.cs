using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using System.IO;
using System.Timers;
using System.Collections.ObjectModel;
using kaleidot725.Collection;

namespace kaleidot725.Model.Library
{
    /// <summary>
    /// トラック検索モデル
    /// </summary>
    public class AudioSearcher : BindableBase
    {

        /// <summary>
        /// 検索フォルダ
        /// </summary>
        private ObservableCollection<string> _folders;
        public ObservableCollection<string> Folders
        {
            get { return _folders; }
            set { SetProperty(ref _folders, value); }
        }

        /// <summary>
        /// トラック コレクション
        /// </summary>
        private ObservableCollection<AudioDetailBase> _audios;
        public ObservableCollection<AudioDetailBase> Audios
        {
            get { return _audios; }
            set { SetProperty(ref _audios, value); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AudioSearcher()
        {
            Folders = new ObservableCollection<string>();
            Audios = new ObservableCollection<AudioDetailBase>();
        }

        /// <summary>
        /// フォルダ追加
        /// </summary>
        /// <param name="folder"></param>
        public void AddFolder(string folder)
        {
            if (folder == null)
            {
                throw new ArgumentNullException();
            }

            Folders.Add(folder);
        }

        /// <summary>
        /// フォルダ削除
        /// </summary>
        public void ClearFolder()
        {
            Folders.Clear();
        }

        /// <summary>
        /// トラック検索
        /// </summary>
        public async void Search()
        {
            var songs = new ObservableCollection<AudioDetailBase>();

            //await Task.Run((Action)(() =>
           // {
                foreach (var folder in Folders)
                {
                    List<string> fileList = Directory.GetFiles(folder, "*", System.IO.SearchOption.AllDirectories).ToList();
                    foreach (var file in fileList)
                    {
                        var detail = GetAudioDetail(file);
                        if (detail == null)
                        {
                            continue;
                        }

                        detail.Parse();
                        songs.Add(detail);
                    }
                }
           // }));

            Audios = songs;
        }

        /// <summary>
        /// トラック詳細取得
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private AudioDetailBase GetAudioDetail(string filePath)
        {
            AudioType.Types type = AudioType.ParseAudioType(filePath);
            switch (type)
            {
                case AudioType.Types.Wave:
                    return new AudioWaveDetail(filePath);
                case AudioType.Types.Mp3:
                    return new AudioMp3Detail(filePath);
                case AudioType.Types.Flac:
                    return new AudioFlacDetail(filePath);
                default:
                    return null;
            }
        }
    }
}
