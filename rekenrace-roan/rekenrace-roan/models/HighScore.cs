using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace rekenrace_roan.Models
{
    public class HighScore
    {
        public required string Name { get; set; }
        public string Difficulty { get; set; } = string.Empty;
        public int Score { get; set; }
        public DateTime Date { get; set; }

        // Method to format the high score for display
        public string FormattedScore => $"{Name}, {Score} van de 10 goed, {Date:dd/MM/yyyy}";
    }

    public class HighScoreRepository
    {
        private const string FILE_PATH = "highscores.txt";

        public void ResetHighScores()
        {
            File.WriteAllText(FILE_PATH, string.Empty);
        }


        public List<HighScore> GetHighScores()
        {
            if (!File.Exists(FILE_PATH))
                return new List<HighScore>();

            // Read and parse high scores, grouping by name and difficulty
            return File.ReadAllLines(FILE_PATH)
                .Select(line =>
                {
                    var parts = line.Split(',');
                    return new HighScore
                    {
                        Name = parts[0].Trim(),
                        Difficulty = parts[1].Trim(),
                        Score = int.Parse(parts[2].Trim()),
                        Date = DateTime.Parse(parts[3].Trim())
                    };
                })
                .GroupBy(h => new { h.Name, h.Difficulty })
                .Select(g => g.OrderByDescending(h => h.Score).First())
                .OrderBy(h => GetDifficultyOrder(h.Difficulty))
                .ThenByDescending(h => h.Score)
                .Take(30) // Increased to allow more high scores across difficulties
                .ToList();
        }

        public void SaveHighScore(HighScore newHighScore)
        {
            // Read existing high scores
            List<HighScore> existingScores = new List<HighScore>();
            if (File.Exists(FILE_PATH))
            {
                existingScores = File.ReadAllLines(FILE_PATH)
                    .Select(line =>
                    {
                        var parts = line.Split(',');
                        return new HighScore
                        {
                            Name = parts[0].Trim(),
                            Difficulty = parts[1].Trim(),
                            Score = int.Parse(parts[2].Trim()),
                            Date = DateTime.Parse(parts[3].Trim())
                        };
                    })
                    .ToList();
            }

            // Find existing score for this name and difficulty
            var existingScore = existingScores.FirstOrDefault(s =>
                s.Name.Equals(newHighScore.Name, StringComparison.OrdinalIgnoreCase) &&
                s.Difficulty.Equals(newHighScore.Difficulty, StringComparison.OrdinalIgnoreCase));

            // Remove existing score if new score is higher
            if (existingScore != null)
            {
                if (newHighScore.Score > existingScore.Score)
                {
                    existingScores.Remove(existingScore);
                }
                else
                {
                    // If existing score is higher or equal, don't save new score
                    return;
                }
            }

            // Add new high score
            existingScores.Add(newHighScore);

            // Save updated high scores
            File.WriteAllLines(FILE_PATH,
                existingScores
                    .OrderBy(h => GetDifficultyOrder(h.Difficulty))
                    .ThenByDescending(h => h.Score)
                    .Select(h => $"{h.Name}, {h.Difficulty}, {h.Score}, {h.Date:yyyy-MM-dd HH:mm:ss}")
            );
        }

        // Helper method to order difficulties
        private int GetDifficultyOrder(string difficulty)
        {
            return difficulty?.ToLower() switch
            {
                "makkelijk" => 1,
                "gemiddeld" => 2,
                "moeilijk" => 3,
                _ => 4
            };
        }
    }
}