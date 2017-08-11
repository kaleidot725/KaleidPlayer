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
using System.Windows.Threading;

namespace kaleidot725.ViewModel
{
    class SettingTabViewModel : BindableBase
    {
        private Player player;
        private Searcher searcher;
        private AudioLibrary library;
        private Playlist playlist;
        private Setting setting;
        private Dispatcher uiDispatcher;

        public ReactiveProperty<ObservableCollection<string>> Directories { get; private set; }
        public DelegateCommand AddPathCommand { get; }
        public DelegateCommand DeletePathCommand { get; }
        public DelegateCommand UpdateLibraryCommand { get; }

        private string selectedPath;
        public string SelectedPath
        {
            get { return selectedPath; }
            set { SetProperty(ref selectedPath, value); }
        }

        public SettingTabViewModel()
        {
            // ライブラリ群
            player = SingletonModels.GetAudioPlayerInstance();
            searcher = SingletonModels.GetAudioSearcherInstance();
            library = SingletonModels.GetArtistListInstance();
            playlist = SingletonModels.GetAudioPlaylist();
            setting = SingletonModels.GetApplicationSetting();
            uiDispatcher = Dispatcher.CurrentDispatcher;
            setting = SingletonModels.GetApplicationSetting();

            Directories = setting.ToReactivePropertyAsSynchronized(m => m.LibraryDirectories).ToReactiveProperty();

            AddPathCommand = new DelegateCommand(AddPath);
            DeletePathCommand = new DelegateCommand(DeletePath);
            UpdateLibraryCommand = new DelegateCommand(AsyncCreateLibrary);
            Directories.Value.Add("C:\\Users\\K-Y\\Music");
        }

        public void AddPath()
        {
            var browser = new FolderBrowserDialog();
            browser.RootFolder = System.Environment.SpecialFolder.MyComputer;
            if (browser.ShowDialog() == DialogResult.OK)
            {
                Directories.Value.Add(browser.SelectedPath);
            }
        }

        public void DeletePath()
        {
            if (selectedPath == null)
            {
                return;
            }

            try
            {
                Directories.Value.Remove(selectedPath);
            }
            catch (ArgumentException)
            {
                System.Windows.MessageBox.Show("Remove Error");
            }
        }

        /// <summary>
        /// ライブラリ作成
        /// </summary>
        private async void AsyncCreateLibrary()
        {
            ObservableCollection<IAudioDetail> audios = null;

            library.Delete();
            
            await Task.Run(() =>
            {
                audios = searcher.SearchFolder(setting.LibraryDirectories);
            });

            library.Create(audios.ToList());

            var convAudios = new ObservableCollection<AudioDetailSerializable>();
            foreach (var i in audios)
            {
                convAudios.Add(new AudioDetailSerializable(i));
            }
            AudioSerializer.Serialize(System.IO.Directory.GetCurrentDirectory() + "\\meta", convAudios);
        }
    }
}
