using System.Windows;
using wpf_snake.Infrastructure;

namespace wpf_snake.Views
{
    public partial class LeaderBoardView : Window
    {
        public LeaderBoardView()
        {
            InitializeComponent();
            WindowPlacement.Apply(this);
            DataContext = new ViewModels.LeaderBoardViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var menu = new MainMenuView();
            menu.Show();
            this.Close();
        }
    }
}
