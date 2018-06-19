using AppClientes.DAL;
using AppClientes.Models;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppClientes.ViewModels
{
	public class ExcluirViewModel : BindableBase, INotifyPropertyChanged
	{
        public ExcluirViewModel(IPageDialogService pageDialog)
        {
            Title = "Excluir Clientes";            
            TitleButton = "Pesquisar";
            _pageDialog = pageDialog;            
            Procurar = new DelegateCommand(ProcurarBD);
            Elementos = _Elementos;
            ListaClientes = ListaItens;
            ListaSelect = new DelegateCommand(ListaClientes_ItemSelectedAsync);
            
        }

        private int referen = 0; 
        public string Title { get; set; }
        public string TitleTipo { get; set; }
        public string TitleButton { get; set; }        
        public int ItemEscolha { get; set; }
        public string ItemProcura { get; set; }
        IPageDialogService _pageDialog;
        public DelegateCommand Procurar { get; set; }
        public DelegateCommand ListaSelect { get; set; }
        public Cliente ListaSelected { get; set; }


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


        private async void ProcurarBD()
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
                if (referen == 0)
                {
                    await _pageDialog.DisplayAlertAsync("Atenção", "Não foi encontrado nada com esse resultado", "OK");
                    ListaClientes = busca;
                }
                else
                {
                    ListaClientes = busca;
                    referen = 0;
                }
              
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
                if (referen == 0)
                {
                    await _pageDialog.DisplayAlertAsync("Atenção", "Não foi encontrado nada com esse resultado", "OK");
                    ListaClientes = busca;
                }
                else
                {
                    ListaClientes = busca;
                    referen = 0;
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

        private async void ListaClientes_ItemSelectedAsync()
        {
            if (ListaSelected != null)
            {
                var result = await _pageDialog.DisplayAlertAsync("Deseja excluir o cliente abaixo ?", "ID:" + ListaSelected.ClienteID + "\nNome: " + ListaSelected.Nome + "\nIdade: " + ListaSelected.Idade + "\nTelefone: " + ListaSelected.Telefone, "SIM", "NÃO");
                await Task.Delay(500);
                if (result)
                {
                    ExcluiBanco(ListaSelected.ClienteID);
                    referen = 1;
                    if (ItemEscolha.Equals(1))
                    {
                        ProcuraPorIDAsync();
                    }
                    else if(ItemEscolha.Equals(2))
                    {
                        ProcuraPorNomeAsync();
                    }
                }                
            }
        }

        private async void ExcluiBanco(int idCliente)
        {
            try
            {
                DatabaseContext contexto = new DatabaseContext();
                Cliente c = contexto.Clientes.Find(idCliente);
                contexto.Entry(c).State = EntityState.Deleted;
                contexto.SaveChanges();
                await _pageDialog.DisplayAlertAsync("Exclusão concluída", "Cliente excluído com sucesso ", "OK");
            }
            catch (Exception)
            {
                await _pageDialog.DisplayAlertAsync("Erro", "Cliente já excluído", "OK");
            }

        }




    }
}
