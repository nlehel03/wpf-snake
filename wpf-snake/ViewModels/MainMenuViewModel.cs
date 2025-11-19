using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf_snake.Controls;
using wpf_snake.ViewModels;
using wpf_snake.Commands;
using wpf_snake.Services;

namespace wpf_snake.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        public RelayCommand StartGameCommand { get; }

        private readonly INavigationService _navigation;

        public MainMenuViewModel(INavigationService navigation)
        {
            _navigation = navigation;
            StartGameCommand = new RelayCommand(_ => _navigation.OpenChoose());
        }
    }
}
