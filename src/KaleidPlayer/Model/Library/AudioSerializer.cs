using System;
using System.Collections;
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
    public static class AudioSerializer
    {
        public static void Serialize(string FilePath, IList<AudioDetailSerializable> collection)
        {
            FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, collection);
            fs.Close();
        }

        public static IList<AudioDetailSerializable> Deserialize(string FilePath)
        {
            using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (IList<AudioDetailSerializable>)(ObservableCollection<AudioDetailSerializable>)bf.Deserialize(fs);
            }
        }

        public static IAudioDetail Convert(AudioDetailSerializable serial)
        {
            IAudioDetail result = AudioParser.GetDetail(serial.FilePath);
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
