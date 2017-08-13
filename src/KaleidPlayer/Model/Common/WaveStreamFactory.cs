using NAudio.Flac;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaleidot725.Model
{
    public class WaveStreamFactory
    {
        public static WaveStream CreateWaveStream(string filePath)
        {
            var type = AudioFileParser.Parse(filePath);
            switch (type)
            {
                case AudioTypes.Wave:
                    return new WaveFileReader(filePath);
                case AudioTypes.Mp3:
                    return new Mp3FileReader(filePath);
                case AudioTypes.Flac:
                    return new FlacReader(filePath);
                default:
                    throw new System.NotSupportedException();
            }
        }
    }
}
