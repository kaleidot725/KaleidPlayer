using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kaleidot725.Model.Library;

namespace kaleidot725.Model
{
    public static class SingletonModels
    {
        static AudioPlayer _audioPlayer = new AudioPlayer();
        static AudioSearcher _audioSearcher = new AudioSearcher();
        static ArtistList _artistList = new ArtistList();
        static AlbumList _albumList = new AlbumList();

        static public AudioPlayer GetAudioPlayerInstance()
        {
            return _audioPlayer;
        }

        static public AudioSearcher GetAudioSearcherInstance()
        {
            return _audioSearcher;
        }

        static public ArtistList GetArtistListInstance()
        {
            return _artistList;
        }

        static public AlbumList GetAlbumListInstance()
        {
            return _albumList;
        }
    }
}
