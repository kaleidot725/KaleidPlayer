﻿using System;
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

        private string _selectedPath;
        public string SelectedPath
        {
            get { return _selectedPath; }
            set { SetProperty(ref _selectedPath, value); }
        }

        public ReactiveProperty<ObservableCollection<string>> LibraryPath { get; private set; }
        public DelegateCommand AddPathCommand { get;}
        public DelegateCommand DeletePathCommand { get; }
        public DelegateCommand UpdateLibraryCommand { get; }


        private AudioPlayer _audioPlayer;
        private SongSearcher _songsSearcher;
        private ArtistList _artistList;
        private AlbumList _albumList;
        private AudioPlaylist _playlist;
        private ApplicationSetting _setting;
        private Dispatcher _uiDispatcher;

        public SettingTabViewModel()
        {
            // ライブラリ群
            _audioPlayer = SingletonModels.GetAudioPlayerInstance();
            _songsSearcher = SingletonModels.GetAudioSearcherInstance();
            _artistList = SingletonModels.GetArtistListInstance();
            _albumList = SingletonModels.GetAlbumListInstance();
            _playlist = SingletonModels.GetAudioPlaylist();
            _setting = SingletonModels.GetApplicationSetting();
            _uiDispatcher = Dispatcher.CurrentDispatcher;
            _setting = SingletonModels.GetApplicationSetting();

            LibraryPath = _setting.ToReactivePropertyAsSynchronized(m => m.LibraryFolder).ToReactiveProperty();

            AddPathCommand = new DelegateCommand(AddPath);
            DeletePathCommand = new DelegateCommand(DeletePath);
            UpdateLibraryCommand = new DelegateCommand(AsyncCreateLibrary);

            LibraryPath.Value.Add("C:\\Users\\K-Y\\Music");
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

        /// <summary>
        /// ライブラリ作成
        /// </summary>
        private async void AsyncCreateLibrary()
        {
            _songsSearcher.ClearSong();
            _songsSearcher.ClearFolder();
            _artistList.Clear();
            _albumList.Clear();

            await Task.Run(() =>
            {
                foreach (var path in _setting.LibraryFolder)
                {
                    _songsSearcher.AddFolder(path);
                    _songsSearcher.SearchFolder();
                }
            });

            _artistList.Create(_songsSearcher.Song.ToList());
            _albumList.Create(_songsSearcher.Song.ToList());

            try
            {
                var collection = new ObservableCollection<AudioSerialzerData>();
                foreach (var song in _songsSearcher.Song)
                {
                    collection.Add(new AudioSerialzerData(song));
                }

                SongSerializer.Serialize(System.IO.Directory.GetCurrentDirectory() + "\\meta", collection);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
