using System.Windows;
using System.Windows.Input;
using rekenrace_roan.Models;
using rekenrace_roan.ViewModels;

namespace rekenrace_roan.views
{
    public partial class QuizWindow : Window
    {
        private QuizViewModel _viewModel;
        private HighScoreRepository _highScoreRepository;

        public QuizWindow(Player player)
        {
            InitializeComponent();
            _highScoreRepository = new HighScoreRepository();
            _viewModel = new QuizViewModel(player);
            DataContext = _viewModel;
        }

        private void CheckAnswer_Click(object sender, RoutedEventArgs e)
        {
            bool isLastProblem = _viewModel.CheckAnswer();

            // Show result for current problem
            MessageBoxResult result = MessageBox.Show(
                _viewModel.UserAnswer == _viewModel.CurrentProblem.CorrectAnswer
                    ? "Goed gedaan! Correct antwoord."
                    : $"Helaas, het goede antwoord was {_viewModel.CurrentProblem.CorrectAnswer}.",
                "Resultaat",
                MessageBoxButton.OK
            );

            // If last problem, show final score and save high score
            if (isLastProblem)
            {
                MessageBox.Show(
                    $"Quiz voltooid! Je hebt {_viewModel.CorrectAnswersCount} van de {_viewModel.TotalProblems} vragen goed.",
                    "Eindresultaat",
                    MessageBoxButton.OK
                );

                // Save high score
                _highScoreRepository.SaveHighScore(new HighScore
                {
                    Name = _viewModel.Player.Name,
                    Score = _viewModel.CorrectAnswersCount,
                    Date = DateTime.Now
                });

                // Return to main menu
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