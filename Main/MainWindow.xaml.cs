
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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