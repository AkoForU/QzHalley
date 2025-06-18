using Main.Models;
using System.IO;
using System.Windows;
using System.Windows.Controls;
namespace Main
{
    /// <summary>
    /// Interaction logic for CheckQuizPage.xaml
    /// </summary>
    public partial class CheckQuizPage : Page
    {
        QuizDbContext _context;
        public CheckQuizPage()
        {
            InitializeComponent();
            _context = new QuizDbContext();
            LoadQuestions();
        }
        private void LoadQuestions()
        {
            QuestionsDataGrid.ItemsSource = _context.Questions.ToArray();
        }

        private void AddQuestionBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Pages.AddEditQuiz(null));
        }

        private void EditQuestionBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedQuestion = QuestionsDataGrid.SelectedItem as Question;
            if (selectedQuestion != null)
            {
                // Navigate to AddEditQuiz page and pass the selected question
                var addEditPage = new Pages.AddEditQuiz(selectedQuestion);
                NavigationService?.Navigate(addEditPage);
            }
            else
            {
                MessageBox.Show("Please select a question to edit.");
            }
        }

        private void DeleteQuestionBtn_Click(object sender, RoutedEventArgs e)
        {
            // Check if a question is selected
            if (QuestionsDataGrid.SelectedItem is Question selectedQuestion)
            {
                // Confirm deletion with the user
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the question: '{selectedQuestion.QuestionText}' (ID: {selectedQuestion.Id})?",
                    "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (_context.Questions.Any(e => e==selectedQuestion&&e.img!=null))
                    {
                        if (!string.IsNullOrEmpty(selectedQuestion.img) && File.Exists(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, selectedQuestion.img)))
                        {
                            try
                            {
                                File.Delete(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, selectedQuestion.img));
                                Console.WriteLine($"Image deleted: {System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, selectedQuestion.img)}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Failed to delete image: {ex.Message}");
                                MessageBox.Show($"Failed to delete image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    // Remove the selected question from the ObservableCollection
                    _context.Questions.Remove(selectedQuestion);

                    // Optional: Refresh the DataGrid (though ObservableCollection handles this automatically)
                    QuestionsDataGrid.ItemsSource = null;
                    _context.SaveChanges();
                    QuestionsDataGrid.ItemsSource = _context.Questions.ToArray();
                    // Provide feedback
                    MessageBox.Show($"Question '{selectedQuestion.QuestionText}' deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                // Inform the user if no question is selected
                MessageBox.Show("Please select a question to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
