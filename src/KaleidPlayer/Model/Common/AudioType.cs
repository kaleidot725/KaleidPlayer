using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaleidot725.Model
{
    /// <summary>
    /// オーディオタイプ
    /// </summary>
    public static class AudioType
    {
        private const string ExtensionWave = ".wav";      // WAVE拡張子
        private const string ExtensionFlac = ".flac";     // FLAC拡張子
        private const string ExtensionMp3 = ".mp3";       // MP3拡張子

        public enum Types { Wave, Mp3, Flac, Unknown };   // ファイルタイプ

        /// <summary>
        /// オーディオ種別 解析
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>オーディオ種別</returns>
        static public Types ParseAudioType(string filePath)
        {
            var extension = System.IO.Path.GetExtension(filePath);
            switch (extension)
            {
                case ExtensionWave:
                    return Types.Wave;
                case ExtensionMp3:
                    return Types.Mp3;
                case ExtensionFlac:
                    return Types.Flac;
                default:
                    return Types.Unknown;
            }
        }
    }
}
