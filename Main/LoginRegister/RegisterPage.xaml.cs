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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        QuizDbContext _context;
        LoginRegisterWindow loginRegisterWindow;
        public RegisterPage(LoginRegisterWindow tmp)
        {
            InitializeComponent();
            loginRegisterWindow = tmp;
            _context=new QuizDbContext();
        }
        private void BackButton(object sender, RoutedEventArgs e)
        {
            loginRegisterWindow.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (usrntxtbx.Text == string.Empty || usrntxtbx.Text.Contains(" "))
            {
                MessageBox.Show("Invalid Username");
            }
            else if (_context.Users.Any(e => e.Name == usrntxtbx.Text))
            {
                MessageBox.Show("The username is already used");
            }
            else if (passwordTxtBox.Text != passwordTxtBoxRepeat.Text)
            {
                MessageBox.Show("Passwords doesn't match");
            }
            else
            {
                if (passwordTxtBox.Text == string.Empty && passwordTxtBox.Text == string.Empty)
                { MessageBox.Show("The password doesn't need to be empty"); }
                else
                {
                    _context.Users.Add(new User
                    {
                        Name = usrntxtbx.Text,
                        Password = passwordTxtBox.Text,
                        Score = "null",
                        Created = DateTime.Now,
                        CompletedQuiz = null
                    });
                    _context.SaveChanges();
                    MessageBox.Show("User added with succes");
                }
            }
        }
    }
}
