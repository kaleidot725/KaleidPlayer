using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace kaleidot725.Model
{
    public class AudioDetail : BindableBase, IAudioDetail
    {

        private string title;          // タイトル
        private string artist;         // アーティスト
        private string album;          // アルバム
        private string date;           // 西暦
        private string trackNo;        // トラックNo
        private string genle;          // ジャンル
        private string comment;        // コメント
        private string albumArtist;    // アルバムアーティスト
        private string composer;       // 作曲者
        private string discnumber;     // ディスクナンバー
        private string filepath;       // ファイルパス

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        /// <summary>
        /// アーティスト
        /// </summary>
        public string Artist
        {
            get { return artist; }
            set { SetProperty(ref artist, value); }
        }

        /// <summary>
        /// アルバム
        /// </summary>
        public string Album
        {
            get { return album; }
            set { SetProperty(ref album, value); }
        }

        /// <summary>
        /// 西暦
        /// </summary>
        public string Date
        {
            get { return date; }
            set { SetProperty(ref date, value); }
        }

        /// <summary>
        /// トラックNo
        /// </summary>
        public string TrackNo
        {
            get { return trackNo; }
            set { SetProperty(ref trackNo, value); }
        }

        /// <summary>
        /// ジャンル
        /// </summary>
        public string Genle
        {
            get { return genle; }
            set { SetProperty(ref genle, value); }
        }

        /// <summary>
        /// コメント
        /// </summary>
        public string Comment
        {
            get { return comment; }
            set { SetProperty(ref comment, value); }
        }

        /// <summary>
        /// アルバムアーティスト
        /// </summary>
        public string AlbumArtist
        {
            get { return albumArtist; }
            set { SetProperty(ref albumArtist, value); }
        }

        /// <summary>
        /// 作曲者
        /// </summary>
        public string Composer
        {
            get { return composer; }
            set { SetProperty(ref composer, value); }
        }

        /// <summary>
        /// ディスクNo
        /// </summary>
        public string DiscNumber
        {
            get { return discnumber; }
            set { SetProperty(ref discnumber, value); }
        }

        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FilePath
        {
            get { return filepath; }
            set { SetProperty(ref filepath, value); }
        }
    }
}
