using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaleidot725.Model
{
    [Serializable]
    public class AudioDetailSerializable
    {
        /// <summary>
        /// タイトル
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// アーティスト
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// アルバム
        /// </summary>
        public string Album { get; set; }

        /// <summary>
        /// 西暦
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// トラックNo
        /// </summary>
        public string TrackNo { get; set; }

        /// <summary>
        /// ジャンル
        /// </summary>
        public string Genle { get; set; }

        /// <summary>
        /// コメント
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// アルバムアーティスト
        /// </summary>
        public string AlbumArtist { get; set; }

        /// <summary>
        /// 作曲者
        /// </summary>
        public string Composer { get; set; }

        /// <summary>
        /// ディスクNo
        /// </summary>
        public string DiscNumber { get; set; }

        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FilePath { get; set; }

        public AudioDetailSerializable(IAudioDetail detail)
        {
            this.Title = detail.Title;
            this.Artist = detail.Artist;
            this.Album = detail.Album;
            this.Date = detail.Date;
            this.TrackNo = detail.TrackNo;
            this.Genle = detail.Comment;
            this.Comment = detail.Comment;
            this.AlbumArtist = detail.AlbumArtist;
            this.Composer = detail.Composer;
            this.DiscNumber = detail.DiscNumber;
            this.FilePath = detail.FilePath;
        }
    }
}
