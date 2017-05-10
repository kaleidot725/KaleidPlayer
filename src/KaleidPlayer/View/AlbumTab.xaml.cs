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
    /// AlbumList.xaml の相互作用ロジック
    /// </summary>
    public partial class AlbumTab : Page
    {
        private List<object> _pageList = new List<object>() { new AlbumsPanelView(), new AudioPanelView() };
        private NavigationService _navi;
        private pageIndex _currentIndex = pageIndex.ALBUM_PANEL_VIEW;

        private enum pageIndex
        {
            ALBUM_PANEL_VIEW = 0,       // アルバム一覧
            AUDIO_PANEL_VIEW = 1       // オーディオ一覧
        }

        public AlbumTab()
        {
            InitializeComponent();
            _navi = this.myFrame.NavigationService;
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

            _navi.Navigate(_pageList[(int)pageIndex.ALBUM_PANEL_VIEW]);
        }

        /// <summary>
        /// 遷移後処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (_currentIndex <= 0)
            {
                this.backwardButton.Visibility = System.Windows.Visibility.Hidden;
                this.tabText.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.backwardButton.Visibility = System.Windows.Visibility.Visible;
                this.tabText.Visibility = System.Windows.Visibility.Hidden;
            }

            return;
        }

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
            var vm = (AlbumTabViewModel)this.DataContext;

            if (currentIndex == pageIndex.ALBUM_PANEL_VIEW)
            {
                if (vm.SeletedAlbum != null)
                {
                    return true;
                }
            }

            if (currentIndex == pageIndex.AUDIO_PANEL_VIEW)
            {
                if (vm.SeletedAlbum != null)
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
            if (currentIndex == pageIndex.ALBUM_PANEL_VIEW)
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
            var vm = (AlbumTabViewModel)this.DataContext;

            if (currentIndex == pageIndex.AUDIO_PANEL_VIEW)
            {
                vm.SeletedAlbum = null;
            }
        }
    }
}
