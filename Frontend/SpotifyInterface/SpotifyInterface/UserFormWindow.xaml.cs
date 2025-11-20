using System.Windows;
using SpotifyInterface.Models;

namespace SpotifyInterface
{
    /// <summary>
    /// Lógica de interacción para UserFormWindow.xaml
    /// </summary>
    public partial class UserFormWindow : Window
    {
        public User UserData { get; private set; }

        // Constructor opcional: si rebem un usuari existent, editem; si no, creem
        public UserFormWindow(User existingUser = null)
        {
            InitializeComponent();

            if (existingUser != null)
            {
                UserData = existingUser;
                NameBox.Text = existingUser.Name;
                PasswordBox.Password = existingUser.Password;
            }
            else
            {
                UserData = new User();
            }
        }

        // Botó Guardar
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("El nom no pot estar buit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            UserData.Name = NameBox.Text;
            UserData.Password = PasswordBox.Password;

            DialogResult = true; // Tanca la finestra i retorna true
        }
    }
}
