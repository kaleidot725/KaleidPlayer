using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NAudio.Flac;

namespace kaleidot725.Model
{
    class AudioFlacDetail : AudioDetailBase
    {
        private const string FLAC_MARKER_STRING = "fLaC";

        /// <summary>
        /// 
        /// </summary>
        private enum FLAC_OFFSET
        {
            MARKER = 0,
            METADATA = 4
        }

        /// <summary>
        /// 
        /// </summary>
        private enum FLAC_LENGTH
        {
            MARKER = 4,
        }

        /// <summary>
        /// 
        /// </summary>
        private enum META_OFFSET
        {
            TYPE = 0,
            SIZE = 1,
            PAYLOAD = 4
        }

        /// <summary>
        /// 
        /// </summary>
        private enum META_LENGTH
        {
            TYPE = 1,
            SIZE = 3,
        }

        /// <summary>
        /// 
        /// </summary>
        private enum META_TYPE
        {
            STREAMINFO = 0,
            PADDING = 1,
            APPLICATION = 2,
            SEEKTABLE = 3,
            VORBIS_COMMENT = 4,
            CUESHEET = 5,
            PICTURE = 6
        }

        private enum VORBIS_COMMENT_SIZE
        {
            VENDER_COMMENT_SIZE = 4,
            COMMENT_NUMBER = 4,
            COMMENT_SIZE = 4,
            COMMENT_LENGTH = 4
        }

        /// <summary>
        /// 
        /// </summary>
        private class MetaData
        {
            private META_TYPE _type;
            private long _index;
            private long _size;
            
            public META_TYPE Type
            {
                get { return _type; }
                set { _type = value; }
            }

            public long Index
            {
                get { return _index; }
                set { _index = value; }
            }

            public long Size
            {
                get { return _size; }
                set { _size = value; }
            }

            public MetaData(META_TYPE type, long index, long size)
            {
                _type = type;
                _index = index;
                _size = size;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public AudioFlacDetail(string filePath)
        {
            if (File.Exists(filePath) != true)
            {
                throw new FileNotFoundException();
            }

            FilePath = filePath;

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Parse()
        {
            List<MetaData> _metaList; _metaList = new List<MetaData>();

            var stream = File.Open(FilePath, FileMode.Open);

            CollectMetaData(stream, ref _metaList);
            ParseMetaData(stream, ref _metaList);
            _metaList.Clear();

            stream.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CollectMetaData(Stream stream, ref List<MetaData> metaList)
        {
            byte[] markerBuffer = new byte[(int)FLAC_LENGTH.MARKER];
            stream.Read(markerBuffer, (int)FLAC_OFFSET.MARKER, (int)FLAC_LENGTH.MARKER);

            var markerString = Encoding.Default.GetString(markerBuffer);
            if (markerString == FLAC_MARKER_STRING)
            {
                var isLastMetaData = false;
                while (!isLastMetaData)
                {
                    META_TYPE metaType;
                    byte[] flags = new byte[(int)META_LENGTH.TYPE];
                    stream.Read(flags, 0, flags.Length);

                    isLastMetaData = ((flags[0] >> 7) == 1) ? (true) : (false);
                    metaType = (META_TYPE)((flags[0] & 0x7F));

                    long metaSize;
                    byte[] sizeBuff = new byte[(int)META_LENGTH.SIZE];
                    byte[] uint32Buff = new byte[sizeof(UInt32)];
                    stream.Read(sizeBuff, 0, sizeBuff.Length);

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(sizeBuff);
                    }
                    Buffer.BlockCopy(sizeBuff, 0, uint32Buff, 0, sizeBuff.Length);
                    metaSize = BitConverter.ToInt32(uint32Buff, 0);

                    long metaIndex;
                    metaIndex = stream.Position;

                    metaList.Add(new MetaData(metaType, metaIndex, metaSize));
                    stream.Position += metaSize;
                }
            }

            stream.Position = 0;
        }

        private void ParseMetaData(Stream stream, ref List<MetaData> metaList)
        {
            foreach (var metaItem in metaList)
            {
                switch (metaItem.Type)
                {
                    case META_TYPE.STREAMINFO:
                        break;
                    case META_TYPE.PADDING:
                        break;
                    case META_TYPE.APPLICATION:
                        break;
                    case META_TYPE.SEEKTABLE:
                        break;
                    case META_TYPE.VORBIS_COMMENT:
                        ParseVorbisComment(stream, metaItem);
                        break;
                    case META_TYPE.CUESHEET:
                        break;
                    case META_TYPE.PICTURE:
                        break;
                }
            }
        }

        private void ParseStreamInfo(MetaData metaData)
        {
            throw new NotImplementedException();
        }

        private void ParsePadding(MetaData metaData)
        {
            throw new NotImplementedException();
        }

        private void ParseApplication(MetaData metaData)
        {
            throw new NotImplementedException();
        }

        private void ParseSeektable(MetaData metaData)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// VorbisCommentのみリトルエンディアンで情報が格納されているので注意
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="metaData"></param>
        private void ParseVorbisComment(Stream stream, MetaData metaData)
        {
            stream.Position = metaData.Index;

            int venderCommentSize;
            byte[] venderCommentSizeBuffer = new byte[(int)VORBIS_COMMENT_SIZE.VENDER_COMMENT_SIZE];
            stream.Read(venderCommentSizeBuffer, 0, venderCommentSizeBuffer.Length);
            venderCommentSize = BitConverter.ToInt32(venderCommentSizeBuffer, 0);

            string venderComment;
            byte[] venderCommentBuffer = new byte[venderCommentSize];
            stream.Read(venderCommentBuffer, 0, venderCommentSize);
            venderComment = Encoding.Default.GetString(venderCommentBuffer);

            int commentNumber;
            byte[] commentNumberBuffer = new byte[(int)VORBIS_COMMENT_SIZE.COMMENT_NUMBER];
            stream.Read(commentNumberBuffer, 0, commentNumberBuffer.Length);
            commentNumber = BitConverter.ToInt32(commentNumberBuffer, 0);
            
            int commentCount = 0;
            while (commentCount < commentNumber )
            {
                byte[] lengthBuffer = new byte[(int)VORBIS_COMMENT_SIZE.VENDER_COMMENT_SIZE];
                stream.Read(lengthBuffer, 0, lengthBuffer.Length);
                Int32 length = BitConverter.ToInt32(lengthBuffer, 0);

                byte[] valueBuffer = new byte[length];
                stream.Read(valueBuffer, 0, valueBuffer.Length);
                string value = Encoding.Default.GetString(valueBuffer);

                VorbisComment.VORBIS_COMMENT_TYPE vorbisType;
                string vobisValue;

                VorbisComment.Parse(value, out vorbisType, out vobisValue);
                switch(vorbisType)
                {
                    case VorbisComment.VORBIS_COMMENT_TYPE.TITLE:
                        this.Title = vobisValue;
                        break;
                    case VorbisComment.VORBIS_COMMENT_TYPE.VERSION:
                        break;
                    case VorbisComment.VORBIS_COMMENT_TYPE.ALBUM:
                        this.Album = vobisValue;
                        break;
                    case VorbisComment.VORBIS_COMMENT_TYPE.TRACKNUMBER:
                        this.TrackNo = vobisValue;
                        break;
                    case VorbisComment.VORBIS_COMMENT_TYPE.ARTIST:
                        this.Artist = vobisValue;
                        break;
                    case VorbisComment.VORBIS_COMMENT_TYPE.PERFORMER:
                        break;
                    case VorbisComment.VORBIS_COMMENT_TYPE.COPYRIGHT:
                        break;
                    case VorbisComment.VORBIS_COMMENT_TYPE.LICENSE:
                        break;
                    case VorbisComment.VORBIS_COMMENT_TYPE.ORGANIZATION:
                        break;
                    case VorbisComment.VORBIS_COMMENT_TYPE.GENRE:
                        break;
                    case VorbisComment.VORBIS_COMMENT_TYPE.DATE:
                        this.Date = vobisValue;
                        break;
                    case VorbisComment.VORBIS_COMMENT_TYPE.LOCATION:
                        break;
                    case VorbisComment.VORBIS_COMMENT_TYPE.CONTACT:
                        break;
                    case VorbisComment.VORBIS_COMMENT_TYPE.ISRC:
                        break;

                    case VorbisComment.VORBIS_COMMENT_TYPE.ALBUMARTIST:
                        this.AlbumArtist = vobisValue;
                        break;
                    default:
                        break;
                }

                commentCount++;
            }
        }
    }
}
