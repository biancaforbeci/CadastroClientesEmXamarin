using AppClientes.DAL;
using AppClientes.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppClientes.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProcurarView : ContentPage
	{
		public ProcurarView ()
		{
			InitializeComponent ();
            
        }       


        public string escolha = null;

        private void ListaClientes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var element = e.SelectedItem as Cliente;
                DisplayAlert("Detalhes do Cliente", "ID:" + element.ClienteID + "\nNome: " + element.Nome + "\nIdade: " + element.Idade + "\nTelefone: " + element.Telefone, "OK");
                ListaClientes.SelectedItem = null;
            }
        }

        private void PckProcurar_SelectedIndexChanged(object sender, EventArgs e)
        {
            escolha = pckProcurar.Items[pckProcurar.SelectedIndex];

        }

        DatabaseContext contexto = new DatabaseContext();

        async void BuscaCliente(object sender, EventArgs args)
        {
            ListaClientes.ItemsSource = null;
            if (entryBusca.Text!=null && escolha!=null)
            {
               if(escolha.Equals("Por ID"))
                {
                    string verifica = "^[0-9]";
                    if (!Regex.IsMatch(entryBusca.Text, verifica))
                    {
                        await DisplayAlert("Campo digitado inválido", "Você digitou um campo inválido, digite apenas números !", "OK");
                    }
                    else
                    {                        
                        ProcuraPorIDAsync();
                                               
                    }                    
                }
                else
                {                    

                    if (!Regex.IsMatch(entryBusca.Text, @"^[a-zA-Z]+$"))
                    {
                        await DisplayAlert("Campo digitado inválido", "Você digitou um campo inválido, digite apenas caracter ! ", "OK");
                    }
                    else
                    {
                        ProcuraPorNomeAsync();
                    }
                    
                }                
            }
            else
            {
                await DisplayAlert("Atenção", "Você esqueceu de preencher o campo de busca ou o tipo de busca", "OK");
            }
        }

        private async void ProcuraPorIDAsync()
        {
            var busca = (from c in contexto.Clientes
                         where c.ClienteID.Equals(Convert.ToInt32(entryBusca.Text))
                         select c).ToList();

                if (busca.Count > 0)
                {
                ObservableCollection<Cliente> ListaItens2 = new ObservableCollection<Cliente>(busca);
                ListaClientes.ItemsSource = ListaItens2;
                ListaClientes.ItemSelected += ListaClientes_ItemSelected;                
                }
                else
                {
                    await DisplayAlert("Atenção", "Não foi encontrado nada com esse resultado", "OK");
                }           

        }

        private async void ProcuraPorNomeAsync()
        {
            var busca = (from c in contexto.Clientes
                         where c.Nome.ToLower().Equals(entryBusca.Text.ToLower())
                         select c).ToList();

            if (busca.Count > 0)
            {
                ObservableCollection<Cliente> ListaItens3 = new ObservableCollection<Cliente>(busca);
                ListaClientes.ItemsSource = ListaItens3;
                ListaClientes.ItemSelected += ListaClientes_ItemSelected;                
            }
            else
            {
                await DisplayAlert("Atenção", "Não foi encontrado nada com esse resultado", "OK");
            }
        }





        }
}