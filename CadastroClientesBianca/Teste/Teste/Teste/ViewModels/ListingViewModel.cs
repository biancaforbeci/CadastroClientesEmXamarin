using AppClientes.Infra.Services;
using AppClientes.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AppClientes.ViewModels
{
	public class ListingViewModel : BindableBase, INotifyPropertyChanged
    {
        private readonly IService _clientService;

        public ListingViewModel(IPageDialogService pageDialog, IService clientService)
        {
            Title = "Clientes Cadastrados";
            _pageDialog = pageDialog;
            Elements = _Elements;
            ListClients = ListItems;
            Search = "Pesquisar";
            SearchDB = new DelegateCommand(SearchClient);
            ListSelect = new DelegateCommand(ListClient_ItemSelected);
            _clientService = clientService;
        }


        public string Title { get; set; }
        public string Search { get; set; }
        public Client ListSelected { get; set; }
        public int Selected { get; set; }
        public List<string> _Elements = new List<string> { "Selecione o tipo de Listagem", "Listar Cliente", "Listar por Ordenação" };
        public List<string> Elements
        {
            get { return _Elements; }
            set
            {
                if (Equals(value, _Elements)) return;
                _Elements = value;
            }
        }

        public int ElementsSelectedIndex
        {
            get
            {
                return Selected;
            }
            set
            {
                if (Selected != value)
                {
                    Selected = value;
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private List<Client> ListItems;
        public List<Client> ListClients
        {
            get { return ListItems; }
            set
            {
                ListItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ListClients)));
            }
        }

        public DelegateCommand ListSelect { get; set; }

        public DelegateCommand SearchDB { get; set; }


        IPageDialogService _pageDialog;




        private void ListClient_ItemSelected()
        {
            if (Selected.Equals(1))
            {

                _pageDialog.DisplayAlertAsync("Detalhes do Cliente", "ID:" + ListSelected.ClientID + "\nNome: " + ListSelected.Name, "OK");
                ListSelected = null;

            }
            else if (Selected.Equals(2))
            {

                _pageDialog.DisplayAlertAsync("Detalhes do Cliente", "ID:" + ListSelected.ClientID + "\nNome: " + ListSelected.Name + "\nIdade: " + ListSelected.Age + "\nTelefone: " + ListSelected.Phone, "OK");
                ListSelected = null;
            }
        }



        private void SearchClient()
        {

            if (Selected.Equals(1))
            {
                var list = _clientService.AllClient();
                if (list.Count > 0)
                {
                    ListClients = list;
                }
                else
                {
                    _pageDialog.DisplayAlertAsync("Nada encontrado", "Não foi encontrado nada cadastrado", "OK");
                }
            }
            else if (Selected.Equals(2))
            {
                var listord = _clientService.AgeListing();

                if (listord.Count > 0)
                {
                    ListClients = listord;
                }
                else
                {
                    _pageDialog.DisplayAlertAsync("Nada encontrado", "Não foi encontrado nada cadastrado", "OK");
                }
            }
        }
    }
}