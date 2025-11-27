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
using System.Windows.Shapes;

using System.Net.Http.Json;
using System.Threading.Tasks;
//using mba.apiref;

namespace SpotifyInterface
{
    /// <summary>
    /// Interaction logic for SongManagementWindow.xaml
    /// </summary>
    public partial class SongManagementWindow : Window
    {
        public SongManagementWindow()
        {
            InitializeComponent();
        }
        private void OpenMainMenu(object sender, RoutedEventArgs e)
        {
            MainWindow objMainWindow = new MainWindow();
            this.Visibility = Visibility.Hidden;
            objMainWindow.Show();
        }
    }
}
