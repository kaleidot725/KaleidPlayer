using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace kaleidot725.Model
{
    public class ApplicationSetting :BindableBase
    {
        private ObservableCollection<string> _libraryFolder;
        public ObservableCollection<string> LibraryFolder
        {
            get { return _libraryFolder; }
            set { SetProperty(ref _libraryFolder, value); }
        }

        public ApplicationSetting()
        {
            _libraryFolder = new ObservableCollection<string>();
        }
    }
}
