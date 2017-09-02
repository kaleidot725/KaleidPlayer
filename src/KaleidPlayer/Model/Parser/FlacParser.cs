using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace kaleidot725.Model
{
    public class FlacParser
    {
        private const string FLAC_MARKER_STRING = "fLaC";

        /// <summary>
        /// オフセット
        /// </summary>
        private enum FlacOffset
        {
            Marker = 0,
            Metadata = 4
        }

        /// <summary>
        /// 長さ
        /// </summary>
        private enum FlacLength
        {
            Marker = 4,
        }

        /// <summary>
        /// メタオフセット
        /// </summary>
        private enum MetaOffset
        {
            Type = 0,
            Size = 1,
            Payload = 4
        }

        /// <summary>
        /// メタ長さ
        /// </summary>
        private enum MetaLength
        {
            Type = 1,
            Size = 3,
        }

        /// VORBISコメントサイズ
        /// </summary>
        private enum VorbitCommentSize
        {
            VenderCommentSize = 4,
            CommentNumber = 4,
            CommentSize = 4,
            CommentLength = 4
        }

        public static IAudioDetail GetDetail(string filePath)
        {
            try
            {
                IAudioDetail detail = new AudioDetail();
                detail.FilePath = filePath;

                using (var stream = File.Open(filePath, FileMode.Open))
                {
                    var metaList = new List<FlacMetaData>();
                    collectMetaData(stream, ref metaList);
                    parseMedaData(stream, ref metaList, ref detail);
                    return detail;
                }
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException("Parse File Not Found.", e);
            }
        }

        public static BitmapImage GetArtwork(string filePath)
        {
            return null;
        }

        /// <summary>
        /// メタデータ収集
        /// </summary>
        private static void collectMetaData(Stream stream, ref List<FlacMetaData> metaList)
        {
            var markerBuff = new byte[(int)FlacLength.Marker];
            stream.Read(markerBuff, (int)FlacOffset.Marker, (int)FlacLength.Marker);

            var markerStr = Encoding.Default.GetString(markerBuff);
            if (markerStr == FLAC_MARKER_STRING)
            {
                var isLastMetaData = false;
                while (!isLastMetaData)
                {
                    FlacMetaType metaType;
                    var flags = new byte[(int)MetaLength.Type];
                    stream.Read(flags, 0, flags.Length);

                    isLastMetaData = ((flags[0] >> 7) == 1) ? (true) : (false);
                    metaType = (FlacMetaType)((flags[0] & 0x7F));

                    long metaSize;
                    var sizeBuff = new byte[(int)MetaLength.Size];
                    var uint32Buff = new byte[sizeof(UInt32)];
                    stream.Read(sizeBuff, 0, sizeBuff.Length);

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(sizeBuff);
                    }
                    Buffer.BlockCopy(sizeBuff, 0, uint32Buff, 0, sizeBuff.Length);
                    metaSize = BitConverter.ToInt32(uint32Buff, 0);

                    long metaIndex;
                    metaIndex = stream.Position;

                    metaList.Add(new FlacMetaData(metaType, metaIndex, metaSize));
                    stream.Position += metaSize;
                }
            }

            stream.Position = 0;
        }

        /// <summary>
        /// メタデータパーズ
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="metaList"></param>
        private static void parseMedaData(Stream stream, ref List<FlacMetaData> metaList, ref IAudioDetail detail)
        {
            foreach (var metaItem in metaList)
            {
                switch (metaItem.Type)
                {
                    case FlacMetaType.StreamInfo:
                        break;
                    case FlacMetaType.Padding:
                        break;
                    case FlacMetaType.Application:
                        break;
                    case FlacMetaType.SeekTable:
                        break;
                    case FlacMetaType.VorbisComment:
                        ParseVorbisComment(stream, metaItem, ref detail);
                        break;
                    case FlacMetaType.CueSheet:
                        break;
                    case FlacMetaType.Picture:
                        break;
                }
            }
        }

        /// <summary>
        /// Streaminfo パーズ
        /// </summary>
        /// <param name="metaData"></param>
        private static void ParseStreamInfo(FlacMetaData metaData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Padding パーズ
        /// </summary>
        /// <param name="metaData"></param>
        private static void ParsePadding(FlacMetaData metaData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Application パーズ
        /// </summary>
        /// <param name="metaData"></param>
        private static void ParseApplication(FlacMetaData metaData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// SeekTable パーズ
        /// </summary>
        /// <param name="metaData"></param>
        private static void ParseSeektable(FlacMetaData metaData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// VorbisCommentのみリトルエンディアンで情報が格納されているので注意
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="metaData"></param>
        private static void ParseVorbisComment(Stream stream, FlacMetaData metaData, ref IAudioDetail detail)
        {
            stream.Position = metaData.Index;

            int venderCommentSize;
            var venderCommentSizeBuffer = new byte[(int)VorbitCommentSize.VenderCommentSize];
            stream.Read(venderCommentSizeBuffer, 0, venderCommentSizeBuffer.Length);
            venderCommentSize = BitConverter.ToInt32(venderCommentSizeBuffer, 0);

            string venderComment;
            var venderCommentBuffer = new byte[venderCommentSize];
            stream.Read(venderCommentBuffer, 0, venderCommentSize);
            venderComment = Encoding.Default.GetString(venderCommentBuffer);

            int commentNumber;
            var commentNumberBuffer = new byte[(int)VorbitCommentSize.CommentNumber];
            stream.Read(commentNumberBuffer, 0, commentNumberBuffer.Length);
            commentNumber = BitConverter.ToInt32(commentNumberBuffer, 0);

            int commentCount = 0;
            while (commentCount < commentNumber)
            {
                var lengthBuffer = new byte[(int)VorbitCommentSize.VenderCommentSize];
                stream.Read(lengthBuffer, 0, lengthBuffer.Length);
                Int32 length = BitConverter.ToInt32(lengthBuffer, 0);

                var valueBuffer = new byte[length];
                stream.Read(valueBuffer, 0, valueBuffer.Length);
                string value = Encoding.UTF8.GetString(valueBuffer);

                VorbisComment.VorbitCommentType vorbisType;
                string vobisValue;

                VorbisComment.Parse(value, out vorbisType, out vobisValue);
                switch (vorbisType)
                {
                    case VorbisComment.VorbitCommentType.Title:
                        detail.Title = vobisValue;
                        break;
                    case VorbisComment.VorbitCommentType.Version:
                        break;
                    case VorbisComment.VorbitCommentType.Album:
                        detail.Album = vobisValue;
                        break;
                    case VorbisComment.VorbitCommentType.TrackNumber:
                        detail.TrackNo = vobisValue;
                        break;
                    case VorbisComment.VorbitCommentType.Artist:
                        detail.Artist = vobisValue;
                        break;
                    case VorbisComment.VorbitCommentType.Performer:
                        break;
                    case VorbisComment.VorbitCommentType.Copyright:
                        break;
                    case VorbisComment.VorbitCommentType.License:
                        break;
                    case VorbisComment.VorbitCommentType.Organization:
                        break;
                    case VorbisComment.VorbitCommentType.Genre:
                        break;
                    case VorbisComment.VorbitCommentType.Date:
                        detail.Date = vobisValue;
                        break;
                    case VorbisComment.VorbitCommentType.Location:
                        break;
                    case VorbisComment.VorbitCommentType.Contact:
                        break;
                    case VorbisComment.VorbitCommentType.Isrc:
                        break;
                    case VorbisComment.VorbitCommentType.AlbumArtist:
                        detail.AlbumArtist = vobisValue;
                        break;
                    default:
                        break;
                }

                commentCount++;
            }
        }
    }
}
