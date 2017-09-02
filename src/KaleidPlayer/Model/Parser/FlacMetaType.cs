using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaleidot725.Model
{
    public enum FlacMetaType
    {
        StreamInfo = 0,
        Padding = 1,
        Application = 2,
        SeekTable = 3,
        VorbisComment = 4,
        CueSheet = 5,
        Picture = 6
    }
}
