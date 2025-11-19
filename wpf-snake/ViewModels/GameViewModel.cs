using System;
using System.Windows.Input;
using System.Windows.Threading;
using wpf_snake.Commands;
using wpf_snake.Models;
using wpf_snake.Services;

namespace wpf_snake.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        public GameState Model { get; }
        private readonly DispatcherTimer timer;
        private readonly INavigationService _navigation;

        public int CellSize { get; }
        public int Score => Model.score;
        public bool IsPaused { get; private set; }

        public ICommand KeyPressCommand { get; }

        public GameViewModel(int cellSize, int n, INavigationService? navigation = null)
        {
            CellSize = cellSize;
            Model = new GameState(n);
            _navigation = navigation ?? new NavigationService();

            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            timer.Tick += (s, e) => UpdateGame();
            timer.Start();

            KeyPressCommand = new RelayCommand(OnKeyPress);
        }

        private void UpdateGame()
        {
            Model.Update();
            OnPropertyChanged(nameof(Score));

            if (Model.isGameOver)
            {
                timer.Stop();
                _navigation.OpenGameOver(Score);
            }
        }

        private void OnKeyPress(object obj)
        {
            if (obj is not string key) return;

            if (Model.isGameOver) return;

            switch (key)
            {
                case "Up": Model.SetDirection(Direction.Up); break;
                case "Down": Model.SetDirection(Direction.Down); break;
                case "Left": Model.SetDirection(Direction.Left); break;
                case "Right": Model.SetDirection(Direction.Right); break;
                case "P":
                case "Escape":
                    IsPaused = !IsPaused;
                    if (IsPaused) timer.Stop();
                    else timer.Start();
                    OnPropertyChanged(nameof(IsPaused));
                    break;
            }
        }
    }
}
