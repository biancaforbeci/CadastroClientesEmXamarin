using AppClientes.DAL;
using AppClientes.Infra;
using AppClientes.Infra.Api;
using AppClientes.Infra.Services;
using AppClientes.ViewModels;
using AppClientes.Views;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace AppClientes
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            InitializingAsync();

            NavigationService.NavigateAsync("MasterDetailHomePage");
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }


        protected async void InitializingAsync()
        {
            var dbPath = Container.Resolve<IFileSystem>().GetDatabasePath();
            DatabaseContext contexto = new DatabaseContext(dbPath);
            await Database.InitializeDataAsync(contexto);
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MasterDetailHomePage, MasterDetailHomePageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<Register, RegisterViewModel>();
            containerRegistry.RegisterForNavigation<Delete, DeleteViewModel>();
            containerRegistry.RegisterForNavigation<Listing, ListingViewModel>();
            containerRegistry.RegisterForNavigation<Search, SearchViewModel>();
            containerRegistry.RegisterForNavigation<LocalFileList, LocalFileListViewModel>();
            containerRegistry.RegisterInstance(new DatabaseContext());
            containerRegistry.Register<IService, ClientService>();
            containerRegistry.Register<IApiClient, APIClient>();
            containerRegistry.RegisterForNavigation<LocalFileList, LocalFileListViewModel>();
            containerRegistry.RegisterForNavigation<Location, LocationViewModel>();           
        }
    }
}
