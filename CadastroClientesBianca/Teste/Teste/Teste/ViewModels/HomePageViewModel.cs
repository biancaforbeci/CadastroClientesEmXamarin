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
            Imagem = "imagem.png";
            Cadastro = "Cadastrar";
            Listagem = "Listagem";
            Procura = "Buscar";
            Excluir = "Excluir";
            _navigationService = navigationService;
            Cadastrar = new DelegateCommand(TelaCadastro);
            Listar = new DelegateCommand(TelaListar);
            //Exclusao = new DelegateCommand(TelaExcluir);
            Procurar = new DelegateCommand(TelaProcurar);

        }


        private readonly INavigationService _navigationService;

        public string Title { get; set; }
        public string Imagem { get; set; }
        public string Cadastro { get; set; }
        public string Listagem { get; set; }
        public string Procura { get; set; }
        public string Excluir { get; set; }


        public DelegateCommand Cadastrar { get; set; }
        public DelegateCommand Listar { get; set; }
        public DelegateCommand Exclusao { get; set; }
        public DelegateCommand Procurar { get; set; }





        private void TelaCadastro()
        {
            _navigationService.NavigateAsync("CadastroView");
        }


        private void TelaExcluir()
        {
            _navigationService.NavigateAsync("ExcluirView");  //não abrindo
        }

        private void TelaListar()
        {
            _navigationService.NavigateAsync("ListagemView");  //não abrindo
        }

        private void TelaProcurar()
        {
            _navigationService.NavigateAsync("ProcurarView");  //não abrindo
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
