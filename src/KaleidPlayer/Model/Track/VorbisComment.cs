using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaleidot725.Model
{
    class VorbisComment
    {
        private const char SPLIT_CHARACTER = '=';
        private const string FIELD_NAME_TITLE = "TITLE";
        private const string FIELD_NAME_VERSION = "VERSION";
        private const string FIELD_NAME_ALBUM = "ALBUM";
        private const string FIELD_NAME_TRACKNUMBER = "TRACKNUMBER";
        private const string FIELD_NAME_ARTIST = "ARTIST";
        private const string FIELD_NAME_PERFORMER = "PERFORMER";
        private const string FIELD_NAME_COPYRIGHT = "COPYRIGHT";
        private const string FIELD_NAME_LICENSE = "LICENSE";
        private const string FIELD_NAME_ORGANIZATION = "ORGANIZATION";
        private const string FIELD_NAME_DESCRIPTION = "DESCRIPTION";
        private const string FIELD_NAME_GENLE = "GENLE";
        private const string FIELD_NAME_DATE = "DATE";
        private const string FIELD_NAME_LOCATION = "LOCATION";
        private const string FIELD_NAME_CONTACT = "CONTACT";
        private const string FIELD_NAME_ISRC = "ISRC";
        private const string FIELD_NAME_ALBUMARTIST = "ALBUMARTIST";

        public enum VORBIS_COMMENT_TYPE
        {
            TITLE = 0,
            VERSION,
            ALBUM,
            TRACKNUMBER,
            ARTIST,
            PERFORMER,
            COPYRIGHT,
            LICENSE,
            ORGANIZATION,
            DESCRIPTION,
            GENRE,
            DATE,
            LOCATION,
            CONTACT,
            ISRC,
            ALBUMARTIST,
            UNKNOWN
        }

        static public void Parse(string source, out VORBIS_COMMENT_TYPE type,out string value)
        {
            List<string> list = new List<string>(source.Split(SPLIT_CHARACTER));
            if (list.Count < 2)
            {
                type = VORBIS_COMMENT_TYPE.UNKNOWN;
                value = "";
                return;
            }

            type = convertFieldStrToType(list[0]);
            value = list[1];
        }

        static private VORBIS_COMMENT_TYPE convertFieldStrToType(string source)
        {
            switch (source)
            {
                case FIELD_NAME_TITLE:
                    return VORBIS_COMMENT_TYPE.TITLE;
                case FIELD_NAME_VERSION:
                    return VORBIS_COMMENT_TYPE.VERSION;
                case FIELD_NAME_ALBUM:
                    return VORBIS_COMMENT_TYPE.ALBUM;
                case FIELD_NAME_TRACKNUMBER:
                    return VORBIS_COMMENT_TYPE.TRACKNUMBER;
                case FIELD_NAME_ARTIST:
                    return VORBIS_COMMENT_TYPE.ARTIST;
                case FIELD_NAME_PERFORMER:
                    return VORBIS_COMMENT_TYPE.PERFORMER;
                case FIELD_NAME_COPYRIGHT:
                    return VORBIS_COMMENT_TYPE.COPYRIGHT;
                case FIELD_NAME_LICENSE:
                    return VORBIS_COMMENT_TYPE.LICENSE;
                case FIELD_NAME_ORGANIZATION:
                    return VORBIS_COMMENT_TYPE.ORGANIZATION;
                case FIELD_NAME_DESCRIPTION:
                    return VORBIS_COMMENT_TYPE.DESCRIPTION;
                case FIELD_NAME_GENLE:
                    return VORBIS_COMMENT_TYPE.GENRE;
                case FIELD_NAME_DATE:
                    return VORBIS_COMMENT_TYPE.DATE;
                case FIELD_NAME_LOCATION:
                    return VORBIS_COMMENT_TYPE.LOCATION;
                case FIELD_NAME_CONTACT:
                    return VORBIS_COMMENT_TYPE.CONTACT;
                case FIELD_NAME_ISRC:
                    return VORBIS_COMMENT_TYPE.ISRC;
                case FIELD_NAME_ALBUMARTIST:
                    return VORBIS_COMMENT_TYPE.ALBUMARTIST;
                default:
                    return VORBIS_COMMENT_TYPE.UNKNOWN;
            }
        }
    }
}
