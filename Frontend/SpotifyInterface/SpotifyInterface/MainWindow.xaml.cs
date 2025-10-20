using System.Windows;
using SpotifyInterface.Models;
using SpotifyInterface.Services;

namespace SpotifyInterface
{
    public partial class MainWindow : Window
    {
        private readonly UserService _userService;

        public MainWindow()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        // Buscar ID 
        private async void BuscarUser_Click(object sender, RoutedEventArgs e)
        {
            Guid userId = Guid.Parse(TXTID.Text);

            User user = await _userService.GetByIdAsync(userId);

            TXTID.Text = user.Id.ToString();
            TXTNom.Text = user.Name;
            TXTPassword.Text = user.Password;
            TXTSalt.Text = user.Salt;
        }

        // Obtenir usaris
        private async void GetAllUsers_Click(object sender, RoutedEventArgs e)
        {
            List<User> users = await _userService.GetAllAsync();

            ListaUsers.Items.Clear();

            foreach (User user in users)
            {
                string userInfo = $"ID: {user.Id} | Nom: {user.Name} | Password: {user.Password} | Salt: {user.Salt}";
                ListaUsers.Items.Add(userInfo);
            }
        }
    }
}
