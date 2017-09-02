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
        private List<object> pageList = new List<object>() { new AlbumsPanelView(), new AudioPanelView() };
        private NavigationService navi;
        private pageIndex currentIndex = pageIndex.AlbumPanelView;

        private enum pageIndex
        {
            AlbumPanelView = 0,      // アルバム一覧
            AudioPanelView = 1       // オーディオ一覧
        }

        public AlbumTab()
        {
            InitializeComponent();
            navi = this.myFrame.NavigationService;
        }

        /// <summary>
        /// ページ初期化
        /// </summary>
        public void InitPage()
        {
            currentIndex = pageIndex.AlbumPanelView;
            navi.Navigate(pageList[(int)currentIndex]);
        }

        /// <summary>
        /// フレーム読み込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myFrame_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Page item in pageList)
            {
                item.DataContext = this.DataContext;
            }

            navi.Navigate(pageList[(int)pageIndex.AlbumPanelView]);
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
                this.backwardButton.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                this.backwardButton.Visibility = System.Windows.Visibility.Visible;
            }

            return;
        }

        private void myFrame_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (navi.CanGoForward)
            {
                navi.GoForward();
            }
            else
            {
                if (isForwardable(currentIndex))
                {
                    currentIndex++;
                    navi.Navigate(pageList[(int)currentIndex]);
                }
            }
        }

        private void backwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (navi.CanGoForward)
            {
                navi.GoForward();
            }
            else
            {
                if (isBackwardable(currentIndex))
                {
                    initSelectedItem(currentIndex);
                    currentIndex--;
                    navi.Navigate(pageList[(int)currentIndex]);
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

            if (currentIndex == pageIndex.AlbumPanelView)
            {
                if (vm.SeletedAlbum != null)
                {
                    return true;
                }
            }

            if (currentIndex == pageIndex.AudioPanelView)
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
            if (currentIndex == pageIndex.AlbumPanelView)
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

            if (currentIndex == pageIndex.AudioPanelView)
            {
                vm.SeletedAlbum = null;
            }
        }
    }
}
