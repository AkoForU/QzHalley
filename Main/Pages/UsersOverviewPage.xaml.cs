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
        QuizDbContext _context;
        public UsersOverviewPage()
        {
            InitializeComponent();
            _context = new QuizDbContext();
            LoadUsers();
        }
        private void LoadUsers()
        {
            UsersDataGrid.ItemsSource = _context.Users.ToArray();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the UserAdd.xaml page (assuming it exists)
            //this.NavigationService.Navigate(new Uri("User.xaml", UriKind.Relative));
            LoginRegisterWindow register = new LoginRegisterWindow(false);
            register.Title = "AddUser";
            register.ShowDialog();
            LoadUsers();
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
                    _context.Users.Remove(selectedUser);
                    _context.SaveChanges();
                    // Refresh the DataGrid
                    UsersDataGrid.ItemsSource = null;
                    UsersDataGrid.ItemsSource = _context.Users.ToList();

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
