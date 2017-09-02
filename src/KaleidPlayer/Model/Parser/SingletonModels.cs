using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kaleidot725.Model.Library;
using System.Collections.ObjectModel;
using System.Collections;

namespace kaleidot725.Model
{
    public static class SingletonModels
    {
        static Player player;  
        static Searcher searcher;
        static AudioLibrary library;    
        static Playlist playlist;
        static Setting setting;

        static SingletonModels()
        {
            player = new Player();
            searcher = new Searcher();
            library = new AudioLibrary();
            playlist = new Playlist();
            setting = new Setting();

            try
            {
                var collection = AudioSerializer.Deserialize(System.IO.Directory.GetCurrentDirectory() + "\\meta");
                var convCollection = new List<IAudioDetail>();
                foreach (var i in collection)
                {
                    var conv = AudioSerializer.Convert(i);
                    convCollection.Add(conv);
                }

                library.Create(convCollection);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        /// <returns></returns>
        static public Player GetAudioPlayerInstance()
        {
            return player;
        }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        /// <returns></returns>
        static public Searcher GetAudioSearcherInstance()
        {
            return searcher;
        }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        /// <returns></returns>
        static public AudioLibrary GetArtistListInstance()
        {
            return library;
        }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        /// <returns></returns>
        static public Playlist GetAudioPlaylist()
        {
            return playlist;
        }

        /// <summary>
        /// アプリケーション設定
        /// </summary>
        /// <returns></returns>
        static public Setting GetApplicationSetting()
        {
            return setting;
        }
    }
}
