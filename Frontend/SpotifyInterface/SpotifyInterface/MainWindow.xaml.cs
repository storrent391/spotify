using System.Windows;
using SpotifyInterface.Models;
using SpotifyInterface.Services;

using System.Net.Http.Json;
using System.Threading.Tasks;
//using mba.apiref;

namespace SpotifyInterface
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (Login.UserSession.LoggedUser != null)
            {
                UserEntrat.Content = $"Benvinguit {Login.UserSession.LoggedUser.Name}";
            }
        }

        private void OpenSongsWindow(object sender, RoutedEventArgs e)
        {
            SongManagementWindow objSongManagementWindow = new SongManagementWindow();
            this.Visibility = Visibility.Hidden;
            objSongManagementWindow.Show();
        }
    }
}
