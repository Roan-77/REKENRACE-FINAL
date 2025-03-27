using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
            // Perform answer check
            var (isCorrect, isLastProblem) = _viewModel.CheckAnswerInternal();

            // Update feedback text
            if (isCorrect)
            {
                txtFeedback.Text = "Goed gedaan! Correct antwoord.";
                txtFeedback.Foreground = Brushes.Green;
            }
            else
            {
                txtFeedback.Text = $"Helaas, het goede antwoord was {_viewModel.CurrentProblem.CorrectAnswer}.";
                txtFeedback.Foreground = Brushes.Red;
            }

            // If last problem, save high score and return to main menu
            if (isLastProblem)
            {
                // Save high score
                _highScoreRepository.SaveHighScore(new HighScore
                {
                    Name = _viewModel.Player.Name,
                    Difficulty = _viewModel.Player.Difficulty,
                    Score = _viewModel.CorrectAnswersCount,
                    Date = DateTime.Now
                });

                // Show final score feedback
                txtFeedback.Text = $"Eind resultaat: {_viewModel.CorrectAnswersCount} van de 10 goed";
                txtFeedback.Foreground = Brushes.Blue;

                // Disable further interactions
                txtAnswer.IsEnabled = false;
                ((Button)sender).IsEnabled = false;
            }
            else
            {
                // Move to next problem
                _viewModel.MoveToNextProblem();
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