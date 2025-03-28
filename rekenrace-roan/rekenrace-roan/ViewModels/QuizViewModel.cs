using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using rekenrace_roan.Models;

namespace rekenrace_roan.ViewModels
{
    public class QuizViewModel : INotifyPropertyChanged
    {
        private Player _player;
        private List<MathProblem> _problems;
        private int _currentProblemIndex;
        private int? _userAnswer;
        private int _correctAnswersCount;
        private bool _isQuizCompleted;
        private bool _canCheckAnswer = true;
        private bool _canMoveToNextProblem = false;
        private string _feedbackMessage = string.Empty;
        private bool _isFeedbackPositive;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Player Player
        {
            get => _player;
            set
            {
                _player = value;
                OnPropertyChanged();
            }
        }

        public MathProblem CurrentProblem
        {
            get => _problems[_currentProblemIndex];
        }

        public int? UserAnswer
        {
            get => _userAnswer;
            set
            {
                _userAnswer = value;
                OnPropertyChanged();
            }
        }

        public bool CanCheckAnswer
        {
            get => _canCheckAnswer;
            set
            {
                _canCheckAnswer = value;
                OnPropertyChanged();
            }
        }

        public bool CanMoveToNextProblem
        {
            get => _canMoveToNextProblem;
            set
            {
                _canMoveToNextProblem = value;
                OnPropertyChanged();
            }
        }

        public string FeedbackMessage
        {
            get => _feedbackMessage;
            set
            {
                _feedbackMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsFeedbackPositive
        {
            get => _isFeedbackPositive;
            set
            {
                _isFeedbackPositive = value;
                OnPropertyChanged();
            }
        }

        public string ProblemNumberInfo
        {
            get => $"Vraag {CurrentProblemNumber} van {TotalProblems}";
        }

        public int CurrentProblemNumber => _currentProblemIndex + 1;
        public int TotalProblems => _problems.Count;
        public int CorrectAnswersCount => _correctAnswersCount;
        public bool IsQuizCompleted => _isQuizCompleted;

        public ICommand CheckAnswerCommand { get; }
        public ICommand MoveToNextProblemCommand { get; }

        public QuizViewModel(Player player)
        {
            _player = player;
            _problems = Enumerable.Range(0, 10)
                .Select(_ => MathProblem.GenerateProblem(player.Difficulty))
                .ToList();

            _currentProblemIndex = 0;
            _correctAnswersCount = 0;
            _isQuizCompleted = false;
            _userAnswer = null;

            CheckAnswerCommand = new RelayCommand(CheckAnswer, () => CanCheckAnswer);
            MoveToNextProblemCommand = new RelayCommand(MoveToNextProblem, () => CanMoveToNextProblem);
        }

        public void CheckAnswer()
        {
            if (!CanCheckAnswer)
                return;

            CanCheckAnswer = false;
            bool isCorrect = _userAnswer.HasValue && _userAnswer.Value == CurrentProblem.CorrectAnswer;
            CurrentProblem.IsCorrect = isCorrect;

            if (isCorrect)
            {
                _correctAnswersCount++;
                FeedbackMessage = "Goed gedaan! Correct antwoord.";
                IsFeedbackPositive = true;
            }
            else
            {
                FeedbackMessage = $"Helaas, het goede antwoord was {CurrentProblem.CorrectAnswer}.";
                IsFeedbackPositive = false;
            }

            CanMoveToNextProblem = true;
        }


        public void MoveToNextProblem()
        {
            if (_currentProblemIndex < _problems.Count - 1)
            {
                _currentProblemIndex++;
                UserAnswer = null;
                CanCheckAnswer = true;
                CanMoveToNextProblem = false;
                FeedbackMessage = string.Empty;

                OnPropertyChanged(nameof(CurrentProblem));
                OnPropertyChanged(nameof(CurrentProblemNumber));
                OnPropertyChanged(nameof(ProblemNumberInfo));
            }
            else
            {
                _isQuizCompleted = true;
                OnPropertyChanged(nameof(IsQuizCompleted)); // Add this line
            }
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // RelayCommand class remains the same as in previous implementation
        private class RelayCommand : ICommand
        {
            private readonly Action _execute;
            private readonly Func<bool>? _canExecute;

            public RelayCommand(Action execute, Func<bool>? canExecute = null)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public event EventHandler? CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;
            public void Execute(object? parameter) => _execute();
        }
    }
}