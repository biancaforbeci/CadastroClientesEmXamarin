using Android.Widget;
using AppClientes.DAL;
using AppClientes.Models;
using System;
using System.Collections.Generic;
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
	public partial class CadastroView : ContentPage
	{
      	public CadastroView ()
		{
			InitializeComponent ();
		}

        bool valida;

        private void SalvarBD(object sender, EventArgs args)
        {
            ValidandoDadosAsync();

            if (valida == true)
            {
                Cliente novo = new Cliente();
                novo.Nome = EntryNome.Text;
                novo.Idade = Convert.ToInt32(EntryIdade.Text);
                novo.Telefone = EntryTelefone.Text;
                SalvandoBancoAsync(novo);
            }               
                      
        }

        private async void SalvandoBancoAsync(Cliente novo)
        {
            try
            {
                DatabaseContext contexto = new DatabaseContext();
                contexto.Add(novo);
                contexto.SaveChanges();
                await DisplayAlert("Salvo", "Cliente salvo com sucesso", "OK");
            }
            catch (Exception e)
            {
                await DisplayAlert("Erro", "Ocorreu um erro para salvar: " + e, "OK");
            }
        }

        private async void ValidandoDadosAsync()
        {

            string tel = "^(?:(?([0-9]{2}))?[-. ]?)?([0-9]{4})[-. ]?([0-9]{4})$";

            if((EntryNome.Text != null) && (EntryIdade.Text != null) && (EntryTelefone.Text != null))
            {
                if (!Regex.IsMatch(EntryNome.Text, @"^[a-zA-Z]+$"))
                {
                    await DisplayAlert("ATENÇÃO", "Campo nome inválido,digite apenas caracteres !", "OK");                   
                }
                else if (Convert.ToInt32(EntryIdade.Text) < 0)
                {
                    await DisplayAlert("ATENÇÃO", "Campo idade inválido, digite valores positivos !", "OK");                   
                }
                else if (Regex.IsMatch(EntryTelefone.Text, tel) == false)
                {
                    await DisplayAlert("ATENÇÃO", "Campo telefone inválido ! Digite como o exemplo: 3333-3333 ou 33333333", "OK");                    
                }
                else
                {
                    valida = true;
                }
            }
            else
            {              
                await DisplayAlert("Campo vazio", "Verifique se foram preenchidos todos os campos", "OK");
                await Task.Delay(500);
            }           
            
        }
    }
}