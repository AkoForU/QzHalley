using Main.Models;
using System.IO;
using System.Windows;
namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (!Directory.Exists("../../../../db"))
            {
                Directory.CreateDirectory("../../../../db");
            }
            new QuizDbContext().Database.EnsureCreated();
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation =WindowStartupLocation.CenterScreen;
        }

        private void authenticate_Click(object sender, RoutedEventArgs e)
        {
            LoginRegisterWindow login=new LoginRegisterWindow(true);
            login.Title = "Login";
            this.Close();
            login.Show();
        }
    }
}