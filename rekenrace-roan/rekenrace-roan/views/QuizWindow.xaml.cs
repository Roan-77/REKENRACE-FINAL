using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using rekenrace_roan.Models;
using rekenrace_roan.ViewModels;
using System.Globalization;
using System.Windows.Data;

namespace rekenrace_roan.views
{
    public partial class QuizWindow : Window
    {
        private QuizViewModel _viewModel;
        private HighScoreRepository _highScoreRepository;
        private Player _originalPlayer;

        public QuizWindow(Player player)
        {
            InitializeComponent();
            _highScoreRepository = new HighScoreRepository();
            _originalPlayer = player;
            _viewModel = new QuizViewModel(player);
            DataContext = _viewModel;

            // Add event handler for quiz completion
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;

            // Add the BooleanToForegroundConverter to resources
            if (!Resources.Contains("BooleanToForegroundConverter"))
            {
                Resources.Add("BooleanToForegroundConverter", new BooleanToForegroundConverter());
            }
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.IsQuizCompleted) && _viewModel.IsQuizCompleted)
            {
                ShowEndScreen();
            }
        }

        private void ShowEndScreen()
        {
            // Clear the feedback message
            _viewModel.FeedbackMessage = string.Empty;

            // Hide quiz panel
            QuizPanel.Visibility = Visibility.Collapsed;

            // Set final score text
            txtFinalScore.Text = $"Je hebt {_viewModel.CorrectAnswersCount} van de 10 vragen goed!";

            // Show end screen panel
            EndScreenPanel.Visibility = Visibility.Visible;

            // Save high score
            SaveHighScore();
        }


        private void SaveHighScore()
        {
            var highScore = new HighScore
            {
                Name = _viewModel.Player.Name,
                Difficulty = _viewModel.Player.Difficulty,
                Score = _viewModel.CorrectAnswersCount,
                Date = DateTime.Now
            };

            _highScoreRepository.SaveHighScore(highScore);
        }

        private void RestartQuiz_Click(object sender, RoutedEventArgs e)
        {
            // Create a new quiz with the same player
            QuizWindow newQuizWindow = new QuizWindow(_originalPlayer);
            newQuizWindow.Show();
            this.Close();
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            // Open main window and close current window
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
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

    // Converter to change feedback text color based on correctness (remains the same)
    public class BooleanToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isFeedbackPositive)
            {
                return isFeedbackPositive ? Brushes.Green : Brushes.Red;
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}