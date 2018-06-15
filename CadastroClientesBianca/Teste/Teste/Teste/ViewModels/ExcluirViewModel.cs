using AppClientes.DAL;
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
	public class ExcluirViewModel : BindableBase
	{
        public ExcluirViewModel()
        {
            Title = "Excluir Clientes";            
            TitleButton = "Pesquisar";
            Procurar = new DelegateCommand<object>(ProcurarBD);
            Elementos = _Elementos;
        }

        public string Title { get; set; }
        public string TitleTipo { get; set; }
        public string TitleButton { get; set; }
        public string ImagemList { get; set; }
        public int ItemEscolha { get; set; }
        public string ItemProcura { get; set; }
        IPageDialogService _pageDialog;
        public DelegateCommand<object> Procurar { get; set; }

        public List<string> _Elementos = new List<string> { "Selecione o tipo de Busca", "Por ID", "Por Nome" };
        public List<string> Elementos
        {
            get { return _Elementos; }
            set
            {
                if (Equals(value, _Elementos)) return;
                _Elementos = value;
            }
        }


        public int ElementosSelectedIndex
        {
            get
            {
                return ItemEscolha;
            }
            set
            {
                if (ItemEscolha != value)
                {
                    ItemEscolha = value;
                }
            }
        }


        private async void ProcurarBD(object parm)
        {

            if (ItemProcura != null && ItemEscolha != 0)
            {
                if (ItemEscolha.Equals(1))
                {
                    string verifica = "^[0-9]";
                    if (!Regex.IsMatch(ItemProcura, verifica))
                    {
                        await _pageDialog.DisplayAlertAsync("Campo digitado inválido", "Você digitou um campo inválido, digite apenas números !", "OK");
                    }
                    else
                    {
                        ProcuraPorIDAsync();

                    }
                }
                else
                {

                    if (!Regex.IsMatch(ItemProcura, @"^[a-zA-Z]+$"))
                    {
                        await _pageDialog.DisplayAlertAsync("Campo digitado inválido", "Você digitou um campo inválido, digite apenas caracter ! ", "OK");
                    }
                    else
                    {
                        ProcuraPorNomeAsync();
                    }

                }
            }
            else
            {
                await _pageDialog.DisplayAlertAsync("Atenção", "Você esqueceu de preencher o campo de busca ou o tipo de busca", "OK");
            }
        }

        DatabaseContext contexto = new DatabaseContext();

        private async void ProcuraPorIDAsync()
        {
            var busca = (from c in contexto.Clientes
                         where c.ClienteID.Equals(Convert.ToInt32(ItemProcura))
                         select c).ToList();

            if (busca.Count > 0)
            {
                ListaClientes = busca;
            }
            else
            {
                await _pageDialog.DisplayAlertAsync("Atenção", "Não foi encontrado nada com esse resultado", "OK");
            }

        }

        private async void ProcuraPorNomeAsync()
        {
            var busca = (from c in contexto.Clientes
                         where c.Nome.ToLower().Equals(ItemProcura.ToLower())
                         select c).ToList();

            if (busca.Count > 0)
            {
                ListaClientes = busca;
            }
            else
            {
                await _pageDialog.DisplayAlertAsync("Atenção", "Não foi encontrado nada com esse resultado", "OK");
            }
        }


        public event PropertyChangedEventHandler Property;
        private List<Cliente> ListaItens;

        public List<Cliente> ListaClientes
        {
            get { return ListaItens; }
            set
            {
                ListaItens = value;
                Property?.Invoke(this, new PropertyChangedEventArgs(nameof(ListaClientes)));
            }
        }

    }
}
