using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppClientes.ViewModels
{
	public class ListagemViewViewModel : BindableBase
	{
        public ListagemViewViewModel()
        {
            Title = "Clientes Cadastrados";
            TitleTipo = "Selecione o tipo:";
            ImagemList = "drawable-xhdpi/person.png";
        }

        public string Title { get; set; }
        public string TitleTipo { get; set; }
        public string ImagemList { get; set; }
       




    }
}
