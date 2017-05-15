using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using kaleidot725.Model;
using kaleidot725.Model.Library;
using System.Collections.ObjectModel;
using Reactive.Bindings;
using System.IO;
using Reactive.Bindings.Extensions;
using System.Windows.Forms;

namespace kaleidot725.ViewModel
{
    class SettingTabViewModel : BindableBase
    {
        private ApplicationSetting _setting;

        private string _selectedPath;
        public string SelectedPath
        {
            get { return _selectedPath; }
            set { SetProperty(ref _selectedPath, value); }
        }

        public ReactiveProperty<ObservableCollection<string>> LibraryPath { get; private set; }
        public DelegateCommand AddPathCommand { get;}
        public DelegateCommand DeletePathCommand { get; }

        public SettingTabViewModel()
        {
            _setting = SingletonModels.GetApplicationSetting();
            LibraryPath = _setting.ToReactivePropertyAsSynchronized(m => m.LibraryFolder).ToReactiveProperty();

            AddPathCommand = new DelegateCommand(AddPath);
            DeletePathCommand = new DelegateCommand(DeletePath);

            LibraryPath.Value.Add("C:\\Users\\K-Y\\Music\\Egoist");
        }

        public void AddPath()
        {
            var browser = new FolderBrowserDialog();
            browser.RootFolder = System.Environment.SpecialFolder.MyComputer;
            if (browser.ShowDialog() == DialogResult.OK)
            {
                LibraryPath.Value.Add(browser.SelectedPath);
            } 
        }

        public void DeletePath()
        {
            if (_selectedPath == null)
            {
                return;
            }

            try
            {
                LibraryPath.Value.Remove(_selectedPath);
            }
            catch (ArgumentException e)
            {
                System.Windows.MessageBox.Show("Remove Error");
            }
        }
    }
}
