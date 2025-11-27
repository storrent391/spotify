using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using SpotifyInterface.Models;

namespace SpotifyInterface
{
    public partial class AdminUsers : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public List<User> Users = new List<User>();
        private const string ApiBaseUrl = "https://localhost:5001"; // Canvia segons la teva API

        public AdminUsers()
        {
            InitializeComponent();
            _ = LoadUsersAsync(); // carreguem la llista inicial
        }

        // Carregar usuaris de l'API
        private async Task LoadUsersAsync()
        {
            try
            {
                var usersFromApi = await _httpClient.GetFromJsonAsync<List<User>>($"{ApiBaseUrl}/users");
                if (usersFromApi != null)
                {
                    Users = usersFromApi;
                    UsersList.ItemsSource = null;
                    UsersList.ItemsSource = Users;
                    UsersList.DisplayMemberPath = "Name";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error carregant usuaris: " + ex.Message);
            }
        }

        // Crear usuari
        private async void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            var form = new UserFormWindow();
            if (form.ShowDialog() == true)
            {
                try
                {
                    var req = new UserRequest
                    {
                        Name = form.UserData.Name,
                        Password = form.UserData.Password
                        // Id i Salt els genera l'API
                    };

                    var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/user", req);
                    if (response.IsSuccessStatusCode)
                    {
                        var createdUser = await response.Content.ReadFromJsonAsync<User>();
                        if (createdUser != null)
                        {
                            Users.Add(createdUser);
                            UsersList.ItemsSource = null;
                            UsersList.ItemsSource = Users;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error creant l'usuari.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Editar usuari
        private async void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersList.SelectedItem is not User selectedUser)
            {
                MessageBox.Show("Selecciona un usuari per editar.", "Atenció", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var form = new UserFormWindow(selectedUser);
            if (form.ShowDialog() == true)
            {
                try
                {
                    var req = new UserRequest
                    {
                        Id = selectedUser.Id,
                        Name = form.UserData.Name,
                        Password = form.UserData.Password
                    };

                    var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/user/{selectedUser.Id}", req);
                    if (response.IsSuccessStatusCode)
                    {
                        var updatedUser = await response.Content.ReadFromJsonAsync<User>();
                        if (updatedUser != null)
                        {
                            var index = Users.FindIndex(u => u.Id == updatedUser.Id);
                            if (index >= 0)
                                Users[index] = updatedUser;

                            UsersList.ItemsSource = null;
                            UsersList.ItemsSource = Users;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error actualitzant l'usuari.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Eliminar usuari
        private async void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersList.SelectedItem is not User selectedUser)
            {
                MessageBox.Show("Selecciona un usuari per eliminar.", "Atenció", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Segur que vols eliminar l'usuari '{selectedUser.Name}'?",
                                         "Confirmar eliminació", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/user/{selectedUser.Id}");
                if (response.IsSuccessStatusCode)
                {
                    Users.Remove(selectedUser);
                    UsersList.ItemsSource = null;
                    UsersList.ItemsSource = Users;
                }
                else
                {
                    MessageBox.Show("Error eliminant l'usuari.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
