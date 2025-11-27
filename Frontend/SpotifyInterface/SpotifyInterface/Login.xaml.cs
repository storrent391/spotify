using SpotifyInterface.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace SpotifyInterface
{
    public partial class Login : Window
    {
        private readonly HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5000")
        };

        public Login()
        {
            InitializeComponent();
        }

        // Mostrar / ocultar placeholder del TextBox
        private void UsernameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UsernamePlaceholder.Visibility =
                string.IsNullOrEmpty(UsernameTextBox.Text)
                ? Visibility.Visible
                : Visibility.Hidden;
        }
        //private void LoginButton_Click(object sender, RoutedEventArgs e)
        //{
        //    // Saltar login y abrir MainWindow directamente
        //    var main = new MainWindow();
        //    main.Show();
        //    this.Close();
        //}
        public static class UserSession
        {
            public static User? LoggedUser { get; set; }
        }
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Crear objeto User para enviar
            User loginUser = new User
            {
                Name = UsernameTextBox.Text,
                Password = PasswordBox.Password,
                Salt = "" 
            };

            try
            {
                HttpResponseMessage response =
                    await _http.PostAsJsonAsync<User>("/login", loginUser);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Usuario o contraseña incorrectos", "Error");
                    return;
                }

                // Leer el usuario devuelto por la API
                User? loggedUser =
                    await response.Content.ReadFromJsonAsync<User>();

                if (loggedUser == null)
                {
                    MessageBox.Show("Respuesta inválida del servidor", "Error");
                    return;
                }

                // Continuar a la ventana principal
                UserSession.LoggedUser = loggedUser;
                new MainWindow().Show();
                Close();
            }
            catch
            {
                MessageBox.Show("No se puede conectar con la API", "Error");
            }
        }

    }
}
