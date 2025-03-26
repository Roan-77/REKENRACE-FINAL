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
        private int _userAnswer;
        private int _correctAnswersCount;

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

        public int UserAnswer
        {
            get => _userAnswer;
            set
            {
                _userAnswer = value;
                OnPropertyChanged();
            }
        }

        public int CurrentProblemNumber => _currentProblemIndex + 1;
        public int TotalProblems => _problems.Count;
        public int CorrectAnswersCount => _correctAnswersCount;

        // Using built-in ICommand implementations
        public ICommand CheckAnswerCommand { get; }
        public ICommand NextProblemCommand { get; }

        public QuizViewModel(Player player)
        {
            _player = player;
            _problems = Enumerable.Range(0, 10)
                .Select(_ => MathProblem.GenerateProblem(player.Difficulty))
                .ToList();

            _currentProblemIndex = 0;
            _correctAnswersCount = 0;

            // Using CommandBinding or RelayCommand if you prefer
            CheckAnswerCommand = new RelayCommand(CheckAnswer);
            NextProblemCommand = new RelayCommand(NextProblem, () => _currentProblemIndex < _problems.Count - 1);
        }

        private void CheckAnswer()
        {
            bool isCorrect = UserAnswer == CurrentProblem.CorrectAnswer;
            CurrentProblem.IsCorrect = isCorrect;

            if (isCorrect)
            {
                _correctAnswersCount++;
            }
        }

        private void NextProblem()
        {
            if (_currentProblemIndex < _problems.Count - 1)
            {
                _currentProblemIndex++;
                UserAnswer = 0;
                OnPropertyChanged(nameof(CurrentProblem));
                OnPropertyChanged(nameof(CurrentProblemNumber));
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Simple inline RelayCommand (optional)
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