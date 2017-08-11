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
    /// 音楽プレイヤー
    /// </summary>
    public class Player : BindableBase, IDisposable
    {
        private WaveStream audioStream;
        private WaveChannel32 volumeStream;
        private IWavePlayer waveout;

        /// <summary>
        /// 現在時間
        /// </summary>
        public TimeSpan CurrentTime
        {
            get { return (audioStream != null) ? audioStream.CurrentTime : TimeSpan.FromSeconds(0); }
        }

        /// <summary>
        /// 合計時間
        /// </summary>
        public TimeSpan TotalTime
        {
            get { return (audioStream != null) ? audioStream.TotalTime : TimeSpan.FromSeconds(0); }
        }

        /// <summary>
        /// 前再生状態
        /// </summary>
        private PlaybackState prePlaybackState;
        public PlaybackState PrePlaybackState
        {
            get { return prePlaybackState; }
        }

        /// <summary>
        /// 再生状態
        /// </summary>
        private PlaybackState playbackState;
        public PlaybackState PlaybackState
        {
            get { return playbackState; }
            private set
            {
                SetProperty(ref prePlaybackState, PlaybackState);
                SetProperty(ref playbackState, value);
            }
        }

        /// <summary>
        /// プレイ
        /// </summary>
        public void Play(IAudioDetail audio)
        {
            if (audio == null)
            {
                throw new System.ArgumentNullException();
            }

            if (File.Exists(audio.FilePath) != true)
            {
                throw new FileNotFoundException(audio.FilePath);
            }

            if (audioStream != null || waveout != null)
            {
                this.Dispose();
            }

            this.InitializeStream(audio.FilePath);
            this.waveout.Play();
            PlaybackState = PlaybackState.Playing;
        }

        /// <summary>
        /// リプレイ
        /// </summary>
        public void Replay()
        {
            if (PlaybackState == PlaybackState.None)
            {
                return;
            }

            if (PlaybackState == PlaybackState.Paused ||
                PlaybackState == PlaybackState.Stopped)
            {
                this.waveout.Play();
                PlaybackState = PlaybackState.Playing;
            }
        }

        /// <summary>
        /// ポーズ
        /// </summary>
        public void Pause()
        {
            if (PlaybackState == PlaybackState.None)
            {
                return;
            }

            if (PlaybackState == PlaybackState.Playing)
            {
                this.waveout.Pause();
                PlaybackState = PlaybackState.Paused;
            }
        }

        /// <summary>
        /// ストップ
        /// </summary>
        public void Stop()
        {
            if (PlaybackState == PlaybackState.None)
            {
                return;
            }

            if (PlaybackState == PlaybackState.Playing ||
                PlaybackState == PlaybackState.Paused)
            {
                this.waveout.Stop();
                this.audioStream.Position = 0;
            }

            // 破棄する
            Dispose();
            PlaybackState = PlaybackState.None;
        }

        /// <summary>
        /// シーク
        /// </summary>
        public void Seek(TimeSpan SeekTime)
        {
            if (PlaybackState == PlaybackState.None)
            {
                return;
            }

            if (PlaybackState == PlaybackState.Playing)
            {
                this.waveout.Pause();
            }

            var second = SeekTime.TotalSeconds;
            audioStream.Position = (int)((double)audioStream.WaveFormat.AverageBytesPerSecond * second);
            this.waveout.Play();
            PlaybackState = PlaybackState.Playing;
        }

        /// <summary>
        /// ボリューム取得
        /// </summary>
        /// <returns></returns>
        public float GetVolume()
        {
            if (PlaybackState == PlaybackState.None)
            {
                return 0;
            }

            return this.volumeStream.Volume;
        }

        /// <summary>
        /// ボリューム設定
        /// </summary>
        /// <param name="value"></param>
        public void SetVolume(float value)
        {
            if (PlaybackState == PlaybackState.None)
            {
                return;
            }

            this.volumeStream.Volume = value;
        }

        /// <summary>
        /// リソース解放
        /// </summary>
        public void Dispose()
        {
            if (audioStream != null)
            {
                audioStream.Dispose();
            }

            if (waveout != null)
            {
                waveout.Dispose();
            }

            waveout = null;
            audioStream = null;
        }

        /// <summary>
        /// ストリーム初期化
        /// </summary>
        /// <param name="fileName"></param>
        private void InitializeStream(string fileName)
        {
            WaveStream reader = null;

            var type = AudioFileParser.Parse(fileName);
            switch (type)
            {
                case AudioTypes.Wave:
                    reader = new WaveFileReader(fileName);
                    break;
                case AudioTypes.Mp3:
                    reader = new Mp3FileReader(fileName);
                    break;
                case AudioTypes.Flac:
                    reader = new FlacReader(fileName);
                    break;
                default:
                    throw new System.NotSupportedException();
            }

            audioStream = reader;
            this.waveout = new WaveOut();
            this.waveout.Init(this.audioStream);
            this.waveout.PlaybackStopped += new EventHandler<StoppedEventArgs>(StoppedEvent);
            PlaybackState = PlaybackState.Stopped;
        }

        /// <summary>
        /// ストップイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void StoppedEvent(object sender, StoppedEventArgs args)
        {
            PlaybackState = PlaybackState.Stopped;
            audioStream.Position = 0;
        }
    }
}
