using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace rekenrace_roan.Models
{
    public class HighScore
    {
        public required string Name { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }
    }

    public class HighScoreRepository
    {
        private const string FILE_PATH = "highscores.txt";

        public List<HighScore> GetHighScores()
        {
            if (!File.Exists(FILE_PATH))
                return new List<HighScore>();

            return File.ReadAllLines(FILE_PATH)
                .Select(line =>
                {
                    var parts = line.Split(',');
                    return new HighScore
                    {
                        Name = parts[0],
                        Score = int.Parse(parts[1]),
                        Date = DateTime.Parse(parts[2])
                    };
                })
                .OrderByDescending(h => h.Score)
                .Take(10)
                .ToList();
        }

        public void SaveHighScore(HighScore highScore)
        {
            // Append the new high score to the file
            using (StreamWriter writer = File.AppendText(FILE_PATH))
            {
                writer.WriteLine($"{highScore.Name},{highScore.Score},{highScore.Date}");
            }
        }
    }
}