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
            Image= "lista.png";
            _fileSystem = fileSystem;
            _service = service;
            _pageDialog = pageDialog;
        }

        public string ImportTitle { get; set; }
        public string ExportTitle { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        IPageDialogService _pageDialog;
        public DelegateCommand Import { get; set; }
        public DelegateCommand Export { get; set; }
        public int CountClients = 0;
        private string pathFile;

        private void CreateDirectory()
        {
            var documents = _fileSystem.GetStoragePath();
            var directoryname = Path.Combine(documents, "List JSON");
            Directory.CreateDirectory(directoryname);
            pathFile= directoryname;
        }

        private async void ImportListAsync()
        {
            try
            {               
                var filename = Path.Combine(pathFile, "clients.json");
                if (SearchFile() == true)
                {
                    var dataJson = File.ReadAllText(filename);
                    Deserialize(dataJson);
                }
                else
                {
                    await _pageDialog.DisplayAlertAsync("Erro", "Não foi encontrado o arquivo JSON !", "OK");
                }                
            }
            catch
            {
                throw;
            }   
        }

        private void ExportList()
        {            
            try
            {
                CountClients = ListingDB().Count;
                if (SearchFile() == false)
                {
                    CreateDirectory();
                    Serialize();
                    ExportNotificationAsync(true);
                }
                else {
                    Serialize();
                    ExportNotificationAsync(true);
                }                
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

        private void Serialize()
        {
            string json = JsonConvert.SerializeObject(ListingDB());
            File.WriteAllText(Path.Combine(pathFile, "clients.json"), json);
            ExportNotificationAsync(true);
        }

        private void Deserialize(string dataJson)
        {            
            IEnumerable<Client> result = JsonConvert.DeserializeObject<IEnumerable<Client>>(dataJson);
            ImportNotificationAsync(result);
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
                await _pageDialog.DisplayAlertAsync("Importação", "Importação realizada com sucesso ! Importado: "+ CountClients+ " clientes", "OK");
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
                await _pageDialog.DisplayAlertAsync("Exportação", "Exportação realizada com sucesso ! Exportado: " +CountClients+ " clientes", "OK");
            }
            else
            {
                await _pageDialog.DisplayAlertAsync("Erro", "Tente novamente a exportação !", "OK" );
            }
        }


        private bool SearchFile()
        {
            var directoryname = Path.Combine(_fileSystem.GetStoragePath(), "List JSON");
            var filename = Path.Combine(directoryname, "clients.json");
            if (_fileSystem.ListFilesAsync(filename) == null)
            {
                return false;
            }
            else
            {
                pathFile = filename;
                return true;
            }
        }

    }
}
