using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace kaleidot725.Model
{
    public static class AudioParser
    {
        private const string ExtensionWave = ".wav";      
        private const string ExtensionFlac = ".flac";     
        private const string ExtensionMp3 = ".mp3";       

        static public AudioTypes GetTypes(string filePath)                 
        {
            if (filePath == null) {
                throw new ArgumentNullException();
            }

            if (!File.Exists(filePath)) {
                throw new FileNotFoundException();
            }

            var ext = System.IO.Path.GetExtension(filePath);
            switch (ext)
            {
                case ExtensionWave:
                    return AudioTypes.Wave;
                case ExtensionMp3:
                    return AudioTypes.Mp3;
                case ExtensionFlac:
                    return AudioTypes.Flac;
                default:
                    return AudioTypes.Unknown;
            }
        }

        static public IAudioDetail GetDetail(string filePath)
        {
            var type = AudioParser.GetTypes(filePath);
            switch (type)
            {
                case AudioTypes.Wave:
                    return WaveParser.GetDetail(filePath);
                case AudioTypes.Mp3:
                    return Mp3Parser.GetDetailID3v23(filePath);
                case AudioTypes.Flac:
                    return FlacParser.GetDetail(filePath);
                default:
                    throw new Exception("Not Support Audio File");
            }
        }

        static public BitmapImage GetArtwork(string filePath)
        {
            var type = AudioParser.GetTypes(filePath);
            switch (type)
            {
                case AudioTypes.Wave:
                    return null;
                case AudioTypes.Mp3:
                    return null;
                case AudioTypes.Flac:
                    return null;
                default:
                    throw new Exception("Not Support Audio File");
            }
        }
    }
}
