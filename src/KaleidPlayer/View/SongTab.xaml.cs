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
    /// SongList.xaml の相互作用ロジック
    /// </summary>
    public partial class SongTab : Page
    {
        private NavigationService navi;
        private AudiosPanelView audioPanelView = new AudiosPanelView();

        public SongTab()
        {
            InitializeComponent();
            navi = this.myFrame.NavigationService;
        }

        /// <summary>
        /// ページ初期化
        /// </summary>
        public void InitPage()
        {
            // do nothing
        }

        /// <summary>
        /// フレーム読み込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myFrame_Loaded(object sender, RoutedEventArgs e)
        {
            audioPanelView.DataContext = this.DataContext;
            navi.Navigate(audioPanelView);
        }
    }
}
