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

namespace kaleidot725.Model
{
    /// <summary>
    /// トラック検索モデル
    /// </summary>
    public class Searcher : BindableBase
    {
        /// <summary>
        /// 
        /// </summary>
        public Searcher()
        {
        }

        /// <summary>
        /// トラック検索
        /// </summary>
        public ObservableCollection<IAudioDetail> SearchFolder(IList<string> directorys)
        {
            var songs = new ObservableCollection<IAudioDetail>();

            foreach (var directory in directorys)
            {
                List<string> fileList = Directory.GetFiles(directory, "*", System.IO.SearchOption.AllDirectories).ToList();
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

            return songs;
        }

        /// <summary>
        /// トラック詳細取得
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private IAudioDetail GetAudioDetail(string filePath)
        {
            AudioTypes type = AudioFileParser.Parse(filePath);
            switch (type)
            {
                case AudioTypes.Wave:
                    return new AudioWaveDetail(filePath);
                case AudioTypes.Mp3:
                    return new AudioMp3Detail(filePath);
                case AudioTypes.Flac:
                    return new AudioFlacDetail(filePath);
                default:
                    var e = new Exception("Not Support Audio File");
                    throw e;
            }
        }
    }
}
