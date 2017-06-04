using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using kaleidot725.ViewModel;

namespace kaleidot725.View
{
    /// <summary>
    /// ArtistList.xaml の相互作用ロジック
    /// </summary>
    public partial class ArtistTab : Page
    {
        private List<object> _pageList = new List<object>() { new ArtistPanelView(), new AlbumPanelView(), new AudioPanelView() };
        private NavigationService _navi;
        private pageIndex _currentIndex = pageIndex.ARITST_PANEL_VIEW;

        /// <summary>
        /// ページインデックス
        /// </summary>
        private enum pageIndex
        {
            ARITST_PANEL_VIEW = 0,      // アーティスト一覧
            ALBUM_PANEL_VIEW = 1,       // アルバム一覧
            AUDIO_PANEL_VIEW = 2        // オーディオ一覧
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ArtistTab()
        {
            InitializeComponent();
            _navi = this.myFrame.NavigationService;
        }

        /// <summary>
        /// ページ初期化
        /// </summary>
        public void InitPage()
        {
            _currentIndex = pageIndex.ARITST_PANEL_VIEW;
            _navi.Navigate(_pageList[(int)_currentIndex]);
        }

        /// <summary>
        /// フレーム読み込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myFrame_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Page item in _pageList)
            {
                item.DataContext = this.DataContext;
            }

            _navi.Navigate(_pageList[(int)pageIndex.ARITST_PANEL_VIEW]);
        }

        /// <summary>
        /// 遷移後処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (isBackwardable(_currentIndex))
            {
                this.backwardButton.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.backwardButton.Visibility = System.Windows.Visibility.Hidden;
            }

            return;
        }

        /// <summary>
        /// 次ページに遷移する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myFrame_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_navi.CanGoForward)
            {
                _navi.GoForward();
            }
            else
            {
                if (isForwardable(_currentIndex))
                {
                    _currentIndex++;
                    _navi.Navigate(_pageList[(int)_currentIndex]);
                }
            }
        }

        /// <summary>
        /// 前ページに遷移する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (_navi.CanGoForward)
            {
                _navi.GoForward();
            }
            else
            {
                if (isBackwardable(_currentIndex))
                {
                    initSelectedItem(_currentIndex);
                    _currentIndex--;
                    _navi.Navigate(_pageList[(int)_currentIndex]);
                }
            }
        }

        /// <summary>
        /// 遷移可能状態
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        private bool isForwardable(pageIndex currentIndex)
        {
            var vm = (ArtistTabViewModel)this.DataContext;

            if (currentIndex == pageIndex.ARITST_PANEL_VIEW)
            {
                if (vm.SelectedArtist != null)
                {
                    return true;
                }
            }

            if (currentIndex == pageIndex.ALBUM_PANEL_VIEW)
            {
                if (vm.SelectedArtist != null && vm.SeletedAlbum != null)
                {
                    return true;
                }
            }

            if (currentIndex == pageIndex.AUDIO_PANEL_VIEW)
            {
                if (vm.SelectedArtist != null && vm.SeletedAlbum != null)
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// 前ページ移動可能状態
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        private bool isBackwardable(pageIndex currentIndex)
        {
            if (currentIndex == pageIndex.ARITST_PANEL_VIEW)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ViewModel状態初期化
        /// </summary>
        private void initSelectedItem(pageIndex currentIndex)
        {
            var vm = (ArtistTabViewModel)this.DataContext;

            if (currentIndex == pageIndex.ARITST_PANEL_VIEW)
            {
                return;
            }

            if (currentIndex == pageIndex.ALBUM_PANEL_VIEW)
            {
                vm.SelectedArtist = null;
            }

            if (currentIndex == pageIndex.AUDIO_PANEL_VIEW)
            {
                vm.SeletedAlbum = null;
            }
        }
    }
}
