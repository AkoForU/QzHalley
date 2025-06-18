using Main.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Main.Pages
{
    /// <summary>
    /// Interaction logic for UserStatusPage.xaml
    /// </summary>
    public partial class UserStatusPage : Page
    {
        private ConsoleOutput console = new ConsoleOutput();
        private readonly QuizDbContext _dbContext;
        private Process _serverProcess;
        private bool _isServerRunning;
        private ConsoleOutput _outputWindow;

        public UserStatusPage()
        {
            InitializeComponent();
            _dbContext = new QuizDbContext();
            LoadUsers();
            UpdateGrid();
        }

        private void LoadUsers()
        {
            var users = _dbContext.Users.Select(u => new { u.Id, u.Name }).ToList();
            UserFilterComboBox.ItemsSource = users;
            UserFilterComboBox.DisplayMemberPath = "Name";
            UserFilterComboBox.SelectedValuePath = "Id";
            UserFilterComboBox.SelectedIndex = -1; // No default selection
        }

        private void UserFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            var query = _dbContext.QuizResults.Include(qr => qr.User).Include(qr => qr.Question).AsQueryable();
            if (UserFilterComboBox.SelectedValue != null)
            {
                int userId = (int)UserFilterComboBox.SelectedValue;
                query = query.Where(qr => qr.UserId == userId);
            }
            UserResultsGrid.ItemsSource = query.ToList();
        }

        private void StartServerButton_Click(object sender, RoutedEventArgs e)
        {
            console.Show();
            console.ServerStart();
            StopServerButton.IsEnabled = true;
            StartServerButton.IsEnabled = false;
        }

        private void StopServerButton_Click(object sender, RoutedEventArgs e)
        {
            console.Close();
            console = new ConsoleOutput();
            StopServerButton.IsEnabled = false;
            StartServerButton.IsEnabled = true;
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                // Check the navigation history for an existing UserStatusPage
                this.NavigationService.GoBack();
            }
            if (_isServerRunning && _serverProcess != null)
            {
                _serverProcess.Kill();
                _serverProcess.Dispose();
                _isServerRunning = false;
                StartServerButton.IsEnabled = true;
                StopServerButton.IsEnabled = false;
                _outputWindow?.AppendOutput("Server stopped.");
                _outputWindow?.Close();
                _outputWindow = null;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            UserFilterComboBox.SelectedValue = null;
            LoadUsers();
            UpdateGrid();
        }
    }
}