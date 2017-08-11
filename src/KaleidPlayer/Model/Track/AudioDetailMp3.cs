using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NAudio.Wave;
using Prism.Mvvm;

namespace kaleidot725.Model
{
    public class AudioMp3Detail : BindableBase, IAudioDetail
    {
        #region 定数定義

        /// <summary>
        /// Syncsafe Interger形式 マスク
        /// SyncsafeInterger形式のデータは8ビット中の
        /// 下位7ビットをデータ領域とする。8ビット目には必ず0が入る。
        /// </summary>
        private const byte SYNCSAFE_INTERGER_MASK = 0x7F;

        /// <summary>
        /// ID3V1 タグ文字列
        /// </summary>
        private const string ID3V1_TAG = "TAG";

        /// <summary>
        /// ID3V2 タグ文字列
        /// </summary>
        private const string ID3V2_TAG = "ID3";

        /// <summary>
        /// ID3V2.2 フレームID
        /// </summary>
        private const string ID3V22_FRAME_TITLE = "TT2";
        private const string ID3V22_FRAME_ARTIST = "IP1";
        private const string ID3V22_FRAME_ALBUM = "TAL";
        private const string ID3V22_FRAME_DATE = "TYE";
        private const string ID3V22_FRAME_COMMENT = "COM";
        private const string ID3V22_FRAME_TRACKNO = "TRK";
        private const string ID3V22_FRAME_GENLE = "TCO";

        /// <summary>
        /// ID3V2.3 フレームID
        /// </summary>
        private const string ID3V23_FRAME_TITLE = "TIT2";
        private const string ID3V23_FRAME_ARTIST = "TPE1";
        private const string ID3V23_FRAME_ALBUM = "TALB";
        private const string ID3V23_FRAME_DATE = "TYER";
        private const string ID3V23_FRAME_COMMENT = "COMM";
        private const string ID3V23_FRAME_TRACKNO = "TRCK";
        private const string ID3V23_FRAME_GENLE = "TCON";

        /// <summary>
        /// ID3V1.1 オフセット
        /// </summary>
        private enum ID3V1_OFFSET
        {
            TAG = 0,
            TITLE = 3,
            ARTIST = 33,
            ALBUM = 63,
            DATE = 93,
            COMMENT = 97,
            BLANK = 125,
            TRACKNO = 126,
            GENLE = 127,
        }

        /// <summary>
        /// ID3V1.1 サイズ
        /// </summary>
        private enum ID3V1_LENGTH
        {
            TAG = 3,
            TITLE = 30,
            ARTIST = 30,
            ALBUM = 30,
            DATE = 4,
            COMMENT = 28,
            BLANK = 1,
            TRACKNO = 1,
            GENLE = 1,
        }

        /// <summary>
        /// ID3V2 ヘッダー オフセット 
        /// </summary>
        private enum ID3V2_HEADER_OFFSET
        {
            ID = 0,
            VERSION = 3,
            FLAG = 5,
            SIZE = 6
        }

        /// <summary>
        /// ID3V2 ヘッダー サイズ
        /// </summary>
        private enum ID3V2_HEADER_SIZE
        {
            ID = 3,
            VERSION = 2,
            FLAG = 1,
            SIZE = 4,
            ALL = 10
        }

        /// <summary>
        /// ID3V2 拡張ヘッダ オフセット
        /// </summary>
        private enum ID3V2_EXHEADER_OFFSET
        {
            SIZE = 0,
            FLAG = 4,
        }

        /// <summary>
        /// ID3V2 拡張ヘッダ サイズ
        /// </summary>
        private enum ID3V2_EX_HEADER_SIZE
        {
            SIZE = 4,
            FLAG = 2,
            ALL = 6
        }

        /// <summary>
        /// ID3V2 フレーム オフセット
        /// </summary>
        private enum ID3V2_FHEADER_OFFSET
        {
            ID = 0,
            SIZE = 4,
            FLAG = 8
        }

        /// <summary>
        /// ID3V2 フレーム サイズ
        /// </summary>
        private enum ID3V2_FHEADER_SIZE
        {
            ID = 4,
            SIZE = 4,
            FLAG = 2,
            ALL = 10
        }

        /// <summary>
        /// ID3V2 拡張フレーム オフセット
        /// </summary>
        private enum ID3V2_EXTFRAME_OFFSET
        {
            ID = 0,
            SIZE = 3,
        }

        /// <summary>
        /// ID3V2 拡張フレーム サイズ 
        /// </summary>
        private enum ID3V2_EXTFRAME_SIZE
        {
            ID = 3,
            SIZE = 3,
            ALL = 6
        }

        /// <summary>
        /// 
        /// </summary>
        private enum ID3V2_FPAYLOAD_OFFSET
        {
            ENCODING = 0,
            DATA = 1,
        }

        /// <summary>
        /// 
        /// </summary>
        private enum ID3V2_FPAYLOAD_SIZE
        {
            ENCODING = 1,
        }

        /// <summary>
        /// エンコーディング タイプ一覧
        /// </summary>
        private enum ID3V2_ENCODING_TYPES
        {
            ISO_8859_1 = 0x00,      // ISO-8859-1
            UTF_16 = 0x01,          // UTF-16 BOMあり
            UTF_16BE = 0x02,        // UTF-16BE / BOMなし
            UTF_8 = 0x03            // UTF-8
        }
        #endregion

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
        public AudioMp3Detail(string filePath)
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
            ParseID3v23();
            return;
        }

        /// <summary>
        /// ID3v1 パース
        /// </summary>
        private void ParseID3v1()
        {
            Mp3FileReader reader = new Mp3FileReader(FilePath);
            byte[] tagV1 = reader.Id3v1Tag;

            string Tag = Encoding.Default.GetString(tagV1, (int)ID3V1_OFFSET.TAG, (int)ID3V1_LENGTH.TAG);
            if (Tag.Equals("TAG"))
            {
                this.Title = Encoding.Default.GetString(tagV1, (int)ID3V1_OFFSET.TITLE, (int)ID3V1_LENGTH.TITLE);
                this.Artist = Encoding.Default.GetString(tagV1, (int)ID3V1_OFFSET.ARTIST, (int)ID3V1_LENGTH.ARTIST);
                this.Album = Encoding.Default.GetString(tagV1, (int)ID3V1_OFFSET.ALBUM, (int)ID3V1_LENGTH.ALBUM);
                this.Date = Encoding.Default.GetString(tagV1, (int)ID3V1_OFFSET.DATE, (int)ID3V1_LENGTH.DATE);
                this.Comment = Encoding.Default.GetString(tagV1, (int)ID3V1_OFFSET.COMMENT, (int)ID3V1_LENGTH.COMMENT);
                this.TrackNo = Encoding.Default.GetString(tagV1, (int)ID3V1_OFFSET.TRACKNO, (int)ID3V1_LENGTH.TRACKNO);
                this.Genle = Encoding.Default.GetString(tagV1, (int)ID3V1_OFFSET.GENLE, (int)ID3V1_LENGTH.GENLE);
            }

            reader.Dispose();
            return;
        }

        /// <summary>
        /// ID3v2 パース
        /// </summary>
        private void ParseID3v23()
        {
            // タグデータ取得
            Mp3FileReader reader = new Mp3FileReader(FilePath);

            // NULLチェック
            if (reader.Id3v2Tag == null)
            {
                return;
            }

            byte[] tagV2 = reader.Id3v2Tag.RawData;
            Encoding encoder = Encoding.GetEncoding("iso-8859-1");

            // ヘッダ部取得
            string id = encoder.GetString(tagV2, (int)ID3V2_HEADER_OFFSET.ID, (int)ID3V2_HEADER_SIZE.ID);
            string ver = encoder.GetString(tagV2, (int)ID3V2_HEADER_OFFSET.VERSION, (int)ID3V2_HEADER_SIZE.VERSION);
            byte flag = tagV2[(int)ID3V2_HEADER_OFFSET.FLAG];

            // Syncsafe Integerを処理する
            int tagSize = ((tagV2[(int)ID3V2_HEADER_OFFSET.SIZE] & SYNCSAFE_INTERGER_MASK) << 21)
                        + ((tagV2[(int)ID3V2_HEADER_OFFSET.SIZE + 1] & SYNCSAFE_INTERGER_MASK) << 14)
                        + ((tagV2[(int)ID3V2_HEADER_OFFSET.SIZE + 2] & SYNCSAFE_INTERGER_MASK) << 7)
                        + (tagV2[(int)ID3V2_HEADER_OFFSET.SIZE + 3] & SYNCSAFE_INTERGER_MASK);


            // フレーム処理
            int fHeaderIndex = (int)ID3V2_HEADER_SIZE.ALL;
            while (fHeaderIndex < tagSize)
            {
                if (tagV2[fHeaderIndex] == '\0')
                {
                    break;
                }

                // ヘッダ 
                string frameId = encoder.GetString(tagV2, fHeaderIndex + (int)ID3V2_FHEADER_OFFSET.ID, (int)ID3V2_FHEADER_SIZE.ID);

                Int32 frameSize;
                {
                    byte[] fSizeArray = new byte[(int)ID3V2_FHEADER_SIZE.SIZE];
                    Buffer.BlockCopy(tagV2, fHeaderIndex + (int)ID3V2_FHEADER_OFFSET.SIZE, fSizeArray, 0, (int)ID3V2_FHEADER_SIZE.SIZE);

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(fSizeArray);
                    }

                    frameSize = BitConverter.ToInt32(fSizeArray, 0);
                }

                byte highFlag = tagV2[fHeaderIndex + (int)ID3V2_FHEADER_OFFSET.FLAG];
                byte lowFlag = tagV2[fHeaderIndex + (int)ID3V2_FHEADER_OFFSET.FLAG + 1];

                // エンコード形式
                byte[] b = new byte[frameSize - 1];
                Buffer.BlockCopy(tagV2, fHeaderIndex + (int)ID3V2_FHEADER_SIZE.ALL + 1, b, 0, frameSize - 1);

                byte fEncType = tagV2[fHeaderIndex + (int)ID3V2_FHEADER_SIZE.ALL];
                string encStr = EncodingString(fEncType, b);

                // ペイロード
                switch (frameId)
                {
                    case ID3V23_FRAME_TITLE:
                        this.Title = encStr;
                        break;
                    case ID3V23_FRAME_ARTIST:
                        this.Artist = encStr;
                        break;
                    case ID3V23_FRAME_ALBUM:
                        this.Album = encStr;
                        break;
                    case ID3V23_FRAME_DATE:
                        this.Date = encStr;
                        break;
                    case ID3V23_FRAME_COMMENT:
                        this.Comment = encStr;
                        break;
                    case ID3V23_FRAME_TRACKNO:
                        this.TrackNo = GetTrackNoFromTRCKFormat(encStr);
                        break;
                    case ID3V23_FRAME_GENLE:
                        this.Genle = encStr;
                        break;
                    default:
                        break;
                }

                fHeaderIndex += (int)ID3V2_FHEADER_SIZE.ALL + frameSize;
            }
        }

        /// <summary>
        /// [トラックの番号/セット中]からトラック番号を取得する
        /// </summary>
        /// <returns></returns>
        static string GetTrackNoFromTRCKFormat(string trackStr)
        {
            var split = trackStr.Split('/');
            return split[0];
        }

        /// <summary>
        /// 文字列をエンコードする
        /// </summary>
        /// <param name="type"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        static string EncodingString(byte type, byte[] input)
        {
            var encoding = GetEncodingTypes(type);
            var preamble = encoding.GetPreamble();
            var bytes = new byte[input.Length];

            // BOMを探す
            if (preamble.Length < input.Length)
            {
                int count = 0;

                foreach (var c in preamble)
                {
                    if (c != input[count])
                    {
                        Buffer.BlockCopy(input, 0, bytes, 0, input.Length);
                        break;
                    }

                    count++;
                    if (count == preamble.Length)
                    {
                        Buffer.BlockCopy(input, preamble.Length, bytes, 0, input.Length - preamble.Length);
                        break;
                    }
                }
            }

            var str = encoding.GetString(bytes);
            str = str.Replace("\0", "");
            return str;
        }

        /// <summary>
        /// エンコーディングオブジェクト取得
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static Encoding GetEncodingTypes(byte type)
        {
            switch (type)
            {
                case (int)ID3V2_ENCODING_TYPES.ISO_8859_1:
                    return Encoding.GetEncoding("iso-8859-1");
                case (int)ID3V2_ENCODING_TYPES.UTF_16:
                    return new UnicodeEncoding(false, true);
                case (int)ID3V2_ENCODING_TYPES.UTF_16BE:
                    return Encoding.BigEndianUnicode;
                case (int)ID3V2_ENCODING_TYPES.UTF_8:
                    return Encoding.UTF8;
                default:
                    return Encoding.Default;
            }
        }
    }
}
