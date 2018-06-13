using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppClientes.ViewModels
{
	public class CadastroViewViewModel : BindableBase
	{
        public CadastroViewViewModel()
        {
            Title = "Cadastro Clientes";
            TitleNome = "Nome";
            TitleIdade = "Idade";
            TitleTelefone = "Telefone";
            Cadastrar= new DelegateCommand<object>(SalvarBD);
        }

        public string Title { get; set; }
        public string TitleNome { get; set; }
        public string TitleIdade { get; set; }
        public string TitleTelefone { get; set; }


        public DelegateCommand<object> Cadastrar { get; set; }

        private void SalvarBD(object parm)
        {

        }




    }
}
