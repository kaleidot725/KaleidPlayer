using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaleidot725.Model
{
    public interface IAudioDetail
    {
        string Title { get; set; }
        string Artist { get; set; }
        string Album { get; set; }
        string Date { get; set; }
        string TrackNo { get; set; }
        string Genle { get; set; }
        string Comment { get; set; }
        string AlbumArtist { get; set; }
        string Composer { get; set; }
        string DiscNumber { get; set; }
        string FilePath { get; set; }

        void Parse();
    }
}
