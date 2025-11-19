using System.Windows.Input;
using wpf_snake.Commands;
using wpf_snake.Models;
using wpf_snake.Persistence;
using wpf_snake.Services;

namespace wpf_snake.ViewModels
{
    public class ChooseViewModel : BaseViewModel
    {
        private readonly FileManagement _fm;
        private readonly INavigationService _navigation;

        public MapSize MapSize { get; private set; }
        public ICommand StartSmallMapCommand { get; }
        public ICommand StartMediumMapCommand { get; }
        public ICommand StartLargeMapCommand { get; }

        public ChooseViewModel(INavigationService? navigation = null)
        {
            _fm = new FileManagement();
            _navigation = navigation ?? new NavigationService();
            StartSmallMapCommand = new RelayCommand(_ => StartSmallMap());
            StartMediumMapCommand = new RelayCommand(_ => StartMediumMap());
            StartLargeMapCommand = new RelayCommand(_ => StartLargeMap());
        }
        public void StartSmallMap()
        {
            MapSize = _fm.loadMapSize(0);
            OnPropertyChanged(nameof(MapSize));
            _navigation.OpenGame(MapSize.cellSize, MapSize.n);
        }
        public void StartMediumMap()
        {
            MapSize = _fm.loadMapSize(1);
            OnPropertyChanged(nameof(MapSize));
            _navigation.OpenGame(MapSize.cellSize, MapSize.n);
        }
        public void StartLargeMap()
        {
            MapSize = _fm.loadMapSize(2);
            OnPropertyChanged(nameof(MapSize));
            _navigation.OpenGame(MapSize.cellSize, MapSize.n);
        }
    }
}