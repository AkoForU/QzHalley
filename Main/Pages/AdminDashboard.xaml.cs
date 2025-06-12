using Main.Models;
using Main.Pages;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Threading;

namespace Main
{
    /// <summary>
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    
    public partial class AdminDashboard : Page
    {
        UserStatusPage userStatusPage;
        private readonly DispatcherTimer _refreshTimer;
        public AdminDashboard()
        {
            InitializeComponent();
            userStatusPage = new UserStatusPage();
            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _refreshTimer.Tick += RefreshFunction;
            Loaded += (s, e) => _refreshTimer.Start();
            Unloaded += (s, e) => _refreshTimer.Stop();
        }

        private async void RefreshFunction(object sender, EventArgs e)
        {
            try
            {
                using (var dbContext = new QuizDbContext())
                {
                    // Get counts
                    ttlqts.Text = (await dbContext.Questions.CountAsync()).ToString();
                    ttlusr.Text = (await dbContext.Users.CountAsync()).ToString();

                    // Get last created user
                    var lastCreated = await dbContext.Users
                        .Where(u => u.Created != null)
                        .OrderByDescending(u => u.Created)
                        .FirstOrDefaultAsync();

                    // Get last quiz completion
                    var quizCompleted = await dbContext.Users
                        .Where(u => u.CompletedQuiz != null)
                        .OrderBy(u => u.CompletedQuiz)
                        .FirstOrDefaultAsync();

                    // Update UI
                    logUsers.Text = lastCreated != null
                        ? $"User '{lastCreated.Name}' created at {lastCreated.Created}"
                        : "No users have been created yet.";

                    logQuiz.Text = quizCompleted != null
                        ? $"User '{quizCompleted.Name}' completed at {quizCompleted.CompletedQuiz} with score {quizCompleted.Score}%"
                        : "No users have completed a quiz yet.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing dashboard: {ex.Message}");
                _refreshTimer.Stop(); // Stop on fatal error
            }
        }
        private void UserAddBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Pages/UsersOverviewPage.xaml", UriKind.Relative));
            //LoginRegisterWindow register = new LoginRegisterWindow(false);
            //register.Title = "AddUser";
            //register.ShowDialog();
        }

        private void QuizCheckBtnClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Pages/CheckQuizPage.xaml", UriKind.Relative));
        }
        //status button clicked
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(userStatusPage);
        }
    }
}
