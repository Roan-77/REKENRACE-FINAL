using System;

namespace rekenrace_roan.Models
{
    public class MathProblem
    {
        public string Equation { get; set; }
        public int CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }

        public static MathProblem GenerateProblem(string difficulty)
        {
            Random random = new Random();
            int num1, num2, answer;
            string equation;

            // Normalize the difficulty string to handle potential case differences or whitespace
            difficulty = difficulty?.Trim().ToLower();

            switch (difficulty)
            {
                case "makkelijk": // Easy
                    num1 = random.Next(1, 21);
                    num2 = random.Next(1, 21);
                    if (random.Next(2) == 0)
                    {
                        equation = $"{num1} + {num2}";
                        answer = num1 + num2;
                    }
                    else
                    {
                        // Ensure positive result
                        if (num1 < num2)
                        {
                            (num1, num2) = (num2, num1);
                        }
                        equation = $"{num1} - {num2}";
                        answer = num1 - num2;
                    }
                    break;

                case "gemiddeld": // Medium
                    num1 = random.Next(1, 11);
                    num2 = random.Next(1, 11);
                    switch (random.Next(3))
                    {
                        case 0:
                            equation = $"{num1} + {num2}";
                            answer = num1 + num2;
                            break;
                        case 1:
                            // Ensure positive result
                            if (num1 < num2)
                            {
                                (num1, num2) = (num2, num1);
                            }
                            equation = $"{num1} - {num2}";
                            answer = num1 - num2;
                            break;
                        default:
                            equation = $"{num1} x {num2}";
                            answer = num1 * num2;
                            break;
                    }
                    break;

                case "moeilijk": // Hard
                    num1 = random.Next(1, 21);
                    num2 = random.Next(1, 21);
                    int operation = random.Next(4);
                    switch (operation)
                    {
                        case 0:
                            equation = $"{num1} + {num2}";
                            answer = num1 + num2;
                            break;
                        case 1:
                            // Ensure positive result
                            if (num1 < num2)
                            {
                                (num1, num2) = (num2, num1);
                            }
                            equation = $"{num1} - {num2}";
                            answer = num1 - num2;
                            break;
                        case 2:
                            equation = $"{num1} x {num2}";
                            answer = num1 * num2;
                            break;
                        default:
                            // Ensure no division by zero and whole number result
                            num2 = num2 == 0 ? 1 : num2;
                            num1 = (num1 * num2);
                            equation = $"{num1} ÷ {num2}";
                            answer = num1 / num2;
                            break;
                    }
                    break;

                default:
                    // Add a fallback to default difficulty or throw a more informative exception
                    throw new ArgumentException($"Invalid difficulty level: {difficulty}. Must be 'Makkelijk', 'Gemiddeld', or 'Moeilijk'.");
            }

            return new MathProblem
            {
                Equation = equation,
                CorrectAnswer = answer,
                IsCorrect = false
            };
        }
    }
}