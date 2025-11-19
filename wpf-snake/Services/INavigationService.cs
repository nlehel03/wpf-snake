namespace wpf_snake.Services
{
    public interface INavigationService
    {
        void OpenChoose();
        void OpenGame(int cellSize, int n);
        void OpenGameOver(int score);
        void OpenMainMenu();
    }
}