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
    public partial class ArtistTab : UserControl
    {
        private NavigationService _navi;
        private List<object> _pageList = new List<object>(){ new ArtistPanelView(), new AlbumPanelView(), new AudioPanelView() };
        private int currentIndex = 0;

        private enum pageIndex {
            ARITST_PANEL_VIEW = 0,      // アーティスト一覧
            ALBUM_PANEL_VIEW = 1,       // アルバム一覧
            AUDIO_PANEL_VIEW = 2        // オーディオ一覧
        }

        public ArtistTab()
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

            _navi.Navigate(_pageList[(int)pageIndex.ARITST_PANEL_VIEW]);
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
                if (currentIndex != 2)
                {
                    currentIndex++;
                    _navi.Navigate(_pageList[currentIndex]);
                }
            }

            //MessageBox.Show("myframe MouseDoubleClick");
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
