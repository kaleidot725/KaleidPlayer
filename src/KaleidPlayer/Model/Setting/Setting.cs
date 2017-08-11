using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace kaleidot725.Model
{
    public class Setting :BindableBase
    {
        private ObservableCollection<string> libraryDirectories;
        public ObservableCollection<string> LibraryDirectories
        {
            get { return libraryDirectories; }
            set { SetProperty(ref libraryDirectories, value); }
        }

        public Setting()
        {
            libraryDirectories = new ObservableCollection<string>();
        }
    }
}
