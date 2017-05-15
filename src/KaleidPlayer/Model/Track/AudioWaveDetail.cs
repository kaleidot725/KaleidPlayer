using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;

namespace kaleidot725.Model
{
    public class AudioWaveDetail : AudioDetailBase
    {
        /// <summary>
        /// WAVE チャンク
        /// </summary>
        private const string WAVE_CHUNK_LIST = "LIST";

        /// <summary>
        /// WAVEタグ
        /// </summary>
        private const string WAVE_INFOID_TITLE = "INAM";
        private const string WAVE_INFOID_ARTIST = "IART";
        private const string WAVE_INFOID_ALBUM = "IPRD";
        private const string WAVE_INFOID_DATE = "ICRD";
        private const string WAVE_INFOID_COMMENT = "ICMT";
        private const string WAVE_INFOID_GENLE = "ISFT";
        private const string WAVE_INFOID_SOFTWARE = "IGNR";
                               
        /// <summary>
        /// リストヘッダ情報 オフセット
        /// </summary>
        private enum LIST_HEADER_OFFSET
        {
            CHUNKID = 0,
            SIZE = 4,
            TYPEID = 8
        }

        /// <summary>
        /// リストヘッダ情報 サイズ
        /// </summary>
        private enum LIST_HEADER_SIZE
        {
            CHUNKID = 4,
            SIZE = 4,
            TYPEID = 4
        }

        /// <summary>
        /// リスト フレーム情報 オフセット
        /// </summary>
        private enum LIST_FRAME_OFFSET
        {
            INFOID = 0,
            SIZE = 4,
            DATA = 8
        }

        /// <summary>
        /// リスト フレーム情報 サイズs
        /// </summary>
        private enum LIST_FRAME_SIZE
        {
            INFOID = 4,
            SIZE = 4
        }

        private string _software;

        public string Software
        {
            get { return _software; }
            set { SetProperty(ref _software, value); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public AudioWaveDetail(string filePath)
        {
            if (File.Exists(filePath) != true)
            {
                throw new FileNotFoundException();
            }

            FilePath = filePath;
        }

        /// <summary>
        /// ID3v1 パース
        /// </summary>
        public override void Parse()
        {
            ParseWave();
            return;
        }

        private void ParseWave()
        {
            // エンコーディング設定
            Encoding encoding = Encoding.Default;

            var reader = new WaveFileReader(FilePath);
            List<RiffChunk> exChunks = reader.ExtraChunks;
            RiffChunk chunkRiff = exChunks.Find(x =>
                (x.IdentifierAsString.Equals(WAVE_CHUNK_LIST, StringComparison.OrdinalIgnoreCase)));

            byte[] listData = reader.GetChunkData(chunkRiff);

            string typeId = encoding.GetString(listData, 0, (int)LIST_HEADER_SIZE.TYPEID);

            int index = (int)LIST_HEADER_SIZE.TYPEID;
            while (index < listData.Length)
            {
                string infoId = encoding.GetString(listData, index, (int)LIST_FRAME_SIZE.INFOID);

                byte[] infoSizeArray = new byte[(int)LIST_FRAME_SIZE.SIZE];
                Buffer.BlockCopy(listData, index + (int)LIST_FRAME_OFFSET.SIZE, infoSizeArray, 0, (int)LIST_FRAME_SIZE.SIZE);
                int infoSize = BitConverter.ToInt32(infoSizeArray, 0);
                string encStr = encoding.GetString(listData, index + (int)LIST_FRAME_OFFSET.DATA, infoSize); ;

                switch (infoId)
                {
                    case WAVE_INFOID_TITLE:
                        this.Title = encStr;
                        break;
                    case WAVE_INFOID_ARTIST:
                        this.Artist = encStr;
                        break;
                    case WAVE_INFOID_ALBUM:
                        this.Album = encStr;
                        break;
                    case WAVE_INFOID_DATE:
                        this.Date = encStr;
                        break;
                    case WAVE_INFOID_COMMENT:
                        this.Comment = encStr;
                        break;
                    case WAVE_INFOID_GENLE:
                        this.Genle = encStr;
                        break;
                    case WAVE_INFOID_SOFTWARE:
                        this.Software = encStr;
                        break;
                    default:
                        // Do Nothing
                        break;
                }

                // 次のデータへ
                index += (int)LIST_FRAME_SIZE.INFOID + (int)LIST_FRAME_SIZE.SIZE + infoSize;
            }

            reader.Dispose();
        }
    }
}
