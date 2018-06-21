using AppClientes.DAL;
using AppClientes.Infra.Services;
using AppClientes.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace AppClientes.ViewModels
{
	public class SearchViewModel : BindableBase, INotifyPropertyChanged
    {
        private readonly IService _clienteService;

        public SearchViewModel(IPageDialogService pageDialog, IService clienteService)
        {
            Title = "Procurar Clientes";
            TitleButton = "Pesquisar";
            _pageDialog = pageDialog;
            Search = new DelegateCommand(SearchBD);
            Elements = _Elements;
            ListClients = ListItems;
            ListSelect = new DelegateCommand(ListClients_ItemSelectedAsync);
            _clienteService = clienteService;
        }

        public string Title { get; set; }
        public string TitleTipo { get; set; }
        public string TitleButton { get; set; }
        public int Selected { get; set; }
        public string ItemSearch { get; set; }
        public Client ListSelected { get; set; }
        IPageDialogService _pageDialog;
        public DelegateCommand Search { get; set; }
        public DelegateCommand ListSelect { get; set; }

        public List<string> _Elements = new List<string> { "Selecione o tipo de Busca", "Por ID", "Por Nome" };
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


        private async void SearchBD()
        {

            if (ItemSearch != null && Selected != 0)
            {
                if (Selected.Equals(1))
                {
                    string verifica = "^[0-9]";
                    if (!Regex.IsMatch(ItemSearch, verifica))
                    {
                        await _pageDialog.DisplayAlertAsync("Campo digitado inválido", "Você digitou um campo inválido, digite apenas números !", "OK");
                    }
                    else
                    {
                        SearchID();

                    }
                }
                else
                {

                    if (!Regex.IsMatch(ItemSearch, @"^[a-zA-Z]+$"))
                    {
                        await _pageDialog.DisplayAlertAsync("Campo digitado inválido", "Você digitou um campo inválido, digite apenas caracter ! ", "OK");
                    }
                    else
                    {
                        SearchName();
                    }

                }
            }
            else
            {
                await _pageDialog.DisplayAlertAsync("Atenção", "Você esqueceu de preencher o campo de busca ou o tipo de busca", "OK");
            }
        }



        private async void SearchID()
        {
            var search = _clienteService.SearchID(Convert.ToInt32(ItemSearch));

            if (search.Count > 0)
            {
                ListClients = search;
            }
            else
            {
                await _pageDialog.DisplayAlertAsync("Atenção", "Não foi encontrado nada com esse resultado", "OK");
                ListClients = search;
            }
        }

        private async void SearchName()
        {
            var search = _clienteService.SearchName(ItemSearch.ToLower());

            if (search.Count > 0)
            {
                ListClients = search.ToList();
            }
            else
            {
                await _pageDialog.DisplayAlertAsync("Atenção", "Não foi encontrado nada com esse resultado", "OK");
                ListClients = search;
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

        private async void ListClients_ItemSelectedAsync()
        {
            await _pageDialog.DisplayAlertAsync("Detalhes do Cliente", "ID:" + ListSelected.ClientID + "\nNome: " + ListSelected.Name + "\nIdade: " + ListSelected.Age + "\nTelefone: " + ListSelected.Phone, "OK");
            ListSelected = null;
        }
    }
}




