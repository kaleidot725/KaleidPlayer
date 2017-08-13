using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaleidot725.Model
{
    public static class AudioFileParser
    {
        private const string ExtensionWave = ".wav";      
        private const string ExtensionFlac = ".flac";     
        private const string ExtensionMp3 = ".mp3";       

        static public AudioTypes Parse(string filePath)                 
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
    }
}
