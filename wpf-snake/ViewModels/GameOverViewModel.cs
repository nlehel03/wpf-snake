using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf_snake.Persistence;
using wpf_snake.Commands;
using wpf_snake.Services;

namespace wpf_snake.ViewModels
{
    public class GameOverViewModel : BaseViewModel
    {
        private readonly FileManagement _fm = new FileManagement();
        private readonly INavigationService _nav;

        public int Score { get; }
        private string _playerName = string.Empty;
        public string PlayerName
        {
            get => _playerName;
            set
            {
                if (_playerName != value)
                {
                    _playerName = value;
                    OnPropertyChanged(nameof(PlayerName));
                }
            }
        }

        public RelayCommand SaveAndExitCommand { get; }

        public GameOverViewModel(int score, INavigationService nav)
        {
            Score = score;
            _nav = nav;
            SaveAndExitCommand = new RelayCommand(_ => SaveAndReturn());
        }

        private void SaveAndReturn()
        {
            _fm.saveScore(Score, PlayerName);
            _nav.OpenMainMenu();
        }
    }
}
