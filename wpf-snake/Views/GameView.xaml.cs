using wpf_snake.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace wpf_snake.Views
{
    public partial class GameView : Window
    {
        private GameViewModel vm;

        public GameView()
        {
            InitializeComponent();
            vm = new GameViewModel(cellSize: 20, n: 30);
            DataContext = vm;
            CompositionTarget.Rendering += RenderGame;
        }

        public GameView(int cellSize, int n)
        {
            InitializeComponent();
            vm = new GameViewModel(cellSize, n);
            DataContext = vm;
            CompositionTarget.Rendering += RenderGame;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            vm.KeyPressCommand.Execute(e.Key.ToString());
        }

        private void RenderGame(object sender, System.EventArgs e)
        {
            GameCanvas.Children.Clear();
            foreach (var segment in vm.Model.snake.body)
            {
                var rect = new Rectangle { Width = vm.CellSize, Height = vm.CellSize, Fill = Brushes.Green };
                Canvas.SetLeft(rect, segment.X * vm.CellSize);
                Canvas.SetTop(rect, segment.Y * vm.CellSize);
                GameCanvas.Children.Add(rect);
            }

            var food = new Rectangle { Width = vm.CellSize, Height = vm.CellSize, Fill = Brushes.Red };
            Canvas.SetLeft(food, vm.Model.food.position.X * vm.CellSize);
            Canvas.SetTop(food, vm.Model.food.position.Y * vm.CellSize);
            GameCanvas.Children.Add(food);
        }
    }
}
