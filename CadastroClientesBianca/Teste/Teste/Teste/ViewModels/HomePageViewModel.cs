using Android.Content.Res;
using AppClientes.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppClientes.ViewModels
{
	public class HomePageViewModel : BindableBase,INavigatedAware
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
            RegisterClient = new DelegateCommand(PageRegister);
            PageListClient = new DelegateCommand(PageList);
            DeletePage = new DelegateCommand(PageDelete);
            SearchPage = new DelegateCommand(PageSearch);
            LocalFileList = new DelegateCommand(PageListingLocal);
            ListingLocal = "Importar e Exportar Lista";
        }


        private readonly INavigationService _navigationService;

        public string Title { get; set; }
        public string Image { get; set; }
        public string Register { get; set; }
        public string Listing { get; set; }
        public string Search { get; set; }
        public string Delete { get; set; }
        public string ListingLocal { get; set; }


        public DelegateCommand RegisterClient { get; set; }
        public DelegateCommand PageListClient { get; set; }
        public DelegateCommand DeletePage { get; set; }
        public DelegateCommand SearchPage { get; set; }
        public DelegateCommand LocalFileList { get; set; }



        private void PageRegister()
        {
            _navigationService.NavigateAsync("Register");
        }


        private void PageDelete()
        {
            _navigationService.NavigateAsync("Delete");  
        }

        private void PageList()
        {
            _navigationService.NavigateAsync("Listing");  
        }

        private void PageSearch()
        {
            _navigationService.NavigateAsync("Search");  
        }

        private void PageListingLocal()
        {
            _navigationService.NavigateAsync("LocalFileList");
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            throw new NotImplementedException();
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
