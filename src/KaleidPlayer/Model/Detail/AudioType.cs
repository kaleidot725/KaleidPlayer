using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaleidot725
{
    static class AudioType
    {
        public enum Type { Wave, Mp3, Flac, Unknown };
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static public Type GetFileTypes(string fileName)
        {
            var extension = System.IO.Path.GetExtension(fileName);
            switch (extension)
            {
                case "wave":
                    return Type.Wave;
                case "mp3":
                    return Type.Mp3;
                case "flac":
                    return Type.Flac;
                default:
                    return Type.Unknown;
            }
        }
    }
}
