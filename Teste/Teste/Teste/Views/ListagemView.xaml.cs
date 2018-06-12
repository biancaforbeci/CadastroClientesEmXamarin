using AppClientes.DAL;
using AppClientes.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppClientes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListagemView : ContentPage
    {

        public ListagemView()
        {
            InitializeComponent();
            
        }

        public string escolha { get; set; }

        private void ListaClientes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                if (escolha.Equals("Listar Clientes"))
                {
                    var element = e.SelectedItem as Cliente;
                    DisplayAlert("Detalhes do Cliente", "ID:" + element.ClienteID + "\nNome: " + element.Nome, "OK");
                    ListaClientes.SelectedItem = null;
                }
                else
                {
                    var element = e.SelectedItem as Cliente;
                    DisplayAlert("Detalhes do Cliente", "ID:" + element.ClienteID + "\nNome: " + element.Nome + "\nIdade: " + element.Idade + "\nTelefone: " + element.Telefone, "OK");
                    ListaClientes.SelectedItem = null;
                }
            }
        }

        

        private void pckBusca_SelectedIndexChanged(object sender, EventArgs e)
        {
            DatabaseContext contexto = new DatabaseContext();
            escolha = pckBusca.Items[pckBusca.SelectedIndex];
            if (escolha.Equals("Listar Clientes"))
            {
                var lista = contexto.Clientes.ToList();
                if (lista.Count > 0)
                {
                    ObservableCollection<Cliente>ListaItens = new ObservableCollection<Cliente>(lista);
                    ListaClientes.ItemsSource = ListaItens;
                    ListaClientes.ItemSelected += ListaClientes_ItemSelected;
                }
                else
                {
                    DisplayAlert("Nada encontrado", "Não foi encontrado nada cadastrado", "OK");
                }
            }
            else
            {
                var listaord = (from x in contexto.Clientes
                                orderby x.Idade
                                select x).ToList();
                if (listaord.Count > 0)
                {
                    ObservableCollection<Cliente> ListaItens1 = new ObservableCollection<Cliente>(listaord);
                    ListaClientes.ItemsSource = ListaItens1;
                    ListaClientes.ItemSelected += ListaClientes_ItemSelected;
                }
                else
                {
                    DisplayAlert("Nada encontrado", "Não foi encontrado nada cadastrado", "OK");
                }

            }

        }

        

    }
}
