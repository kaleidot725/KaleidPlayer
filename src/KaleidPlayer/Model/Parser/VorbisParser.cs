using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaleidot725.Model
{
    public class VorbisComment
    {
        private const char SplitCharacter = '=';
        private const string FieldNameTitle = "TITLE";
        private const string FieldNameVersion = "VERSION";
        private const string FieldNameAlbum = "ALBUM";
        private const string FieldNameTrackNumber = "TRACKNUMBER";
        private const string FieldNameArtist = "ARTIST";
        private const string FieldNasmePerformer = "PERFORMER";
        private const string FieldNameCopyright = "COPYRIGHT";
        private const string FieldNameLicense = "LICENSE";
        private const string FieldNameOrganization = "ORGANIZATION";
        private const string FieldNameDescription = "DESCRIPTION";
        private const string FieldNameGenle = "GENLE";
        private const string FieldNameDate = "DATE";
        private const string FieldNameLocation = "LOCATION";
        private const string FieldNameContact = "CONTACT";
        private const string FieldNameIsrc = "ISRC";
        private const string FieldnameAlbumArtist = "ALBUMARTIST";

        public enum VorbitCommentType
        {
            Title = 0,
            Version,
            Album,
            TrackNumber,
            Artist,
            Performer,
            Copyright,
            License,
            Organization,
            Description,
            Genre,
            Date,
            Location,
            Contact,
            Isrc,
            AlbumArtist,
            Unknown
        }

        static public void Parse(string source, out VorbitCommentType type,out string value)
        {
            List<string> list = new List<string>(source.Split(SplitCharacter));
            if (list.Count < 2)
            {
                type = VorbitCommentType.Unknown;
                value = "";
                return;
            }

            type = convertFieldStrToType(list[0]);
            value = list[1];
        }

        static private VorbitCommentType convertFieldStrToType(string source)
        {
            switch (source)
            {
                case FieldNameTitle:
                    return VorbitCommentType.Title;
                case FieldNameVersion:
                    return VorbitCommentType.Version;
                case FieldNameAlbum:
                    return VorbitCommentType.Album;
                case FieldNameTrackNumber:
                    return VorbitCommentType.TrackNumber;
                case FieldNameArtist:
                    return VorbitCommentType.Artist;
                case FieldNasmePerformer:
                    return VorbitCommentType.Performer;
                case FieldNameCopyright:
                    return VorbitCommentType.Copyright;
                case FieldNameLicense:
                    return VorbitCommentType.License;
                case FieldNameOrganization:
                    return VorbitCommentType.Organization;
                case FieldNameDescription:
                    return VorbitCommentType.Description;
                case FieldNameGenle:
                    return VorbitCommentType.Genre;
                case FieldNameDate:
                    return VorbitCommentType.Date;
                case FieldNameLocation:
                    return VorbitCommentType.Location;
                case FieldNameContact:
                    return VorbitCommentType.Contact;
                case FieldNameIsrc:
                    return VorbitCommentType.Isrc;
                case FieldnameAlbumArtist:
                    return VorbitCommentType.AlbumArtist;
                default:
                    return VorbitCommentType.Unknown;
            }
        }
    }
}
