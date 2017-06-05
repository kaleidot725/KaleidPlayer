using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace kaleidot725.Model.Library
{
    public static class SongSerializer
    {
        public static void Serialize<AudioSerialzerData>(string FilePath, ObservableCollection<AudioSerialzerData> collection)
        {
            FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, collection);
            fs.Close();
        }

        public static void Deserialize<AudioSerialzerData>(string FilePath,ref ObservableCollection<AudioSerialzerData> collection)
        {
            FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            BinaryFormatter bf = new BinaryFormatter();
            collection = (ObservableCollection<AudioSerialzerData>)bf.Deserialize(fs);
            fs.Close();
        }

        public static IAudioDetail Convert(AudioSerialzerData serial)
        {
            IAudioDetail result;

            AudioType.Types type = AudioType.ParseAudioType(serial.FilePath);
            switch (type)
            {
                case AudioType.Types.Wave:
                    result = new AudioWaveDetail(serial.FilePath);
                    break;
                case AudioType.Types.Mp3:
                    result = new AudioMp3Detail(serial.FilePath);
                    break;
                case AudioType.Types.Flac:
                    result = new AudioFlacDetail(serial.FilePath);
                    break;
                default:
                    var e = new Exception("Not Support Audio File");
                    throw e;
            }

            result.Title = serial.Title;
            result.Artist = serial.Artist;
            result.Album = serial.Album;
            result.Date = serial.Date;
            result.TrackNo = serial.TrackNo;
            result.Genle = serial.Comment;
            result.Comment = serial.Comment;
            result.AlbumArtist = serial.AlbumArtist;
            result.Composer = serial.Composer;
            result.DiscNumber = serial.DiscNumber;
            result.FilePath = serial.FilePath;

            return result;
        }
    }
}
