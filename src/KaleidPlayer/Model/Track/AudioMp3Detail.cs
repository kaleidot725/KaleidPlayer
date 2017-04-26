using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NAudio.Wave;

namespace kaleidot725.Model
{
    public class AudioMp3Detail : AudioDetailBase
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
        public override void Parse()
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
            byte[] tagV2 = reader.Id3v2Tag.RawData;
            Encoding encoder = Encoding.Default;

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
                byte fEncType = tagV2[fHeaderIndex + (int)ID3V2_FHEADER_SIZE.ALL];
                Encoding fEncoding = GetEncodingTypes(fEncType);

                // ペイロード
                switch (frameId)
                {
                    case ID3V23_FRAME_TITLE:
                        this.Title = fEncoding.GetString(tagV2, fHeaderIndex + (int)ID3V2_FHEADER_SIZE.ALL + 1, frameSize - 1);
                        break;
                    case ID3V23_FRAME_ARTIST:
                        this.Artist = fEncoding.GetString(tagV2, fHeaderIndex + (int)ID3V2_FHEADER_SIZE.ALL + 1, frameSize - 1);
                        break;
                    case ID3V23_FRAME_ALBUM:
                        this.Album = fEncoding.GetString(tagV2, fHeaderIndex + (int)ID3V2_FHEADER_SIZE.ALL + 1, frameSize - 1);
                        break;
                    case ID3V23_FRAME_DATE:
                        this.Date = fEncoding.GetString(tagV2, fHeaderIndex + (int)ID3V2_FHEADER_SIZE.ALL + 1, frameSize - 1);
                        break;
                    case ID3V23_FRAME_COMMENT:
                        this.Comment = fEncoding.GetString(tagV2, fHeaderIndex + (int)ID3V2_FHEADER_SIZE.ALL + 1, frameSize - 1);
                        break;
                    case ID3V23_FRAME_TRACKNO:
                        this.TrackNo = fEncoding.GetString(tagV2, fHeaderIndex + (int)ID3V2_FHEADER_SIZE.ALL + 1, frameSize - 1);
                        break;
                    case ID3V23_FRAME_GENLE:
                        this.Genle = fEncoding.GetString(tagV2, fHeaderIndex + (int)ID3V2_FHEADER_SIZE.ALL + 1, frameSize - 1);
                        break;
                    default:
                        break;
                }

                fHeaderIndex += (int)ID3V2_FHEADER_SIZE.ALL + frameSize;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static Encoding GetEncodingTypes(byte type)
        {
            switch (type)
            {
                case (int)ID3V2_ENCODING_TYPES.ISO_8859_1:
                    return Encoding.GetEncoding(0);
                case (int)ID3V2_ENCODING_TYPES.UTF_16:
                    return Encoding.BigEndianUnicode;
                default:
                    return Encoding.Default;
            }
        }
    }
}
