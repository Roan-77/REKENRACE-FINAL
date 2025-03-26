using System;
using System.Globalization;
using System.Linq;
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
            var allHighScores = _highScoreRepository.GetHighScores();

            // Group high scores by difficulty
            var makkelijkHighScores = allHighScores
                .Where(h => h.Difficulty.Equals("Makkelijk", StringComparison.OrdinalIgnoreCase))
                .Take(5)
                .ToList();

            var gemiddeldHighScores = allHighScores
                .Where(h => h.Difficulty.Equals("Gemiddeld", StringComparison.OrdinalIgnoreCase))
                .Take(5)
                .ToList();

            var moeilijkHighScores = allHighScores
                .Where(h => h.Difficulty.Equals("Moeilijk", StringComparison.OrdinalIgnoreCase))
                .Take(5)
                .ToList();

            // Set ItemsSource for each ListView
            lvMakkelijkHighScores.ItemsSource = makkelijkHighScores;
            lvGemiddeldHighScores.ItemsSource = gemiddeldHighScores;
            lvMoeilijkHighScores.ItemsSource = moeilijkHighScores;
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

    // StringLengthToBooleanConverter remains the same
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