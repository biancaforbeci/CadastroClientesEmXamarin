using AppClientes.DAL;
using AppClientes.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace AppClientes
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new CadastroCliente());
            InicializandoAsync();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}


        protected async void InicializandoAsync()
        {
            var dbFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var fileName = "Clientes.db";
            var dbFullPath = Path.Combine(dbFolder, fileName);
            DatabaseContext contexto = new DatabaseContext(dbFullPath);
            await Database.InitializeDataAsync(contexto);
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
