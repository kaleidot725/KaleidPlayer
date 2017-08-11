using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaleidot725.Model
{
    public enum PalybackTypes
    {
        Playing = NAudio.Wave.PlaybackState.Playing,
        Paused = NAudio.Wave.PlaybackState.Paused,
        Stopped = NAudio.Wave.PlaybackState.Stopped,
        None = -1,
    }
}
