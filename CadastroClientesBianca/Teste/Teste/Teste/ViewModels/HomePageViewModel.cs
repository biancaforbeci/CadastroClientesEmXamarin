using Android.Content.Res;
using AppClientes.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppClientes.ViewModels
{
	public class HomePageViewModel : BindableBase
	{
        public HomePageViewModel()
        {
            Title = "Bem-Vindo";
            Imagem = "imagem.png";
            Cadastro = "Cadastrar";
            Listagem = "Listagem";
            Procura = "Buscar";
            Excluir = "Excluir";
            Cadastrar = new DelegateCommand<object>(TelaCadastro);
            Listar = new DelegateCommand<object>(TelaListar);
            Exclusao = new DelegateCommand<object>(TelaExcluir);
            Procurar = new DelegateCommand<object>(TelaProcurar);

        }


        public string Title { get; set; }
        public string Imagem { get; set; }
        public string Cadastro { get; set; }
        public string Listagem { get; set; }
        public string Procura { get; set; }
        public string Excluir { get; set; }


        public DelegateCommand<object> Cadastrar { get; set; }
        public DelegateCommand<object> Listar { get; set; }
        public DelegateCommand<object> Exclusao { get; set; }
        public DelegateCommand<object> Procurar { get; set; }





        private void TelaCadastro(object parm)
        {
           
        }


        private void TelaExcluir(object parm)
        {

        }

        private void TelaListar(object parm)
        {

        }

        private void TelaProcurar(object parm)
        {

        }


    }
}
