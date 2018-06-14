using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppClientes.ViewModels
{
    public class ListagemViewModel : BindableBase
    {
        public ListagemViewModel(IPageDialogService pageDialog)
        {

            Title = "Clientes Cadastrados";
            TitleTipo = "Selecione o tipo:";
            ImagemList = "drawable-xhdpi/person.png";
            _pageDialog = pageDialog;
        }

        public string Title { get; set; }
        public string TitleTipo { get; set; }
        public string ImagemList { get; set; }        
        public string escolha { get; set; }



        IPageDialogService _pageDialog;





            //private void ListaClientes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
            //{
            //    if (e.SelectedItem != null)
            //    {
            //        if (escolha.Equals("Listar Clientes"))
            //        {
            //            var element = e.SelectedItem as Cliente;
            //            DisplayAlert("Detalhes do Cliente", "ID:" + element.ClienteID + "\nNome: " + element.Nome, "OK");
            //            ListaClientes.SelectedItem = null;
            //        }
            //        else
            //        {
            //            var element = e.SelectedItem as Cliente;
            //            DisplayAlert("Detalhes do Cliente", "ID:" + element.ClienteID + "\nNome: " + element.Nome + "\nIdade: " + element.Idade + "\nTelefone: " + element.Telefone, "OK");
            //            ListaClientes.SelectedItem = null;
            //        }
            //    }
            //}



            //private void pckBusca_SelectedIndexChanged(object sender, EventArgs e)
            //{
            //    DatabaseContext contexto = new DatabaseContext();
            //    escolha = pckBusca.Items[pckBusca.SelectedIndex];
            //    if (escolha.Equals("Listar Clientes"))
            //    {
            //        var lista = contexto.Clientes.ToList();
            //        if (lista.Count > 0)
            //        {
            //            ObservableCollection<Cliente> ListaItens = new ObservableCollection<Cliente>(lista);
            //            ListaClientes.ItemsSource = ListaItens;
            //            ListaClientes.ItemSelected += ListaClientes_ItemSelected;
            //        }
            //        else
            //        {
            //            DisplayAlert("Nada encontrado", "Não foi encontrado nada cadastrado", "OK");
            //        }
            //    }
            //    else
            //    {
            //        var listaord = (from x in contexto.Clientes
            //                        orderby x.Idade
            //                        select x).ToList();
            //        if (listaord.Count > 0)
            //        {
            //            ObservableCollection<Cliente> ListaItens1 = new ObservableCollection<Cliente>(listaord);
            //            ListaClientes.ItemsSource = ListaItens1;
            //            ListaClientes.ItemSelected += ListaClientes_ItemSelected;
            //        }
            //        else
            //        {
            //            DisplayAlert("Nada encontrado", "Não foi encontrado nada cadastrado", "OK");
            //        }

            //    }

            //}



        }
    }

















}
}