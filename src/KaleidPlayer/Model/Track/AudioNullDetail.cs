using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace kaleidot725.Model
{
    /// <summary>
    /// NULLオブジェクト
    /// </summary>
    public class AudioNullDetail : BindableBase, IAudioDetail
    {

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
        public AudioNullDetail()
        {
            return;
        }

        /// <summary>
        /// パース
        /// </summary>
        public void Parse()
        {
            return;
        }

    }
}
