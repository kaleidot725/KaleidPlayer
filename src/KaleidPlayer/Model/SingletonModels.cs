using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kaleidot725.Model.Library;
using System.Collections.ObjectModel;

namespace kaleidot725.Model
{
    public static class SingletonModels
    {
        static AudioPlayer _audioPlayer;  
        static SongSearcher _songSearcher;
        static ArtistList _artistList;    
        static AlbumList _albumList;      
        static AudioPlaylist _playlist;
        static ApplicationSetting _setting;

        static SingletonModels()
        {
            _audioPlayer = new AudioPlayer();
            _songSearcher = new SongSearcher();
            _artistList = new ArtistList();
            _albumList = new AlbumList();
            _playlist = new AudioPlaylist();
            _setting = new ApplicationSetting();

            var collection = new ObservableCollection<AudioSerialzerData>();
            SongSerializer.Deserialize(System.IO.Directory.GetCurrentDirectory() + "\\meta", ref collection);
            foreach (var item in collection)
            {
                var conv = SongSerializer.Convert(item);
                _songSearcher.Song.Add(item);
            }

            _artistList.Create(_songSearcher.Song.ToList());
            _albumList.Create(_songSearcher.Song.ToList());
        }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        /// <returns></returns>
        static public AudioPlayer GetAudioPlayerInstance()
        {
            return _audioPlayer;
        }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        /// <returns></returns>
        static public SongSearcher GetAudioSearcherInstance()
        {
            return _songSearcher;
        }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        /// <returns></returns>
        static public ArtistList GetArtistListInstance()
        {
            return _artistList;
        }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        /// <returns></returns>
        static public AlbumList GetAlbumListInstance()
        {
            return _albumList;
        }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        /// <returns></returns>
        static public AudioPlaylist GetAudioPlaylist()
        {
            return _playlist;
        }

        /// <summary>
        /// アプリケーション設定
        /// </summary>
        /// <returns></returns>
        static public ApplicationSetting GetApplicationSetting()
        {
            return _setting;
        }
    }
}
