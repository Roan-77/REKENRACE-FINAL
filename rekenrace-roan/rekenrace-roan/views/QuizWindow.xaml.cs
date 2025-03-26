using System.Windows;
using System.Windows.Input;
using rekenrace_roan.Models;
using rekenrace_roan.ViewModels;

namespace rekenrace_roan.views
{
    public partial class QuizWindow : Window
    {
        private QuizViewModel _viewModel;

        public QuizWindow(Player player)
        {
            InitializeComponent();
            _viewModel = new QuizViewModel(player);
            DataContext = _viewModel;
        }

        private void CheckAnswer_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_viewModel.UserAnswer == _viewModel.CurrentProblem.CorrectAnswer
                ? "Goed gedaan! Correct antwoord."
                : $"Helaas, het goede antwoord was {_viewModel.CurrentProblem.CorrectAnswer}.",
                "Resultaat");
        }

        private void NextProblem_Click(object sender, RoutedEventArgs e)
        {
            // Check if it's the last problem
            if (_viewModel.CurrentProblemNumber == _viewModel.TotalProblems)
            {
                MessageBox.Show($"Quiz voltooid! Je hebt {_viewModel.CorrectAnswersCount} van de {_viewModel.TotalProblems} vragen goed.", "Quiz Resultaat");
                BackToMainMenu_Click(sender, e);
            }
        }

        private void BackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void txtAnswer_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Only allow numeric input
            e.Handled = !int.TryParse(e.Text, out _);
        }
    }
}