using System.Windows;
using wpf_snake.Views;
using wpf_snake.ViewModels;

namespace wpf_snake.Services
{
    // existing INavigationService (defined elsewhere) should include OpenGame; this class implements it
    public class NavigationService : INavigationService
    {
        public void OpenChoose()
        {
            var wnd = new ChooseView();
            wnd.Show();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is MainMenuView)
                {
                    w.Close();
                    break;
                }
            }
        }

        public void OpenGame(int cellSize, int n)
        {
            // Create GameView with the requested size and show it
            var game = new GameView(cellSize, n);
            game.Show();

            // Close any ChooseView windows
            foreach (Window w in Application.Current.Windows)
            {
                if (w is ChooseView)
                {
                    w.Close();
                    break;
                }
            }
        }
        public void OpenGameOver(int score)
        {
            var over = new GameOverView
            {
                DataContext = new GameOverViewModel(score, this)
            };
            over.Show();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is GameView)
                {
                    w.Close();
                    break;
                }
            }
        }
        public void OpenMainMenu()
        {
            var menu = new MainMenuView
            {
                DataContext = new MainMenuViewModel(this)
            };
            menu.Show();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is GameOverView)
                {
                    w.Close();
                    break;
                }
            }
        }
    }
}