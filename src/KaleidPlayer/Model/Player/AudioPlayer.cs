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
    public class AudioPlayer : BindableBase, IDisposable
    {
        private AudioDetailBase _currentAudio;
        private WaveStream _audioStream;
        private WaveChannel32 _volumeStream;
        private IWavePlayer _waveOut;
        private IDisposable _timerSubscrition;

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
        /// プレイ
        /// </summary>
        public void Play(AudioDetailBase audio)
        {
            if (_audioStream != null || _waveOut != null)
            {
                this.Dispose();
            }

            if (File.Exists(audio.FilePath) != true)
            {
                throw new FileNotFoundException(audio.FilePath);
            }

            this._currentAudio = audio;
            this.InitializeStream(_currentAudio.FilePath);
            this._waveOut.Play();
        }

        /// <summary>
        /// リプレイ
        /// </summary>
        public void Replay()
        {
            if (_audioStream == null || _waveOut == null)
            {
                throw new NullReferenceException();
            }

            if (_waveOut.PlaybackState == PlaybackState.Paused)
            {
                this._waveOut.Play();
            }
        }

        /// <summary>
        /// ポーズ
        /// </summary>
        public void Pause()
        {
            if (_audioStream == null || _waveOut == null)
            {
                throw new NullReferenceException();
            }

            if (_waveOut.PlaybackState == PlaybackState.Playing)
            {
                this._waveOut.Pause();
            }
        }

        /// <summary>
        /// ストップ
        /// </summary>
        public void Stop()
        {
            if (_audioStream == null || _waveOut == null)
            {
                throw new NullReferenceException();
            }

            if (_waveOut.PlaybackState != PlaybackState.Stopped)
            {
                this._waveOut.Stop();
                this._audioStream.Position = 0;
            }

            // 破棄する
            Dispose();
        }

        /// <summary>
        /// シーク
        /// </summary>
        public void Seek(TimeSpan SeekTime)
        {
            if (_audioStream == null || _waveOut == null)
            {
                throw new NullReferenceException();
            }

            var second = SeekTime.TotalSeconds;
            _audioStream.Position = (int)((double)_audioStream.WaveFormat.AverageBytesPerSecond * second);
        }

        /// <summary>
        /// ボリューム取得
        /// </summary>
        /// <returns></returns>
        public float GetVolume()
        {
            if (_audioStream == null || _waveOut == null)
            {
                throw new NullReferenceException();
            }

            return this._volumeStream.Volume;
        }

        /// <summary>
        /// ボリューム設定
        /// </summary>
        /// <param name="value"></param>
        public void SetVolume(float value)
        {
            if (_audioStream == null || _waveOut == null)
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
        }

    }
}
