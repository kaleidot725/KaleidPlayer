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
        private List<object> pageList = new List<object>() { new ArtistPanelView(), new AlbumPanelView(), new AudioPanelView() };
        private NavigationService navi;
        private pageIndex curentIndex = pageIndex.ArtistPanelView;

        /// <summary>
        /// ページインデックス
        /// </summary>
        private enum pageIndex
        {
            ArtistPanelView = 0,      // アーティスト一覧
            AlbumPanelView = 1,       // アルバム一覧
            AudioPanelView = 2        // オーディオ一覧
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ArtistTab()
        {
            InitializeComponent();
            navi = this.myFrame.NavigationService;
        }

        /// <summary>
        /// ページ初期化
        /// </summary>
        public void InitPage()
        {
            curentIndex = pageIndex.ArtistPanelView;
            navi.Navigate(pageList[(int)curentIndex]);
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

            navi.Navigate(pageList[(int)pageIndex.ArtistPanelView]);
        }

        /// <summary>
        /// 遷移後処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (isBackwardable(curentIndex))
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
            if (navi.CanGoForward)
            {
                navi.GoForward();
            }
            else
            {
                if (isForwardable(curentIndex))
                {
                    curentIndex++;
                    navi.Navigate(pageList[(int)curentIndex]);
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
            if (navi.CanGoForward)
            {
                navi.GoForward();
            }
            else
            {
                if (isBackwardable(curentIndex))
                {
                    initSelectedItem(curentIndex);
                    curentIndex--;
                    navi.Navigate(pageList[(int)curentIndex]);
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

            if (currentIndex == pageIndex.ArtistPanelView)
            {
                if (vm.SelectedArtist != null)
                {
                    return true;
                }
            }

            if (currentIndex == pageIndex.AlbumPanelView)
            {
                if (vm.SelectedArtist != null && vm.SeletedAlbum != null)
                {
                    return true;
                }
            }

            if (currentIndex == pageIndex.AudioPanelView)
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
            if (currentIndex == pageIndex.ArtistPanelView)
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

            if (currentIndex == pageIndex.ArtistPanelView)
            {
                return;
            }

            if (currentIndex == pageIndex.AlbumPanelView)
            {
                vm.SelectedArtist = null;
            }

            if (currentIndex == pageIndex.AudioPanelView)
            {
                vm.SeletedAlbum = null;
            }
        }
    }
}
