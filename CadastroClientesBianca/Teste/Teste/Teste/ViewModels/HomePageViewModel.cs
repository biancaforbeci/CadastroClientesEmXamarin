using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppClientes.ViewModels
{
	public class HomePageViewModel : BindableBase, INavigatedAware
    {
        public HomePageViewModel(INavigationService navigationService)
        {
            Title = "Bem-Vindo";
            Image = "imagem.png";
            Register = "Cadastrar";
            Listing = "Listagem";
            Search = "Buscar";
            Delete = "Excluir";
            _navigationService = navigationService;
            NavigateCommand = new DelegateCommand<string>(OnNavigateCommandExecuted);
        }

        public string Title { get; set; }
        public string Image { get; set; }
        public string Register { get; set; }
        public string Listing { get; set; }
        public string Search { get; set; }
        public string Delete { get; set; }
        private readonly INavigationService _navigationService;

        public DelegateCommand<string> NavigateCommand { get; set; }

        private async void OnNavigateCommandExecuted(string path)
        {
            await _navigationService.NavigateAsync(path);
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
