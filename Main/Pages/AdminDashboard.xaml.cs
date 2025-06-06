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

namespace Main
{
    /// <summary>
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Page
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }

        private void UserAddBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Pages/UsersOverviewPage.xaml", UriKind.Relative));
            //LoginRegisterWindow register = new LoginRegisterWindow(false);
            //register.Title = "AddUser";
            //register.ShowDialog();
        }
    }
}
