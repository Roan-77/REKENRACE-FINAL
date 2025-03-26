using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using rekenrace_roan.Models;
using rekenrace_roan.views;

namespace rekenrace_roan
{
    public partial class MainWindow : Window
    {
        private HighScoreRepository _highScoreRepository;

        public MainWindow()
        {
            InitializeComponent();
            _highScoreRepository = new HighScoreRepository();
            LoadHighScores();
        }

        private void LoadHighScores()
        {
            lvHighScores.ItemsSource = _highScoreRepository.GetHighScores();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            // Get the player model from resources
            Player player = (Player)FindResource("PlayerModel");

            // Create and show the quiz window
            QuizWindow quizWindow = new QuizWindow(player);
            quizWindow.Show();

            // Close the main window
            this.Close();
        }
    }

    // StringLengthToBooleanConverter remains the same as in previous implementation
    public class StringLengthToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int length)
            {
                return length > 0;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}