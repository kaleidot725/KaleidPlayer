using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using NAudio.Wave;
using NAudio.Flac;
using System.IO;
using System.Timers;

namespace kaleidot725.Model
{
    /// <summary>
    /// NAudio.Wave.PlaybackState 要素付加
    /// </summary>
    public enum AudioPlaybackState
    {
        Playing = PlaybackState.Playing,
        Paused = PlaybackState.Paused,
        Stopped = PlaybackState.Stopped,
        None = -1,
    }

    /// <summary>
    /// 音楽プレイヤー
    /// </summary>
    public class AudioPlayer : BindableBase, IDisposable
    {
        private WaveStream _audioStream;
        private WaveChannel32 _volumeStream;
        private IWavePlayer _waveOut;

        /// <summary>
        /// 現在時間
        /// </summary>
        public TimeSpan CurrentTime
        {
            get { return (_audioStream != null) ? _audioStream.CurrentTime : TimeSpan.FromSeconds(0); }
        }

        /// <summary>
        /// 合計時間
        /// </summary>
        public TimeSpan TotalTime
        {
            get { return (_audioStream != null) ? _audioStream.TotalTime : TimeSpan.FromSeconds(0); }
        }

        /// <summary>
        /// 前再生状態
        /// </summary>
        private AudioPlaybackState _prePlaybackState;
        public AudioPlaybackState PrePlaybackState
        {
            get { return _prePlaybackState; }
        }

        /// <summary>
        /// 再生状態
        /// </summary>
        private AudioPlaybackState _playbackState;
        public AudioPlaybackState PlaybackState
        {
            get { return _playbackState; }
            private set
            {
                SetProperty(ref _prePlaybackState, PlaybackState);
                SetProperty(ref _playbackState, value);
            }
        }

        /// <summary>
        /// プレイ
        /// </summary>
        public void Play(AudioDetailBase audio)
        {
            if (audio == null)
            {
                throw new System.ArgumentNullException();
            }

            if (File.Exists(audio.FilePath) != true)
            {
                throw new FileNotFoundException(audio.FilePath);
            }

            if (_audioStream != null || _waveOut != null)
            {
                this.Dispose();
            }

            this.InitializeStream(audio.FilePath);
            this._waveOut.Play();
            PlaybackState = AudioPlaybackState.Playing;
        }

        /// <summary>
        /// リプレイ
        /// </summary>
        public void Replay()
        {
            if (PlaybackState == AudioPlaybackState.None)
            {
                return;
            }

            if (PlaybackState == AudioPlaybackState.Paused)
            {
                this._waveOut.Play();
                PlaybackState = AudioPlaybackState.Playing;
            }
        }

        /// <summary>
        /// ポーズ
        /// </summary>
        public void Pause()
        {
            if (PlaybackState == AudioPlaybackState.None)
            {
                return;
            }

            if (PlaybackState == AudioPlaybackState.Playing)
            {
                this._waveOut.Pause();
                PlaybackState = AudioPlaybackState.Paused;
            }
        }

        /// <summary>
        /// ストップ
        /// </summary>
        public void Stop()
        {
            if (PlaybackState == AudioPlaybackState.None)
            {
                return;
            }

            if (PlaybackState == AudioPlaybackState.Playing ||
                PlaybackState == AudioPlaybackState.Paused)
            {
                this._waveOut.Stop();
                this._audioStream.Position = 0;
            }

            // 破棄する
            Dispose();
            PlaybackState = AudioPlaybackState.None;
        }

        /// <summary>
        /// シーク
        /// </summary>
        public void Seek(TimeSpan SeekTime)
        {
            if (PlaybackState == AudioPlaybackState.None)
            {
                return;
            }

            if (PlaybackState == AudioPlaybackState.Playing)
            {
                this._waveOut.Pause();
            }

            var second = SeekTime.TotalSeconds;
            _audioStream.Position = (int)((double)_audioStream.WaveFormat.AverageBytesPerSecond * second);
            this._waveOut.Play();
            PlaybackState = AudioPlaybackState.Playing;
        }

        /// <summary>
        /// ボリューム取得
        /// </summary>
        /// <returns></returns>
        public float GetVolume()
        {
            if (PlaybackState == AudioPlaybackState.None)
            {
                return 0;
            }

            return this._volumeStream.Volume;
        }

        /// <summary>
        /// ボリューム設定
        /// </summary>
        /// <param name="value"></param>
        public void SetVolume(float value)
        {
            if (PlaybackState == AudioPlaybackState.None)
            {
                return;
            }

            this._volumeStream.Volume = value;
        }

        /// <summary>
        /// リソース解放
        /// </summary>
        public void Dispose()
        {
            if (_audioStream != null)
            {
                _audioStream.Dispose();
            }

            if (_waveOut != null)
            {
                _waveOut.Dispose();
            }

            _waveOut = null;
            _audioStream = null;
        }

        /// <summary>
        /// ストリーム初期化
        /// </summary>
        /// <param name="fileName"></param>
        private void InitializeStream(string fileName)
        {
            WaveStream reader = null;

            var type = AudioType.ParseAudioType(fileName);
            switch (type)
            {
                case AudioType.Types.Wave:
                    reader = new WaveFileReader(fileName);
                    break;
                case AudioType.Types.Mp3:
                    reader = new Mp3FileReader(fileName);
                    break;
                case AudioType.Types.Flac:
                    reader = new FlacReader(fileName);
                    break;
                default:
                    throw new System.NotSupportedException();
            }

            _audioStream = reader;
            this._waveOut = new WaveOut();
            this._waveOut.Init(this._audioStream);
            this._waveOut.PlaybackStopped += new EventHandler<StoppedEventArgs>(StoppedEvent);
            PlaybackState = AudioPlaybackState.Stopped;
        }

        /// <summary>
        /// ストップイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void StoppedEvent(object sender, StoppedEventArgs args)
        {
            PlaybackState = AudioPlaybackState.Stopped;
        }
    }
}
