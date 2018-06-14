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
using Xamarin.Forms;

namespace AppClientes.ViewModels
{
    public class ListagemViewModel : BindableBase, INotifyPropertyChanged
    {
        public ListagemViewModel(IPageDialogService pageDialog)
        {
             Title = "Clientes Cadastrados";
            TitleTipo = "Selecione o tipo:";
            ImagemList = "drawable-xhdpi/person.png";
            TitleLista = "Lista Clientes";            
            TitleLista2 = "Lista Clientes por Ordenação";
            _pageDialog = pageDialog;
            Elementos = _Elementos;
        }
        

        private void ItemSelecionado()
        {
            throw new NotImplementedException();
        }

        public string Title { get; set; }
        public string TitleTipo { get; set; }
        public string ImagemList { get; set; }
        public string TitleLista { get; set; }
        public string TitleLista2 { get; set; }        
        public Cliente ListaSelected { get; set; }
        public int ItemEscolha { get; set; }
        public List<string> _Elementos = new List<string> {"Listar Cliente","Listar por Ordenação"};
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
                    
                    //SelectedCountry = Countries[countriesSelectedIndex];
                }
            }
        }




        public DelegateCommand SelectedProviderChanged { get; set; }
        public DelegateCommand OpcaoSelect { get; set; }


        IPageDialogService _pageDialog;
        public ObservableCollection<Cliente> ListaClientes { get; set; } = new ObservableCollection<Cliente>();


       

        //private void ListaClientes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    if (e.SelectedItem != null)
        //    {
        //        if (escolha.Equals("Listar Clientes"))
        //        {
        //            var element = e.SelectedItem as Cliente;
        //            _pageDialog.DisplayAlertAsync("Detalhes do Cliente", "ID:" + element.ClienteID + "\nNome: " + element.Nome, "OK");
        //            ListaClientes.SelectedItem = null;
        //        }
        //        else
        //        {
        //            var element = e.SelectedItem as Cliente;
        //            _pageDialog.DisplayAlertAsync("Detalhes do Cliente", "ID:" + element.ClienteID + "\nNome: " + element.Nome + "\nIdade: " + element.Idade + "\nTelefone: " + element.Telefone, "OK");
        //            ListaClientes.SelectedItem = null;
        //        }
        //    }
        //}



        private void pckBusca_SelectedIndexChanged()
        {
            DatabaseContext contexto = new DatabaseContext();
            
            if (ItemEscolha.Equals("Listar Clientes"))
            {
                var lista = contexto.Clientes.ToList();
                if (lista.Count > 0)
                {
                    ObservableCollection<Cliente> ListaItens = new ObservableCollection<Cliente>(lista);
                   
                }
                else
                {
                    _pageDialog.DisplayAlertAsync("Nada encontrado", "Não foi encontrado nada cadastrado", "OK");
                }
            }
            else
            {
                var listaord = (from x in contexto.Clientes
                                orderby x.Idade
                                select x).ToList();
                if (listaord.Count > 0)
                {
                    ObservableCollection<Cliente> ListaItens1 = new ObservableCollection<Cliente>(listaord);
                    
                }
                else
                {
                    _pageDialog.DisplayAlertAsync("Nada encontrado", "Não foi encontrado nada cadastrado", "OK");
                }

            }

        }



    }
}

