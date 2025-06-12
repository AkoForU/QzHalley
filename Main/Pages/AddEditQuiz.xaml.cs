using Main.Models;
using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Main.Pages
{
    /// <summary>
    /// Interaction logic for AddEditQuiz.xaml
    /// </summary>
    public partial class AddEditQuiz : Page
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private Question _question;
        private QuizDbContext _context;
        private string _imagePath; // Stores the original selected file path
        private string _newImagePath; // Stores the new path after copying to qzphotos
        private bool imageRemove = false;

        public AddEditQuiz(Question question = null)
        {
            InitializeComponent();
            _context = new QuizDbContext();
            _question = question ?? new Question(); // New question if null
            LoadQuestionData();
        }

        private void LoadQuestionData()
        {
            // Populate fields with question data if editing
            QuestionTextBox.Text = _question.QuestionText ?? string.Empty;
            Option1TextBox.Text = _question.Option1 ?? string.Empty;
            Option2TextBox.Text = _question.Option2 ?? string.Empty;
            Option3TextBox.Text = _question.Option3 ?? string.Empty;
            Option4TextBox.Text = _question.Option4 ?? string.Empty;
            CorrectAnswerTextBox.Text = _question.CorrectAnswer ?? string.Empty;

            // Load image if it exists
            if (!string.IsNullOrEmpty(_question.img))
            {
                RemoveImageButton.Visibility = Visibility.Visible;
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _question.img);
                if (File.Exists(fullPath))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(fullPath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad; // Ensure image loads
                    bitmap.EndInit();
                    ImagePreview.Source = bitmap;
                    _imagePath = _question.img; // Store the relative path
                    ImagePreview.Visibility = Visibility.Visible;
                }
                else
                {
                    Console.WriteLine($"Image not found at: {fullPath}");
                    MessageBox.Show($"Image file not found at: {fullPath}");
                }
            }
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg",
                Title = "Select an Image"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                ImagePreview.Visibility= Visibility.Visible;
                _imagePath = openFileDialog.FileName; // Store the original selected file path
                ImagePreview.Source = new BitmapImage(new Uri(_imagePath));
                // Copy the image to qzphotos folder immediately (optional preview step)
                //CopyImageToQzphotos();
            }
        }

        private void CopyImageToQzphotos()
        {
            if (!string.IsNullOrEmpty(_imagePath) && File.Exists(_imagePath))
            {
                // Define the target directory (use relative path ../../../qzphotos or current directory qzphotos)
                string targetDirectory = Path.Combine("../../../../QzHalleyAPI/images"); // Adjust based on project structure
                // Ensure the directory exists
                Directory.CreateDirectory(targetDirectory);

                // Generate a unique filename to avoid overwriting
                string fileName = Path.GetFileNameWithoutExtension(_imagePath) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(_imagePath);
                _newImagePath = Path.Combine(targetDirectory, fileName);

                // Copy the file to the qzphotos folder
                File.Copy(_imagePath, _newImagePath, true); // Overwrite if exists
            }
        }

        private void SaveQuestion_Click(object sender, RoutedEventArgs e)
        {
            var options = new[] { Option1TextBox.Text, Option2TextBox.Text, Option3TextBox.Text, Option4TextBox.Text };
            var nonEmptyOptions = options.Where(o => !string.IsNullOrEmpty(o)).ToList();

            // Validate repeated options
            if (nonEmptyOptions.Count != nonEmptyOptions.Distinct(StringComparer.OrdinalIgnoreCase).Count())
            {
                MessageBox.Show("Options must be unique. Please ensure no two options have the same text.", "Duplicate Options", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate number of options (at least two, unless only one is provided)
            if (nonEmptyOptions.Count == 1)
            {
                MessageBox.Show("You must provide at least two options for the question.", "Single Option", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate sequential options
            if (nonEmptyOptions.Count > 0)
            {
                bool isSequential = true;
                for (int i = 0; i < options.Length - 1; i++)
                {
                    if (string.IsNullOrEmpty(options[i]) && !string.IsNullOrEmpty(options[i + 1]))
                    {
                        isSequential = false;
                        break;
                    }
                }
                if (!isSequential)
                {
                    MessageBox.Show("Options must be entered sequentially. Fill options starting from Option 1 without skipping.", "Non-Sequential Options", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // Validate correct answer matches one option
            if (Option1TextBox.Text != CorrectAnswerTextBox.Text &&
                Option2TextBox.Text != CorrectAnswerTextBox.Text &&
                Option3TextBox.Text != CorrectAnswerTextBox.Text &&
                Option4TextBox.Text != CorrectAnswerTextBox.Text)
            {
                MessageBox.Show("One of the options needs to be the same as the correct answer.", "Careful", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else if (string.IsNullOrEmpty(CorrectAnswerTextBox.Text))
            {
                MessageBox.Show("The correct answer cannot be empty.", "No Correct Answer", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else if (string.IsNullOrEmpty(QuestionTextBox.Text))
            {
                MessageBox.Show("The question cannot be empty. Please provide a question.", "No Question", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                try
                {
                    _question.QuestionText = QuestionTextBox.Text;
                    _question.Option1 = Option1TextBox.Text;
                    _question.Option2 = Option2TextBox.Text;
                    _question.Option3 = Option3TextBox.Text;
                    _question.Option4 = Option4TextBox.Text;
                    _question.CorrectAnswer = CorrectAnswerTextBox.Text;

                    // Handle image saving
                    if (!string.IsNullOrEmpty(_imagePath) && File.Exists(_imagePath))
                    {
                        try
                        {
                            CopyImageToQzphotos(); // Copy the image
                            _question.img = _newImagePath != null ? Path.GetRelativePath(Directory.GetCurrentDirectory(), _newImagePath) : null;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to copy image: {ex.Message}");
                            MessageBox.Show($"Failed to copy image: {ex.Message}", "Image Copy Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    else if (string.IsNullOrEmpty(_imagePath) && !string.IsNullOrEmpty(_question.img))
                    {
                        _question.img = null; // Image removed
                    }
                    else
                    {
                        _question.img = null; // No image
                    }

                    using (var context = new QuizDbContext())
                    {
                        try
                        {
                            if (_question.Id == 0) // New question
                            {
                                context.Questions.Add(_question);
                            }
                            else // Existing question
                            {
                                context.Questions.Update(_question);
                            }
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to save question: {ex.Message}");
                            MessageBox.Show($"Failed to save question: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    if (imageRemove == true)
                    {
                        if (!string.IsNullOrEmpty(_question.img) && File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _question.img)))
                        {
                            try
                            {
                                File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _question.img));
                                Console.WriteLine($"Image deleted: {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _question.img)}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Failed to delete image: {ex.Message}");
                                MessageBox.Show($"Failed to delete image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    MessageBox.Show("Question saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService?.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void RemoveImageButton_Click(object sender, RoutedEventArgs e)
        {
            ImagePreview.Visibility = Visibility.Collapsed;
            ImagePreview.Source = null; // Clear the image preview
            _imagePath = null; // Reset the original path
            _newImagePath = null; // Reset the new path
            RemoveImageButton.Visibility = Visibility.Collapsed; // Hide the remove button
            imageRemove = true;
            // Note: The image file itself is not deleted from the filesystem; only the reference is removed
        }
    }
}