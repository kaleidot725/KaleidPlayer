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

namespace kaleidot725.Model
{
    /// <summary>
    /// トラック検索モデル
    /// </summary>
    public class Searcher : BindableBase
    {
        /// <summary>
        /// トラック検索
        /// </summary>
        public ObservableCollection<IAudioDetail> SearchFolder(IList<string> directries)
        {
            if (directries == null) {
                throw new ArgumentNullException();
            }

            if (directries.Count == 0)
            {
                return new ObservableCollection<IAudioDetail>(); ;
            }

            var songs = new ObservableCollection<IAudioDetail>();
            foreach (var directory in directries)
            {
                List<string> fileList = Directory.GetFiles(directory, "*", System.IO.SearchOption.AllDirectories).ToList();
                foreach (var file in fileList)
                {
                    try
                    {
                        var detail = AudioParser.GetDetail(file);
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
    }
}
