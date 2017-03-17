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
    class TrackSearcher : BindableBase
    {
        private ObservableCollection<string> _folders;
        private ObservableCollection<AudioDetailBase> _songs;

        /// <summary>
        /// 検索フォルダ
        /// </summary>
        public ObservableCollection<string> Folders
        {
            get { return _folders; }
            set { SetProperty(ref _folders, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<AudioDetailBase> Songs
        {
            get { return _songs; }
            set { SetProperty(ref _songs, value); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TrackSearcher()
        {
            Folders = new ObservableCollection<string>();
            Songs = new ObservableCollection<AudioDetailBase>();
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
        /// 音楽ファイル検索
        /// </summary>
        public async void SearchMusicFiles()
        {
            var songs = new ObservableCollection<AudioDetailBase>();

            await Task.Run((Action)(() =>
            {
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
            }));

            Songs = songs;
        }

        /// <summary>
        /// オーディオ詳細取得
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
