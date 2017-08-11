using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaleidot725.Model
{
    public class AudioDetailFactory
    {
        public static IAudioDetail CreateAudioDetail(string filePath)
        {
            var type = AudioFileParser.Parse(filePath);
            switch (type)
            {
                case AudioTypes.Wave:
                    return new AudioWaveDetail(filePath);
                case AudioTypes.Mp3:
                    return new AudioMp3Detail(filePath);
                case AudioTypes.Flac:
                    return new AudioFlacDetail(filePath);
                default:
                    throw new Exception("Not Support Audio File");
            }
        }
    }
}
