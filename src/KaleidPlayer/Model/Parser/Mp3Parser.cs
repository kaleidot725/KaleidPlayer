using NAudio.Wave;
using System;
using System.Text;
using System.Windows.Media.Imaging;

namespace kaleidot725.Model
{
    public class Mp3Parser
    {
        /// <summary>
        /// Syncsafe Interger形式 マスク
        /// SyncsafeInterger形式のデータは8ビット中の
        /// 下位7ビットをデータ領域とする。8ビット目には必ず0が入る。
        /// </summary>
        private const byte SyncsafeIntegerMask = 0x7F;

        /// <summary>
        /// ID3V1 タグ文字列
        /// </summary>
        private const string ID3v1Tag = "TAG";

        /// <summary>
        /// ID3V2 タグ文字列
        /// </summary>
        private const string ID3v2Tag = "ID3";

        /// <summary>
        /// ID3V2.2 フレームID
        /// </summary>
        private const string ID3v22FrameTitle = "TT2";
        private const string ID3v22FrameArtist = "IP1";
        private const string ID3v22FrameAlbum = "TAL";
        private const string ID3v22FrameDate = "TYE";
        private const string ID3v22FrameComment = "COM";
        private const string ID3v22FrameTrackNo = "TRK";
        private const string ID3v22FrameGenle = "TCO";

        /// <summary>
        /// ID3V2.3 フレームID
        /// </summary>
        private const string ID3v23FrameTitle = "TIT2";
        private const string ID3v23FrameArtist = "TPE1";
        private const string ID3v23FrameAlbum = "TALB";
        private const string ID3v23FrameDate = "TYER";
        private const string ID3v23FrameComment = "COMM";
        private const string ID3v23FrameTrackNo = "TRCK";
        private const string ID3v23FrameGenle = "TCON";
        private const string ID3v23FramePicture = "APIC";

        /// <summary>
        /// ID3V1.1 オフセット
        /// </summary>
        private enum ID3v1Offset
        {
            Tag = 0,
            Title = 3,
            Artist = 33,
            Album = 63,
            Date = 93,
            Comment = 97,
            Blank = 125,
            TrackNo = 126,
            Genle = 127,
        }

        /// <summary>
        /// ID3V1.1 サイズ
        /// </summary>
        private enum ID3v1Length
        {
            Tag = 3,
            Title = 30,
            Artist = 30,
            Album = 30,
            Date = 4,
            Comment = 28,
            Blank = 1,
            TrackNo = 1,
            Genle = 1,
        }

        /// <summary>
        /// ID3V2 ヘッダー オフセット 
        /// </summary>
        private enum ID3v2HeaderOffset
        {
            Id = 0,
            Version = 3,
            Flag = 5,
            Size = 6
        }

        /// <summary>
        /// ID3V2 ヘッダー サイズ
        /// </summary>
        private enum ID3v2HeaderSize
        {
            Id = 3,
            Version = 2,
            Flag = 1,
            Size = 4,
            All = 10
        }

        /// <summary>
        /// ID3V2 拡張ヘッダ オフセット
        /// </summary>
        private enum ID3v2ExHeaderoffset
        {
            Size = 0,
            Flag = 4,
        }

        /// <summary>
        /// ID3V2 拡張ヘッダ サイズ
        /// </summary>
        private enum ID3v2ExHeaderSize
        {
            Size = 4,
            Flag = 2,
            All = 6
        }

        /// <summary>
        /// ID3V2 フレーム オフセット
        /// </summary>
        private enum ID3v2FHeaderOffset
        {
            Id = 0,
            Size = 4,
            Flag = 8
        }

        /// <summary>
        /// ID3V2 フレーム サイズ
        /// </summary>
        private enum ID3v2FHeaderSize
        {
            Id = 4,
            Size = 4,
            Flag = 2,
            All = 10
        }

        /// <summary>
        /// ID3V2 拡張フレーム オフセット
        /// </summary>
        private enum ID3v2ExtFrameOffset
        {
            ID = 0,
            SIZE = 3,
        }

        /// <summary>
        /// ID3V2 拡張フレーム サイズ 
        /// </summary>
        private enum ID3v2ExtFrameSize
        {
            Id = 3,
            Size = 3,
            All = 6
        }

        /// <summary>
        /// 
        /// </summary>
        private enum ID3v2FPayloadOffset
        {
            Encoding = 0,
            Data = 1,
        }

        /// <summary>
        /// 
        /// </summary>
        private enum ID3v2FPayloadSizes
        {
            Encoding = 1,
        }

        /// <summary>
        /// エンコーディング タイプ一覧
        /// </summary>
        private enum ID3v2EncodingTypes
        {
            ISO_8859_1 = 0x00,      // ISO-8859-1
            UTF_16 = 0x01,          // UTF-16 BOMあり
            UTF_16BE = 0x02,        // UTF-16BE / BOMなし
            UTF_8 = 0x03            // UTF-8
        }
        
        /// <summary>
        /// ID3v1 パース
        /// </summary>
        static public IAudioDetail GetDetailID3v1(string filePath)
        {
            IAudioDetail detail = new AudioDetail();
            detail.FilePath = filePath;

            using (var reader = new Mp3FileReader(filePath))
            {
                
                byte[] tagV1 = reader.Id3v1Tag;
                string Tag = Encoding.Default.GetString(tagV1, (int)ID3v1Offset.Tag, (int)ID3v1Length.Tag);
                if (Tag.Equals("TAG"))
                {
                    detail.Title = Encoding.Default.GetString(tagV1, (int)ID3v1Offset.Title, (int)ID3v1Length.Title);
                    detail.Artist = Encoding.Default.GetString(tagV1, (int)ID3v1Offset.Artist, (int)ID3v1Length.Artist);
                    detail.Album = Encoding.Default.GetString(tagV1, (int)ID3v1Offset.Album, (int)ID3v1Length.Album);
                    detail.Date = Encoding.Default.GetString(tagV1, (int)ID3v1Offset.Date, (int)ID3v1Length.Date);
                    detail.Comment = Encoding.Default.GetString(tagV1, (int)ID3v1Offset.Comment, (int)ID3v1Length.Comment);
                    detail.TrackNo = Encoding.Default.GetString(tagV1, (int)ID3v1Offset.TrackNo, (int)ID3v1Length.TrackNo);
                    detail.Genle = Encoding.Default.GetString(tagV1, (int)ID3v1Offset.Genle, (int)ID3v1Length.Genle);
                }
            }

            return detail;
        }

        /// <summary>
        /// ID3v2 パース
        /// </summary>
        static public IAudioDetail GetDetailID3v23(string filePath)
        {
            IAudioDetail detail = new AudioDetail();
            detail.FilePath = filePath;

            using (var reader = new Mp3FileReader(filePath))
            {
                if (reader.Id3v2Tag == null)
                {
                    return detail;
                }

                var tagV2 = reader.Id3v2Tag.RawData;
                var encoder = Encoding.GetEncoding("iso-8859-1");

                // Header
                var id = encoder.GetString(tagV2, (int)ID3v2HeaderOffset.Id, (int)ID3v2HeaderSize.Id);
                var ver = encoder.GetString(tagV2, (int)ID3v2HeaderOffset.Version, (int)ID3v2HeaderSize.Version);
                var flag = tagV2[(int)ID3v2HeaderOffset.Flag];

                // Syncsafe Integer
                var tagSize = ((tagV2[(int)ID3v2HeaderOffset.Size] & SyncsafeIntegerMask) << 21)
                            + ((tagV2[(int)ID3v2HeaderOffset.Size + 1] & SyncsafeIntegerMask) << 14)
                            + ((tagV2[(int)ID3v2HeaderOffset.Size + 2] & SyncsafeIntegerMask) << 7)
                            + (tagV2[(int)ID3v2HeaderOffset.Size + 3] & SyncsafeIntegerMask);

                // Frame
                var fHeaderIndex = (int)ID3v2HeaderSize.All;
                while (fHeaderIndex < tagSize)
                {
                    if (tagV2[fHeaderIndex] == '\0')
                    {
                        break;
                    }

                    // Frame Header 
                    var frameId = encoder.GetString(tagV2, fHeaderIndex + (int)ID3v2FHeaderOffset.Id, (int)ID3v2FHeaderSize.Id);
                    var fSizeArray = new byte[(int)ID3v2FHeaderSize.Size];

                    Buffer.BlockCopy(tagV2, fHeaderIndex + (int)ID3v2FHeaderOffset.Size, fSizeArray, 0, (int)ID3v2FHeaderSize.Size);
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(fSizeArray);
                    }
                    var frameSize = BitConverter.ToInt32(fSizeArray, 0);

                    var highFlag = tagV2[fHeaderIndex + (int)ID3v2FHeaderOffset.Flag];
                    var lowFlag = tagV2[fHeaderIndex + (int)ID3v2FHeaderOffset.Flag + 1];

                    // Frame Payload
                    var encodeType = tagV2[fHeaderIndex + (int)ID3v2FHeaderSize.All];

                    // FIXME:frameSizeが一致しない、frameSizeをそのままパーズすると文字列語尾に制御文字がつく
                    var encodeBuffer = new byte[frameSize - 1];
                    Buffer.BlockCopy(tagV2, fHeaderIndex + (int)ID3v2FHeaderSize.All + 1, encodeBuffer, 0, frameSize - 1);
                    encodeAndSet(frameId, encodeType, encodeBuffer, ref detail);

                    fHeaderIndex += (int)ID3v2FHeaderSize.All + frameSize;
                }
            }

            return detail;
        }

        static public BitmapImage GetArtwork(string filePath)
        {
            return null;
        }

        static private void encodeAndSet(string id, byte encodeType, byte[] value, ref IAudioDetail detail)
        {
            var str = EncodingString(encodeType, value);
            switch (id)
            {
                case ID3v23FrameTitle:
                    detail.Title = str;
                    break;
                case ID3v23FrameArtist:
                    detail.Artist = str;
                    break;
                case ID3v23FrameAlbum:
                    detail.Album = str;
                    break;
                case ID3v23FrameDate:
                    detail.Date = str;
                    break;
                case ID3v23FrameComment:
                    detail.Comment = str;
                    break;
                case ID3v23FrameTrackNo:
                    detail.TrackNo = GetTrackNoFromTRCKFormat(str);
                    break;
                case ID3v23FrameGenle:
                    detail.Genle = str;
                    break;
                default:
                    break;
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
            var encoding = GetEncoding(type);
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
        static Encoding GetEncoding(byte type)
        {
            switch (type)
            {
                case (int)ID3v2EncodingTypes.ISO_8859_1:
                    return Encoding.GetEncoding("iso-8859-1");
                case (int)ID3v2EncodingTypes.UTF_16:
                    return new UnicodeEncoding(false, true);
                case (int)ID3v2EncodingTypes.UTF_16BE:
                    return Encoding.BigEndianUnicode;
                case (int)ID3v2EncodingTypes.UTF_8:
                    return Encoding.UTF8;
                default:
                    return Encoding.Default;
            }
        }
    }
}
