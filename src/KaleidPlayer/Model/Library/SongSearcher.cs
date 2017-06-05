﻿using System;
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
    public class SongSearcher : BindableBase
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
        private ObservableCollection<IAudioDetail> _song;
        public ObservableCollection<IAudioDetail> Song
        {
            get { return _song; }
            set { SetProperty(ref _song, value); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SongSearcher()
        {
            Folders = new ObservableCollection<string>();
            Song = new ObservableCollection<IAudioDetail>();
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
        /// 曲削除
        /// </summary>
        public void ClearSong()
        {
            Song.Clear();
        }

        /// <summary>
        /// トラック検索
        /// </summary>
        public void SearchFolder()
        {
            var songs = new ObservableCollection<IAudioDetail>();

            foreach (var folder in Folders)
            {
                List<string> fileList = Directory.GetFiles(folder, "*", System.IO.SearchOption.AllDirectories).ToList();
                foreach (var file in fileList)
                {
                    try
                    {
                        var detail = GetAudioDetail(file);
                        detail.Parse();
                        songs.Add(detail);
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine(e.Message);
                    }
                }
            }

            Song = songs;
        }

        /// <summary>
        /// トラック詳細取得
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private IAudioDetail GetAudioDetail(string filePath)
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
                    var e = new Exception("Not Support Audio File");
                    throw e;
            }
        }
    }
}