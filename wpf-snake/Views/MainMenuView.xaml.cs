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
using wpf_snake.Services;
using wpf_snake.ViewModels;

namespace wpf_snake.Views
{
    /// <summary>
    /// Interaction logic for MainMenuView.xaml
    /// </summary>
    public partial class MainMenuView : Window
    {
        public MainMenuView()
        {
            InitializeComponent();
            DataContext = new MainMenuViewModel(new NavigationService());
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var choose = new ChooseView();
            choose.Show();
            this.Close();
        }
    }
}
