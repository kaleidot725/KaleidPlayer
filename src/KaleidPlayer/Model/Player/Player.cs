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
        private WaveStream stream;
        private WaveChannel32 volumeStream;
        private IWavePlayer waveout;

        /// <summary>
        /// 現在時間
        /// </summary>
        public TimeSpan CurrentTime
        {
            get { return (stream != null) ? stream.CurrentTime : TimeSpan.FromSeconds(0); }
        }

        /// <summary>
        /// 合計時間
        /// </summary>
        public TimeSpan TotalTime
        {
            get { return (stream != null) ? stream.TotalTime : TimeSpan.FromSeconds(0); }
        }

        /// <summary>
        /// 前再生状態
        /// </summary>
        private PalybackTypes prePlaybackState;
        public PalybackTypes PrePlaybackState
        {
            get { return prePlaybackState; }
        }

        /// <summary>
        /// 再生状態
        /// </summary>
        private PalybackTypes playbackState;
        public PalybackTypes PlaybackState
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

            if (stream != null || waveout != null)
            {
                this.Dispose();
            }

            this.InitializeStream(audio.FilePath);
            this.waveout.Play();
            PlaybackState = PalybackTypes.Playing;
        }

        /// <summary>
        /// リプレイ
        /// </summary>
        public void Replay()
        {
            if (PlaybackState == PalybackTypes.None)
            {
                return;
            }

            if (PlaybackState == PalybackTypes.Paused ||
                PlaybackState == PalybackTypes.Stopped)
            {
                this.waveout.Play();
                PlaybackState = PalybackTypes.Playing;
            }
        }

        /// <summary>
        /// ポーズ
        /// </summary>
        public void Pause()
        {
            if (PlaybackState == PalybackTypes.None)
            {
                return;
            }

            if (PlaybackState == PalybackTypes.Playing)
            {
                this.waveout.Pause();
                PlaybackState = PalybackTypes.Paused;
            }
        }

        /// <summary>
        /// ストップ
        /// </summary>
        public void Stop()
        {
            if (PlaybackState == PalybackTypes.None)
            {
                return;
            }

            if (PlaybackState == PalybackTypes.Playing ||
                PlaybackState == PalybackTypes.Paused)
            {
                this.waveout.Stop();
                this.stream.Position = 0;
            }

            // 破棄する
            Dispose();
            PlaybackState = PalybackTypes.None;
        }

        /// <summary>
        /// シーク
        /// </summary>
        public void Seek(TimeSpan SeekTime)
        {
            if (PlaybackState == PalybackTypes.None)
            {
                return;
            }

            if (PlaybackState == PalybackTypes.Playing)
            {
                this.waveout.Pause();
            }

            var second = SeekTime.TotalSeconds;
            stream.Position = (int)((double)stream.WaveFormat.AverageBytesPerSecond * second);
            this.waveout.Play();
            PlaybackState = PalybackTypes.Playing;
        }

        /// <summary>
        /// ボリューム取得
        /// </summary>
        /// <returns></returns>
        public float GetVolume()
        {
            if (PlaybackState == PalybackTypes.None)
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
            if (PlaybackState == PalybackTypes.None)
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
            if (stream != null)
            {
                stream.Dispose();
            }

            if (waveout != null)
            {
                waveout.Dispose();
            }

            waveout = null;
            stream = null;
        }

        /// <summary>
        /// ストリーム初期化
        /// </summary>
        /// <param name="fileName"></param>
        private void InitializeStream(string fileName)
        {
            this.stream = WaveStreamFactory.CreateWaveStream(fileName);
            this.waveout = new WaveOut();
            this.waveout.Init(this.stream);
            this.waveout.PlaybackStopped += new EventHandler<StoppedEventArgs>(StoppedEvent);
            this.PlaybackState = PalybackTypes.Stopped;
        }

        /// <summary>
        /// ストップイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void StoppedEvent(object sender, StoppedEventArgs args)
        {
            PlaybackState = PalybackTypes.Stopped;
            stream.Position = 0;
        }
    }
}
