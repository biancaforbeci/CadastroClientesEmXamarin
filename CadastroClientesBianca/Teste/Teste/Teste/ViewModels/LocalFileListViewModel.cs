using AppClientes.Infra;
using AppClientes.Infra.Services;
using AppClientes.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace AppClientes.ViewModels
{
    public class LocalFileListViewModel : BindableBase
	{
        private readonly IFileSystem _fileSystem;
        private readonly IService _service;

        public LocalFileListViewModel(IFileSystem fileSystem, IService service, IPageDialogService pageDialog)
        {
            Title = "Importar e Exportar Em JSON";
            ImportTitle = "Importar";
            ExportTitle = "Exportar";
            Import = new DelegateCommand(ImportListAsync);
            Export = new DelegateCommand(ExportList);
            Image = "lista.png";
            _fileSystem = fileSystem;
            _service = service;
            _pageDialog = pageDialog;
        }

        public string ImportTitle { get; set; }
        public string ExportTitle { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public bool x=false;
        IPageDialogService _pageDialog;
        public DelegateCommand Import { get; set; }
        public DelegateCommand Export { get; set; }
        public int CountClients = 0;

        private String CreateDirectory()
        {
            var documents = _fileSystem.GetStoragePath();
            var directoryname = Path.Combine(documents, "List JSON");
            Directory.CreateDirectory(directoryname);
            return directoryname;
        }

        private async void ImportListAsync()
        {
            try
            {
                var documents = CreateDirectory();
                var filename = Path.Combine(documents, "clients.json");
                if (SearchFile(filename) == false)
                {
                    await _pageDialog.DisplayAlertAsync("ATENÇÃO", "Crie um arquivo JSON com nome clients.JSON no diretório List JSON ou cadastre clientes e exporte", "OK");
                }
                else
                {
                    var dataJson = File.ReadAllText(filename);
                    if (dataJson == null)
                    {
                        await _pageDialog.DisplayAlertAsync("Arquivo vazio", "Esse arquivo não possui nenhuma informação", "OK");
                    }
                    IEnumerable<Client> result = JsonConvert.DeserializeObject<IEnumerable<Client>>(dataJson);
                    ValidationAsync(result);
                    if (x)
                    {
                        ImportNotificationAsync(result);
                    }
                }               
            }
            catch
            {
                throw;
            }
        }

        private async void ValidationAsync(IEnumerable<Client> result)
        {
            string tel = "^(?:(?([0-9]{2}))?[-. ]?)?([0-9]{4})[-. ]?([0-9]{4})$";
            foreach (var item in result)
            {
                if (( item.Name != null) && (item.Age.ToString() != null) && (item.Phone != null))
                {
                    if (!Regex.IsMatch(item.Name, @"^[a-zA-Z]"))
                    {
                        await _pageDialog.DisplayAlertAsync("ATENÇÃO", "Campo nome inválido no item de ID : " + item.ClientID + " Digite apenas caracteres !", "OK");
                    }
                    else if (Convert.ToInt32(item.Age) < 0)
                    {
                        await _pageDialog.DisplayAlertAsync("ATENÇÃO", "Campo idade inválido no item de ID : " + item.ClientID + " Digite valores positivos !", "OK");
                    }
                    else if (Regex.IsMatch(item.Phone, tel) == false)
                    {
                        await _pageDialog.DisplayAlertAsync("ATENÇÃO", "Campo telefone inválido no item de ID : " + item.ClientID + " Digite como o exemplo: 3333-3333 ou 33333333", "OK");
                    }
                    else
                    {
                        x = true;
                    }
                }
                else
                {
                    await _pageDialog.DisplayAlertAsync("Campo vazio", "Verifique se foram preenchidos todos os campos do item: " + item.ClientID , "OK");
                }
            }            
        }

        private void ExportList()
        {
            string documents;

            try
            {
                string json = JsonConvert.SerializeObject(ListingDB());
                CountClients = ListingDB().Count;                
                var directoryname = Path.Combine(_fileSystem.GetStoragePath(), "List JSON");
                if(SearchDirectory(directoryname) == false)
                {
                    documents = CreateDirectory();
                }                
                var filename = Path.Combine(directoryname, "clients.json");
                File.WriteAllText(filename, json);
                ExportNotificationAsync(true);
            }
            catch
            {
                ExportNotificationAsync(false);
            }
        }

        private List<Client> ListingDB()
        {
            return _service.AllClient();
        }

        private bool UpdateDB(IEnumerable<Client> listJSON)
        {
            try
            {
                foreach (var i in listJSON)
                {
                    _service.SaveClient(i);
                    CountClients++;
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        private async void ImportNotificationAsync(IEnumerable<Client> listClients)
        {
            if (UpdateDB(listClients))
            {
                await _pageDialog.DisplayAlertAsync("Importação", "Importação realizada com sucesso ! Importado: " + CountClients + " clientes", "OK");
            }
            else
            {
                await _pageDialog.DisplayAlertAsync("Erro", "Erro ao salvar no banco, tente novamente !", "OK");
            }
        }


        private async void ExportNotificationAsync(bool response)
        {
            if (response)
            {
                await _pageDialog.DisplayAlertAsync("Exportação", "Exportação realizada com sucesso ! Exportado: " + CountClients + " clientes", "OK");
            }
            else
            {
                await _pageDialog.DisplayAlertAsync("Erro", "Tente novamente a exportação !", "OK");
            }
        }

        private bool SearchFile(string filename)
        {           
            if (_fileSystem.FileExists(filename))
            {
                return true;
            }
            else
            {
                return false;
            }         
        }

        private bool SearchDirectory(string filename)
        {
            if (_fileSystem.DirectoryExists(filename))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}