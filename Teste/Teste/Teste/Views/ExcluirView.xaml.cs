using AppClientes.DAL;
using AppClientes.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppClientes.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExcluirView : ContentPage
	{
		public ExcluirView ()
		{
			InitializeComponent ();

        }

        public string escolha = null;

        private async void ListaClientes_ItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var element = e.SelectedItem as Cliente;
                var result = await DisplayAlert("Deseja excluir o cliente abaixo ?", "ID:" + element.ClienteID + "\nNome: " + element.Nome + "\nIdade: " + element.Idade + "\nTelefone: " + element.Telefone, "SIM", "NÃO");
                await Task.Delay(500);
                if (result)
                {
                    ExcluiBancoAsync(element.ClienteID);
                    ListaClientes.ItemsSource = null;
                }
                else
                {
                    ListaClientes.SelectedItem = null;
                }                
            }
        }

        private async void ExcluiBancoAsync(int idCliente)
        {
            try
            {
                DatabaseContext contexto = new DatabaseContext();
                Cliente c = contexto.Clientes.Find(idCliente);                
                contexto.Entry(c).State = EntityState.Deleted;
                contexto.SaveChanges();
                await DisplayAlert("Exclusão concluída", "Cliente excluído com sucesso " ,"OK");
            }
            catch (Exception)
            {
                await DisplayAlert("Erro", "Cliente já excluído", "OK");
            }
           
        }


        private void pckExcluir_SelectedIndexChanged(object sender, EventArgs e)
        {
            escolha = pckExcluir.Items[pckExcluir.SelectedIndex];

        }

        DatabaseContext contexto = new DatabaseContext();

        async void BuscaCliente(object sender, EventArgs args)
        {
            ListaClientes.ItemsSource = null;
           
            if (entryExcluir.Text != null && escolha != null)
            {
                if (escolha.Equals("Por ID"))
                {
                    string verifica = "^[0-9]";
                    if (!Regex.IsMatch(entryExcluir.Text, verifica))
                    {
                        await DisplayAlert("Campo digitado inválido", "Você digitou um campo inválido, digite apenas números !", "OK");
                    }
                    else
                    {
                        BuscaPorIDAsync();

                    }
                }
                else
                {
                    if (!Regex.IsMatch(entryExcluir.Text, @"^[a-zA-Z]+$"))
                    {
                        await DisplayAlert("Campo digitado inválido", "Você digitou um campo inválido, digite apenas caracter ! ", "OK");
                    }
                    else
                    {
                        BuscaPorNomeAsync();
                    }
                }
            }
            else
            {
                await DisplayAlert("Atenção", "Você esqueceu de preencher o campo de busca ou o tipo de busca", "OK");
            }
        }


        private async void BuscaPorIDAsync()
        {
            var busca = (from c in contexto.Clientes
                         where c.ClienteID.Equals(Convert.ToInt32(entryExcluir.Text))
                         select c).ToList();

            if (busca.Count > 0)
            {
                ObservableCollection<Cliente> ListaItens3 = new ObservableCollection<Cliente>(busca);
                ListaClientes.ItemsSource = ListaItens3;
                ListaClientes.ItemSelected += ListaClientes_ItemSelectedAsync;                
            }
            else
            {
                await DisplayAlert("Atenção", "Não foi encontrado nada com esse resultado", "OK");                
            }
        }


        private async void BuscaPorNomeAsync()
        {
            var busca = (from c in contexto.Clientes
                         where c.Nome.ToLower().Equals(entryExcluir.Text.ToLower())
                         select c).ToList();

            
            if (busca.Count > 0)
            {
                ObservableCollection<Cliente> ListaItens4 = new ObservableCollection<Cliente>(busca);
                ListaClientes.ItemsSource = ListaItens4;
                ListaClientes.ItemSelected += ListaClientes_ItemSelectedAsync;                
            }
            else
            {
                await DisplayAlert("Atenção", "Não foi encontrado nada com esse resultado", "OK");                
            }
        }



    }
}