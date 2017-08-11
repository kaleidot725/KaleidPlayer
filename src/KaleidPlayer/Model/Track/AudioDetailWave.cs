using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;
using Prism.Mvvm;

namespace kaleidot725.Model
{
    public class AudioWaveDetail : BindableBase, IAudioDetail
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
        /// リスト フレーム情報 サイズ
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

        private string _title;          // タイトル
        private string _artist;         // アーティスト
        private string _album;          // アルバム
        private string _date;           // 西暦
        private string _trackNo;        // トラックNo
        private string _genle;          // ジャンル
        private string _comment;        // コメント
        private string _albumArtist;    // アルバムアーティスト
        private string _composer;       // 作曲者
        private string _discnumber;     // ディスクナンバー
        private string _filePath;       // ファイルパス

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        /// <summary>
        /// アーティスト
        /// </summary>
        public string Artist
        {
            get { return _artist; }
            set { SetProperty(ref _artist, value); }
        }

        /// <summary>
        /// アルバム
        /// </summary>
        public string Album
        {
            get { return _album; }
            set { SetProperty(ref _album, value); }
        }

        /// <summary>
        /// 西暦
        /// </summary>
        public string Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        /// <summary>
        /// トラックNo
        /// </summary>
        public string TrackNo
        {
            get { return _trackNo; }
            set { SetProperty(ref _trackNo, value); }
        }

        /// <summary>
        /// ジャンル
        /// </summary>
        public string Genle
        {
            get { return _genle; }
            set { SetProperty(ref _genle, value); }
        }

        /// <summary>
        /// コメント
        /// </summary>
        public string Comment
        {
            get { return _comment; }
            set { SetProperty(ref _comment, value); }
        }

        /// <summary>
        /// アルバムアーティスト
        /// </summary>
        public string AlbumArtist
        {
            get { return _albumArtist; }
            set { SetProperty(ref _albumArtist, value); }
        }

        /// <summary>
        /// 作曲者
        /// </summary>
        public string Composer
        {
            get { return _composer; }
            set { SetProperty(ref _composer, value); }
        }

        /// <summary>
        /// ディスクNo
        /// </summary>
        public string DiscNumber
        {
            get { return _discnumber; }
            set { SetProperty(ref _discnumber, value); }
        }

        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set { SetProperty(ref _filePath, value); }
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
        public void Parse()
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
