using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;
using Prism.Mvvm;
using System.Windows.Media.Imaging;

namespace kaleidot725.Model
{
    public class WaveParser
    {
        /// <summary>
        /// WAVE チャンク
        /// </summary>
        private const string WaveChunkList = "LIST";

        /// <summary>
        /// WAVEタグ
        /// </summary>
        private const string WaveInfoidTitle = "INAM";
        private const string WaveInfoidArtist = "IART";
        private const string WaveInfoidAlbum = "IPRD";
        private const string WaveInfoidDate = "ICRD";
        private const string WaveInfoidComment = "ICMT";
        private const string WaveInfoidGenle = "ISFT";
        private const string WaveInfoidSofware = "IGNR";

        /// <summary>
        /// リストヘッダ情報 オフセット
        /// </summary>
        private enum ListHeaderOffset
        {
            ChunkId = 0,
            Size = 4,
            TypeId = 8
        }

        /// <summary>
        /// リストヘッダ情報 サイズ
        /// </summary>
        private enum ListHeaderSize
        {
            ChunkId = 4,
            Size = 4,
            TypeId = 4
        }

        /// <summary>
        /// リスト フレーム情報 オフセット
        /// </summary>
        private enum ListFrameOffset
        {
            Infoid = 0,
            Size = 4,
            Data = 8
        }

        /// <summary>
        /// リスト フレーム情報 サイズ
        /// </summary>
        private enum ListFrameSize
        {
            Infoid = 4,
            Size = 4
        }

        static public IAudioDetail GetDetail(string filePath)
        {
            // エンコーディング設定
            Encoding encoding = Encoding.Default;

            IAudioDetail detail = new AudioDetail();
            detail.FilePath = filePath;

            using (var reader = new WaveFileReader(filePath))
            {
                List<RiffChunk> exChunks = reader.ExtraChunks;
                RiffChunk chunkRiff = exChunks.Find(x =>
                    (x.IdentifierAsString.Equals(WaveChunkList, StringComparison.OrdinalIgnoreCase)));

                byte[] listData = reader.GetChunkData(chunkRiff);

                string typeId = encoding.GetString(listData, 0, (int)ListHeaderSize.TypeId);

                int index = (int)ListHeaderSize.TypeId;
                while (index < listData.Length)
                {
                    string infoId = encoding.GetString(listData, index, (int)ListFrameSize.Infoid);

                    byte[] infoSizeArray = new byte[(int)ListFrameSize.Size];
                    Buffer.BlockCopy(listData, index + (int)ListFrameOffset.Size, infoSizeArray, 0, (int)ListFrameSize.Size);

                    int infoSize = BitConverter.ToInt32(infoSizeArray, 0);
                    string encStr = encoding.GetString(listData, index + (int)ListFrameOffset.Data, infoSize); ;

                    switch (infoId)
                    {
                        case WaveInfoidTitle:
                            detail.Title = encStr;
                            break;
                        case WaveInfoidArtist:
                            detail.Artist = encStr;
                            break;
                        case WaveInfoidAlbum:
                            detail.Album = encStr;
                            break;
                        case WaveInfoidDate:
                            detail.Date = encStr;
                            break;
                        case WaveInfoidComment:
                            detail.Comment = encStr;
                            break;
                        case WaveInfoidGenle:
                            detail.Genle = encStr;
                            break;
                        case WaveInfoidSofware:
                            // detail.Software = encStr;
                            break;
                        default:
                            // Do Nothing
                            break;
                    }

                    // 次のデータへ
                    index += (int)ListFrameSize.Infoid + (int)ListFrameSize.Size + infoSize;
                }
            }

            return detail;
        }

        public static BitmapImage GetArtwork(string filePath)
        {
            return null;
        }
    }
}
