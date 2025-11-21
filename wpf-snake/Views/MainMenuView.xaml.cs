using System;
using System.Windows;
using wpf_snake.Infrastructure;
using wpf_snake.ViewModels;
using wpf_snake.Services;

namespace wpf_snake.Views
{
    public partial class MainMenuView : Window
    {
        public MainMenuView()
        {
            InitializeComponent();
            WindowPlacement.Apply(this);
            DataContext = new MainMenuViewModel(new NavigationService());
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // Optional: remove if you only want MVVM command
            var choose = new ChooseView();
            choose.Show();
            Close();
        }

        private void LeaderBoardButton_Click(object sender, RoutedEventArgs e)
        {
            var leaderBoard = new LeaderBoardView();
            leaderBoard.Show();
            Close();
        }
    }
}
