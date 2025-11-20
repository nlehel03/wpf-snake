using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace wpf_snake.Views
{
    /// <summary>
    /// Interaction logic for LeaderBoardView.xaml
    /// </summary>
    public partial class LeaderBoardView : Window
    {
        public LeaderBoardView()
        {
            InitializeComponent();
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
