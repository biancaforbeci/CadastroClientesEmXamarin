using AppClientes.DAL;
using AppClientes.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AppClientes.ViewModels
{
	public class CadastroViewModel : BindableBase
	{
        public CadastroViewModel()
        {
            Title = "Cadastro Clientes";
            TitleNome = "Nome";
            TitleIdade = "Idade";
            TitleTelefone = "Telefone";
            Cadastrar = new DelegateCommand<object>(SalvarBD);
        }

        public string Title { get; set; }
        public string TitleNome { get; set; }
        public string TitleIdade { get; set; }
        public string TitleTelefone { get; set; }
        public string NomeCli { get; set; }
        public string IdadeCli { get; set; }
        public string TelefoneCli { get; set; }

        
        public DelegateCommand<object> Cadastrar { get; set; }      
       

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
                    DatabaseContext contexto = new DatabaseContext();
                    contexto.Add(novo);
                    contexto.SaveChanges();
                   // await DisplayAlert("Salvo", "Cliente salvo com sucesso", "OK");
                }
                catch (Exception e)
                {
                   // await DisplayAlert("Erro", "Ocorreu um erro para salvar: " + e, "OK");
                }
            }

            private void ValidandoDadosAsync()
            {

            string tel = "^(?:(?([0-9]{2}))?[-. ]?)?([0-9]{4})[-. ]?([0-9]{4})$";

            if ((NomeCli != null) && (IdadeCli != null) && (TelefoneCli != null))
            {
                if (!Regex.IsMatch(NomeCli, @"^[a-zA-Z]+$"))
                {
                  //  await DisplayAlert("ATENÇÃO", "Campo nome inválido,digite apenas caracteres !", "OK");
                }
                else if (Convert.ToInt32(IdadeCli) < 0)
                {
                  //  await DisplayAlert("ATENÇÃO", "Campo idade inválido, digite valores positivos !", "OK");
                }
                else if (Regex.IsMatch(TelefoneCli, tel) == false)
                {
                   // await DisplayAlert("ATENÇÃO", "Campo telefone inválido ! Digite como o exemplo: 3333-3333 ou 33333333", "OK");
                }
                else
                {
                    valida = true;
                }
            }
            else
            {
              //  await DisplayAlert("Campo vazio", "Verifique se foram preenchidos todos os campos", "OK");
               // await Task.Delay(500);
            }

        }
        }
    }











