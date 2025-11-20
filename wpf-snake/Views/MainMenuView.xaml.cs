using System;
using System.Windows;
using wpf_snake.Infrastructure; // WindowPlacement használata

namespace wpf_snake.Views
{
    public partial class MainMenuView : Window
    {
        public MainMenuView()
        {
            InitializeComponent();
            WindowPlacement.Apply(this); // Pozíció megőrzése
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var choose = new ChooseView();
            choose.Show();
            this.Close();
        }

        private void LeaderBoardButton_Click(object sender, RoutedEventArgs e)
        {
            var leaderBoard = new LeaderBoardView();
            leaderBoard.Show();
            this.Close();
        }
    }
}
