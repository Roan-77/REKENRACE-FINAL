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

        public List<HighScore> GetHighScores()
        {
            if (!File.Exists(FILE_PATH))
                return new List<HighScore>();

            // Read and parse high scores, keeping top 10 across all difficulties
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
                .OrderByDescending(h => h.Score)
                .ThenByDescending(h => h.Date)
                .Take(10)
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

            // Add new high score
            existingScores.Add(newHighScore);

            // Save top 10 high scores overall
            File.WriteAllLines(FILE_PATH,
                existingScores
                    .OrderByDescending(h => h.Score)
                    .ThenByDescending(h => h.Date)
                    .Take(10)
                    .Select(h => $"{h.Name}, {h.Difficulty}, {h.Score}, {h.Date:yyyy-MM-dd HH:mm:ss}")
            );
        }
    }
}