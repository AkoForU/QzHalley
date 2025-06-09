using Main.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for CheckQuizPage.xaml
    /// </summary>
    public partial class CheckQuizPage : Page
    {
        public ObservableCollection<Question> Questions { get; set; }
        public CheckQuizPage()
        {
            InitializeComponent();
            LoadQuestions();
        }
        private void LoadQuestions()
        {
            Questions = new ObservableCollection<Question>
            {
                new Question { Id = 1, QuestionText = "What is 2 + 2?", Option1 = "2", Option2 = "4", Option3 = "", Option4 = "", CorrectAnswer = "4" },
                new Question { Id = 2, QuestionText = "What is the capital of France?", Option1 = "Berlin", Option2 = "Paris", Option3 = "Madrid", Option4 = "", CorrectAnswer = "Paris" },
                new Question { Id = 3, QuestionText = "What is the largest planet?", Option1 = "Earth", Option2 = "Mars", Option3 = "Jupiter", Option4 = "Saturn", CorrectAnswer = "Jupiter" },
                new Question { Id = 4, QuestionText = "Which gas makes up most of the air?", Option1 = "Oxygen", Option2 = "Nitrogen", Option3 = "", Option4 = "", CorrectAnswer = "Nitrogen" },
                new Question { Id = 5, QuestionText = "What color is the sky?", Option1 = "Green", Option2 = "Blue", Option3 = "Red", Option4 = "Yellow", CorrectAnswer = "Blue" }
            };
            QuestionsDataGrid.ItemsSource = Questions;
        }

        private void AddQuestionBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Pages/AddEditQuiz.xaml", UriKind.Relative));
        }

        private void EditQuestionBtn_Click(object sender, RoutedEventArgs e)
        {

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
                    // Remove the selected question from the ObservableCollection
                    Questions.Remove(selectedQuestion);

                    // Optional: Refresh the DataGrid (though ObservableCollection handles this automatically)
                    QuestionsDataGrid.ItemsSource = null;
                    QuestionsDataGrid.ItemsSource = Questions;

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
