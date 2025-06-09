using Microsoft.Win32;
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
    /// Interaction logic for AddEditQuiz.xaml
    /// </summary>
    public partial class AddEditQuiz : Page
    {
        private string selectedImagePath;
        public AddEditQuiz()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void OptionsCountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SaveQuestion_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All files (*.*)|*.*",
                Title = "Select an Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                selectedImagePath = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage(new Uri(selectedImagePath));
                ImagePreview.Source = bitmap;
            }
        }
    }
}
