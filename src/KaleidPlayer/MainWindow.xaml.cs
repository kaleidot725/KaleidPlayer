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
using kaleidot725.View;

namespace kaleidot725
{

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        private NavigationService _navi;
        private ArtistTab _artistTab = new ArtistTab();
        private AlbumTab _albumTab = new AlbumTab();
        private SongTab _songTab = new SongTab();
        private SettingTab _settingTab = new SettingTab();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _navi = myFrame.NavigationService;
        }

        /// <summary>
        /// フレーム読み込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myFrame_Loaded(object sender, RoutedEventArgs e)
        {
            ArtistBorder.Visibility = Visibility.Visible;
            AlbumBorder.Visibility = Visibility.Hidden;
            SongBorder.Visibility = Visibility.Hidden;
            SettingBorder.Visibility = Visibility.Hidden;
            _navi.Navigate(_artistTab);
        }

        /// <summary>
        /// 遷移後処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myFrame_Navigated(object sender, NavigationEventArgs e)
        {
            return;
        }

        /// <summary>
        /// アーティスト選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArtistButton_Click(object sender, RoutedEventArgs e)
        {
            ArtistBorder.Visibility = Visibility.Visible;
            AlbumBorder.Visibility = Visibility.Hidden;
            SongBorder.Visibility = Visibility.Hidden;
            SettingBorder.Visibility = Visibility.Hidden;

            _artistTab.InitPage();
            _navi.Navigate(_artistTab);
        }

        /// <summary>
        /// アルバム選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlbumButton_Click(object sender, RoutedEventArgs e)
        {
            ArtistBorder.Visibility = Visibility.Hidden;
            AlbumBorder.Visibility = Visibility.Visible;
            SongBorder.Visibility = Visibility.Hidden;
            SettingBorder.Visibility = Visibility.Hidden;

            _albumTab.InitPage();
            _navi.Navigate(_albumTab);
        }

        /// <summary>
        /// オーディオ選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AudioButton_Click(object sender, RoutedEventArgs e)
        {
            ArtistBorder.Visibility = Visibility.Hidden;
            AlbumBorder.Visibility = Visibility.Hidden;
            SongBorder.Visibility = Visibility.Visible;
            SettingBorder.Visibility = Visibility.Hidden;

            _songTab.InitPage();
            _navi.Navigate(_songTab);
        }

        /// <summary>
        /// 設定選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            ArtistBorder.Visibility = Visibility.Hidden;
            AlbumBorder.Visibility = Visibility.Hidden;
            SongBorder.Visibility = Visibility.Hidden;
            SettingBorder.Visibility = Visibility.Visible;

            _navi.Navigate(_settingTab);
        }
    }
}
