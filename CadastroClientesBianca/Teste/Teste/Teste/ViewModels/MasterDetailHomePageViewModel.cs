using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppClientes.ViewModels
{
	public class MasterDetailHomePageViewModel : BindableBase, INavigatedAware
    {
        public MasterDetailHomePageViewModel(INavigationService navigationService)
        {            
            HomePage = "Home Page";
            TitleMenu = "Menu";
            Title = "Bem-Vindo";
            Image = "imagem.png";
            Image2 = "clientes.png";
            Register = "Cadastrar";
            Listing = "Listagem";
            Search = "Buscar";
            Delete = "Excluir";
            _navigationService = navigationService;
            NavigateCommand = new DelegateCommand<string>(OnNavigateCommandExecuted);
            ListingLocal = "Importar e Exportar Lista";            
            Localization = "GeoLocalização";            
        }

        public string Title { get; set; }
        public string Image { get; set; }
        public string Image2 { get; set; }
        public string Register { get; set; }
        public string Listing { get; set; }
        public string Search { get; set; }
        public string Delete { get; set; }
        public string TitleMenu { get; set; }
        private readonly INavigationService _navigationService;
        
        public string ListingLocal { get; set; }
        public string Localization { get; set; }
        public string HomePage { get; set; }

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
            throw new NotImplementedException();
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
