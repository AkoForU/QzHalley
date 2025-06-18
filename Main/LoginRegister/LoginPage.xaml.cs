using System.Text;
using System.Windows;
using System.Windows.Controls;
namespace Main
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    
    public partial class LoginPage : Page
    {
        static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
        LoginRegisterWindow loginRegisterWindow;
        public LoginPage(LoginRegisterWindow tmp)
        {
            InitializeComponent();
            loginRegisterWindow = tmp;
        }
        private void ShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            passwordTxtBox.Text = passwordBox.Password;
            passwordBox.Visibility = Visibility.Collapsed;
            passwordTxtBox.Visibility = Visibility.Visible;
        }

        private void ShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            passwordBox.Password = passwordTxtBox.Text;
            passwordTxtBox.Visibility = Visibility.Collapsed;
            passwordBox.Visibility = Visibility.Visible;
        }

        private void AuthenticateButton_Click(object sender, RoutedEventArgs e)
        {
            if (usrntxtbx.Text != "admin" || (sha256(passwordTxtBox.Text) != "678a7e594c0d7963619b3975f7814b40e081937b417bf8734e1e6b7b83763a36") && sha256(passwordBox.Password) != "678a7e594c0d7963619b3975f7814b40e081937b417bf8734e1e6b7b83763a36")
            {
                MessageBox.Show("The password or the username is wrong");
            }
            else
            {
                AdminWindow adminWindow = new AdminWindow();
                loginRegisterWindow.Close();
                adminWindow.Show();
            }
        }
    }
}
