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
using System.Windows.Shapes;

namespace Main
{
    /// <summary>
    /// Interaction logic for AminPage.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            WindowStartupLocation=WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.NoResize;
        }

        private void Loaded(object sender, RoutedEventArgs e)
        {
            //Frame adminFrame = new Frame();
            //adminFrame.Name = "AdminPage";
            //MainWindow.Children.Clear()
            //MainWindow.Children.Add(adminFrame);
            //adminFrame.Navigate(new AdminDashboard());
            MainWindow.Navigate(new Uri("/Pages/AdminDashboard.xaml", UriKind.Relative));
        }
    }
}
