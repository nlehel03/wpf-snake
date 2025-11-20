using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using wpf_snake.Commands;
using wpf_snake.Models;
using wpf_snake.Persistence;
using wpf_snake.Views;

namespace wpf_snake.ViewModels
{
    public class LeaderBoardViewModel : BaseViewModel
    {
        private readonly FileManagement _fm = new FileManagement();

        public ObservableCollection<Scores> Scores { get; } = new ObservableCollection<Scores>();

        public ICommand MenuCommand { get; }

        public LeaderBoardViewModel()
        {
            LoadScores();
            MenuCommand = new RelayCommand(_ => OpenMainMenu());
        }

        private void LoadScores()
        {
            var list = _fm.loadScores() ?? new System.Collections.Generic.List<Scores>();
            // sort descending by score as in FileManagement.saveScore
            foreach (var s in list.OrderByDescending(x => x.score))
            {
                Scores.Add(s);
            }
        }

        private void OpenMainMenu()
        {
            // Show MainMenu and close this LeaderBoardView
            var menu = new MainMenuView
            {
                DataContext = new MainMenuViewModel(new Services.NavigationService())
            };
            menu.Show();

            // Close any existing LeaderBoardView(s)
            foreach (Window w in Application.Current.Windows)
            {
                if (w is LeaderBoardView)
                {
                    w.Close();
                    break;
                }
            }
        }
    }
}

