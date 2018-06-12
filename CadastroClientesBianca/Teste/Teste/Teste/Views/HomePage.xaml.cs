
using AppClientes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppClientes.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CadastroCliente : ContentPage
	{
		public CadastroCliente ()
		{
			InitializeComponent ();
		}

        async void Cadastro(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new CadastroView(), true);          
            
        }       
        

        async void ExcluirCliente(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new ExcluirView(), true);
        }


        async void ListagemCliente(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new ListagemView(), true);
        }


        async void ProcurarCliente(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new ProcurarView(), true);
        }

    }
}