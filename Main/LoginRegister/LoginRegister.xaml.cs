using System.Windows;
using System.Windows.Controls;
namespace Main
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LoginRegisterWindow : Window
    {
        bool _state = false;//true is for Login and false is for Register
        public LoginRegisterWindow(bool state)
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _state = state;
            PageSetter();
        }
        //setting the page for the window (Login or Register an User)
        public void PageSetter()
        {
            if (_state) {
                Frame loginFrame = new Frame();
                loginFrame.Name = "LoginPage";
                ContentFrame.Children.Clear();
                ContentFrame.Children.Add(loginFrame);
                loginFrame.Navigate(new LoginPage(this));
            }
            else
            {
                Frame addUserFrame=new Frame();
                addUserFrame.Name = "AddUser";
                ContentFrame.Children.Clear();
                ContentFrame.Children.Add(addUserFrame);
                addUserFrame.Navigate(new RegisterPage(this));
            }
        }
    }
}
