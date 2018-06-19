using AppClientes.DAL;
using AppClientes.Infra;
using AppClientes.Infra.Services;
using AppClientes.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppClientes.ViewModels
{
	public class CadastroViewModel : BindableBase
	{

        private readonly ClienteService _clienteService;

        public CadastroViewModel(IPageDialogService pageDialog, ClienteService ClienteService)
        {
            Title = "Cadastro Clientes";            
            TitleNome = "Nome";
            TitleIdade = "Idade";
            TitleTelefone = "Telefone";
            Cadastrar = new DelegateCommand<object>(SalvarBD);
            _pageDialog = pageDialog;
            _clienteService=ClienteService;
        }

        public string Title { get; set; }
        public string TitleNome { get; set; }
        public string TitleIdade { get; set; }
        public string TitleTelefone { get; set; }
        public string NomeCli { get; set; }
        public string IdadeCli { get; set; }
        public string TelefoneCli { get; set; }

        
        public DelegateCommand<object> Cadastrar { get; set; }
        IPageDialogService _pageDialog;

            bool valida;

            private void SalvarBD(object sender)
            {
                ValidandoDadosAsync();

                if (valida == true)
                {
                    Cliente novo = new Cliente();
                    novo.Nome = NomeCli;
                    novo.Idade = Convert.ToInt32(IdadeCli);
                    novo.Telefone = TelefoneCli;
                    SalvandoBancoAsync(novo);
                }

            }

            private void SalvandoBancoAsync(Cliente novo)
            {
                try
                {
                    _clienteService.SalvaBanco(novo);
                    _pageDialog.DisplayAlertAsync("Salvo", "Cliente salvo com sucesso", "OK");
                }
                catch (Exception e)
                {
                _pageDialog.DisplayAlertAsync("Erro", "Ocorreu um erro para salvar: " + e, "OK");
                }
            }

            private async void ValidandoDadosAsync()
            {

            string tel = "^(?:(?([0-9]{2}))?[-. ]?)?([0-9]{4})[-. ]?([0-9]{4})$";

            if ((NomeCli != null) && (IdadeCli != null) && (TelefoneCli != null))
            {
                if (!Regex.IsMatch(NomeCli, @"^[a-zA-Z]"))
                {
                    await _pageDialog.DisplayAlertAsync("ATENÇÃO", "Campo nome inválido,digite apenas caracteres !", "OK");
                }
                else if (Convert.ToInt32(IdadeCli) < 0)
                {
                    await _pageDialog.DisplayAlertAsync("ATENÇÃO", "Campo idade inválido, digite valores positivos !", "OK");
                }
                else if (Regex.IsMatch(TelefoneCli, tel) == false)
                {
                    await _pageDialog.DisplayAlertAsync("ATENÇÃO", "Campo telefone inválido ! Digite como o exemplo: 3333-3333 ou 33333333", "OK");
                }
                else
                {
                    valida = true;
                }
            }
            else
            {
                await _pageDialog.DisplayAlertAsync("Campo vazio", "Verifique se foram preenchidos todos os campos", "OK");
                await Task.Delay(500);
            }

        }
        }
    }











