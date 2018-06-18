using AppClientes.DAL;
using AppClientes.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppClientes.ViewModels
{
    public class ListagemViewModel : BindableBase, INotifyPropertyChanged
    {
        public ListagemViewModel(IPageDialogService pageDialog)
        {
            Title = "Clientes Cadastrados";           
            _pageDialog = pageDialog;
            Elementos = _Elementos;
            ListaClientes = ListaItens;
            Pesquisar = "Pesquisar";
            PesquisaBD = new DelegateCommand(PesquisarBD);
            ListaSelect = new DelegateCommand(ListaClientes_ItemSelected);
        }


        

        public string Title { get; set; }
        public string Pesquisar { get; set; }        
        public Cliente ListaSelected { get; set; }
        public int ItemEscolha { get; set; }
        public List<string> _Elementos = new List<string> { "Selecione o tipo de Listagem", "Listar Cliente", "Listar por Ordenação" };
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


        public event PropertyChangedEventHandler PropertyChanged;
        private List<Cliente> ListaItens;
        public List<Cliente> ListaClientes
        {
            get { return ListaItens; }
            set
            {
                ListaItens = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ListaClientes)));
            }
        }

        public DelegateCommand ListaSelect { get; set; }
         
        public DelegateCommand PesquisaBD { get; set; }


        IPageDialogService _pageDialog;




        private void ListaClientes_ItemSelected()
        {
                if (ItemEscolha.Equals(1))
                {
                    
                    _pageDialog.DisplayAlertAsync("Detalhes do Cliente", "ID:" + ListaSelected.ClienteID + "\nNome: " + ListaSelected.Nome, "OK");
                    ListaSelected = null;                    
                    
                }
                else if(ItemEscolha.Equals(2))
                {
                    
                    _pageDialog.DisplayAlertAsync("Detalhes do Cliente", "ID:" + ListaSelected.ClienteID + "\nNome: " + ListaSelected.Nome + "\nIdade: " + ListaSelected.Idade + "\nTelefone: " + ListaSelected.Telefone, "OK");
                    ListaSelected = null;
                }            
        }



        private void PesquisarBD()
        {
            DatabaseContext contexto = new DatabaseContext();

            if (ItemEscolha.Equals(1))
            {
                var lista = contexto.Clientes.ToList();
                if (lista.Count > 0)
                {
                    ListaClientes = lista;
                }
                else
                {
                    _pageDialog.DisplayAlertAsync("Nada encontrado", "Não foi encontrado nada cadastrado", "OK");
                }
            }
            else if (ItemEscolha.Equals(2))
            {
                var listaord = (from x in contexto.Clientes
                                orderby x.Idade
                                select x).ToList();
                if (listaord.Count > 0)
                {
                    ListaClientes = listaord;
                }
                else
                {
                    _pageDialog.DisplayAlertAsync("Nada encontrado", "Não foi encontrado nada cadastrado", "OK");
                }

            }

        }
    }
}

