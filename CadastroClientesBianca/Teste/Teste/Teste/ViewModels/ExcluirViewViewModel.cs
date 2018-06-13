using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppClientes.ViewModels
{
	public class ExcluirViewViewModel : BindableBase
	{
        public ExcluirViewViewModel()
        {
            Title = "Excluir Clientes";
            Titleprocura = "Selecione o tipo de procura:";
            TitleButton = "Pesquisar";
            BuscaCliente= new DelegateCommand<object>(BuscarCliente);
        }

        public string Title { get; set; }
        public string Titleprocura { get; set; }
        public string TitleButton { get; set; }


        public DelegateCommand<object> BuscaCliente { get; set; }



        private void BuscarCliente(object parm)
        {

        }




    }
}
