using Main.Models;
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

namespace Main.Pages
{
    /// <summary>
    /// Interaction logic for UsersOverviewPage.xaml
    /// </summary>
    public partial class UsersOverviewPage : Page
    {
        private List<User> users;
        public UsersOverviewPage()
        {
            InitializeComponent();
            LoadUsers();
        }
        private void LoadUsers()
        {
            // Temporary array of users for demonstration
            users = new List<User>
            {
                new User { Id = 1, Name = "John Doe", Password = "johnpass123", Score = "85" },
                new User { Id = 2, Name = "Alice Smith", Password = "alicepass456", Score = "null" },
                new User { Id = 3, Name = "Bob Johnson", Password = "bobpass789", Score = "92" },
                new User { Id = 4, Name = "Emma Wilson", Password = "emmapass101", Score = "78" },
                new User { Id = 5, Name = "Michael Brown", Password = "michaelpass202", Score = "null" }
            };

            // Bind the users to the DataGrid
            UsersDataGrid.ItemsSource = users;
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the UserAdd.xaml page (assuming it exists)
            //this.NavigationService.Navigate(new Uri("User.xaml", UriKind.Relative));
            LoginRegisterWindow register = new LoginRegisterWindow(false);
            register.Title = "AddUser";
            register.ShowDialog();
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            // Check if a user is selected
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                // Confirm deletion
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete user '{selectedUser.Name}' (ID: {selectedUser.Id})?",
                    "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Remove the user from the list
                    users.Remove(selectedUser);

                    // Refresh the DataGrid
                    UsersDataGrid.ItemsSource = null;
                    UsersDataGrid.ItemsSource = users;

                    MessageBox.Show($"User '{selectedUser.Name}' deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
