using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaleidot725.Model
{
    public interface IAlbum
    {
        string Name { get; set; }
        ObservableCollection<IAudioDetail> Tracks { get; set; }
    }
}
