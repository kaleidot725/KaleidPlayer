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

namespace kaleidot725.View
{
    /// <summary>
    /// AlbumList.xaml の相互作用ロジック
    /// </summary>
    public partial class AlbumTab : UserControl
    {
        private NavigationService _navi;
        private List<object> _pageList = new List<object>() { new AlbumsPanelView(), new AudioPanelView() };
        private int currentIndex = 0;

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
            if (currentIndex <= 0)
            {
                this.forwardButton.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                this.forwardButton.Visibility = System.Windows.Visibility.Visible;
            }

            return;
        }

        private void myFrame_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_navi.CanGoForward)
            {
                _navi.GoForward();
            }
            else
            {
                if (currentIndex != 1)
                {
                    currentIndex++;
                    _navi.Navigate(_pageList[currentIndex]);
                }
            }
        }

        private void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (_navi.CanGoForward)
            {
                _navi.GoForward();
            }
            else
            {
                if (currentIndex != 0)
                {
                    currentIndex--;
                    _navi.Navigate(_pageList[currentIndex]);
                }
            }
        }
    }
}
