using Android.Webkit;
using AppClientes.Infra;
using AppClientes.Infra.Api;
using AppClientes.Infra.Services;
using AppClientes.Models;
using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        private readonly IApiClient _apiClient;
        public LocalFileListViewModel(IFileSystem fileSystem, IService service, IPageDialogService pageDialog, IApiClient apiClient)
        {
            Title = "Importar e Exportar Em JSON";
            ImportTitle = "Importar";
            ExportTitle = "Exportar";
            TitleURL = "Coloque a URL para importação abaixo";
            TitleExport = "Exportar clientes cadastrados para pasta local";
            Import = new DelegateCommand(ImportListAsync);
            Export = new DelegateCommand(ExportListAsync);
            Image = "lista.png";
            _fileSystem = fileSystem;
            _service = service;
            _pageDialog = pageDialog;
            _apiClient = apiClient;
        }

        public string ImportTitle { get; set; }
        public string ExportTitle { get; set; }
        public string Title { get; set; }
        public string TitleURL { get; set; }
        public string TitleExport { get; set; }
        public string URLImport { get; set; }
        public string Image { get; set; }
        public int x = 0;
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
            if ((URLUtil.IsValidUrl(URLImport) == false) || (URLImport == null))
            {
                await _pageDialog.DisplayAlertAsync("ATENÇÃO", "Verifique a inserção da URL !", "OK");
            }
            else
            {
                ImportAPIAsync(URLImport);
            }
        }

        private async void ValidationImportAsync(IEnumerable<Client> result)
        {
            string tel = "^(?:(?([0-9]{2}))?[-. ]?)?([0-9]{4})[-. ]?([0-9]{4})$";           

            foreach (var item in result)
            {
                if ((item.ClientID != 0) && (item.Name != null) && (item.Age.ToString() != null) && (item.Phone != null))
                {
                    if (!Regex.IsMatch(item.ClientID.ToString(), "^[0-9]"))
                    {
                        x++;
                        await _pageDialog.DisplayAlertAsync("ATENÇÃO", "ClientID inválido no campo: " + item.ClientID + " Digite apenas números !", "OK");
                    }
                    else if (!Regex.IsMatch(item.Name, @"^[a-zA-Z]"))
                    {
                        x++;
                        await _pageDialog.DisplayAlertAsync("ATENÇÃO", "Campo nome inválido no item de ID : " + item.ClientID + " Digite apenas caracteres !", "OK");

                    }
                    else if ((Convert.ToInt32(item.Age) < 0) || (!Regex.IsMatch(item.Age.ToString(), "^[0-9]")))
                    {
                        x++;
                        await _pageDialog.DisplayAlertAsync("ATENÇÃO", "Campo idade inválido no item de ID : " + item.ClientID + " Digite valores numéricos positivos !", "OK");
                    }
                    else if (Regex.IsMatch(item.Phone, tel) == false)
                    {
                        x++;
                        await _pageDialog.DisplayAlertAsync("ATENÇÃO", "Campo telefone inválido no item de ID : " + item.ClientID + " Digite como o exemplo: 3333-3333 ou 33333333", "OK");
                    }
                }
                else
                {
                    x++;
                    await _pageDialog.DisplayAlertAsync("Campo vazio ou inválido", "Verifique se foram preenchidos todos os campos corretamente do item de ID: " + item.ClientID, "OK");
                }
            }
        }

        private async void ImportAPIAsync(string uri)
        {
            try
            {
                IEnumerable<Client> list = await _apiClient.GetAsync(uri);
                
                if (list != null)
                {
                    ValidationImportAsync(list);
                    if (x == 0)
                    {
                        x = 0;
                        ExportJSON_API(Path.Combine(_fileSystem.GetStoragePath(), "List JSON"), _apiClient.Read_JSON());  //exporta para pasta local JSON.
                        ImportNotificationAsync(list);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private async void CompressionGZipAsync(string list)
        {
            try
            {
                _apiClient.CompressionGZIPAsync(list);
            }catch(Exception e)
            {
                await _pageDialog.DisplayAlertAsync("Erro","Erro: " + e,"OK");
            }           
        }

        private void ExportListAsync()
        {
            string documents;           

            try
            {
                string json = JsonConvert.SerializeObject(ListingDB());
                CountClients = ListingDB().Count;

                PostServerAsync(json);
                var directoryname = Path.Combine(_fileSystem.GetStoragePath(), "List JSON");
                if (SearchDirectory(directoryname) == false)
                {
                    documents = CreateDirectory();
                }
                var filename = Path.Combine(directoryname, "clients.JSON");
                File.WriteAllText(filename, json);                
                ExportNotificationAsync(true);
            }
            catch
            {
                ExportNotificationAsync(false);
            }
        }

        private async void PostServerAsync(string json)
        {
            string compressedString = _apiClient.CompressionGZIPAsync(json);   //compression GZIP                

            try
            {
                _apiClient.PostAsync(ListingDB());              //enviando para servidor   

            }
            catch (Exception e)
            {
                await _pageDialog.DisplayAlertAsync("Erro", "Erro: " + e, "OK");
            }       
        }

        private void ExportJSON_API(string directoryname, string json)
        {
            if (SearchDirectory(directoryname) == false)
            {
                CreateDirectory();
            }
            bool request;
            do
            {
                string number = RandomNumberFile() + ".json";
                var filename = Path.Combine(directoryname, number);
                if (SearchFile(filename) == false)
                {
                    File.WriteAllText(filename, json);
                    _pageDialog.DisplayAlertAsync("Exportado JSON da API", "Exportado arquivo JSON da URL para arquivo de nome: " + number + " na pasta local.", "OK");
                    request = true;
                }
                else
                {
                    request = false;
                }
            } while (request == false);
        }  
              
        private List<Client> ListingDB()
        {
            return _service.AllClient();
        }

        private string RandomNumberFile()
        {
            Random num = new Random();
            return num.Next(0, 7000).ToString();
        }

        private bool UpdateDB(IEnumerable<Client> listJSON)
        {
            try
            {
                foreach (var i in listJSON)
                {
                    if (_service.SearchID(i.ClientID).Count > 0)
                    {
                        i.ClientID = _service.LastID() + 1;
                    }
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
                await _pageDialog.DisplayAlertAsync("Importação", "Importação realizada com sucesso !", "OK");
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
                await _pageDialog.DisplayAlertAsync("Exportação", "Exportação realizada com sucesso ! Exportado: " + CountClients + " clientes para arquivo clients.json na pasta local.", "OK");
                CountClients = 0;
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

        private void CreateDirectoryPhotos()
        {
            var documents = _fileSystem.GetStoragePath();
            var directoryname = Path.Combine(documents, "Photos");
            Directory.CreateDirectory(directoryname);
        }

        private void ExistsDirectoryPhotos()
        {
            if (_fileSystem.DirectoryExists(Path.Combine(_fileSystem.GetStoragePath(), "Photos")) == false)
            {
                CreateDirectory();
            }
        }
    }
}

        