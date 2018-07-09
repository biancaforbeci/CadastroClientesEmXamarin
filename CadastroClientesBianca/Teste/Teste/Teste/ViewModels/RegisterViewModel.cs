using Android.Media;
using AppClientes.Infra;
using AppClientes.Infra.Services;
using AppClientes.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppClientes.ViewModels
{
	public class RegisterViewModel : BindableBase, INotifyPropertyChanged
    {
        private readonly IService _clienteService;
        private readonly IFileSystem _fileSystem;
        public RegisterViewModel(IPageDialogService pageDialog, IService clienteService, IFileSystem fileSystem)
        {
            Title = "Cadastro Clientes";
            TitleName = "Nome";
            TitleAge = "Idade";
            TitlePhone = "Telefone";
            Register = new DelegateCommand<object>(SavingClient);
            _pageDialog = pageDialog;
            _clienteService = clienteService;
            AddPhoto = new DelegateCommand(AcessCameraAsync);
            ChoosePhoto = new DelegateCommand(ChoosePhotoAlbumAsync);
            _fileSystem = fileSystem;
            Photo= "drawable-xhdpi/person.png";
        }
        

        public string Title { get; set; }
        public string TitleName { get; set; }
        public string TitleAge { get; set; }
        public string TitlePhone { get; set; }
        private string Name { get; set; }
        private string Age { get; set; }
        private string Phone { get; set; }
        public string NameCli {
            get { return Name; }
            set {
                Name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NameCli)));
            }
        }
        public string AgeCli {
            get { return Age; }
            set {
                Age = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AgeCli)));
            }
        }
        public string PhoneCli {
            get { return Phone; }
            set {
                Phone = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PhoneCli)));
            }
        }
        private string Path_Photo = null;
        public event PropertyChangedEventHandler PropertyChanged;
        public string Photo
        {
            get { return Path_Photo; }
            set
            {
                Path_Photo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Photo)));
            }
        }


        public DelegateCommand<object> Register { get; set; }
        public DelegateCommand AddPhoto { get; set; }
        public DelegateCommand ChoosePhoto { get; set; }
        IPageDialogService _pageDialog;

        bool x;

        private void SavingClient(object sender)
        {
            Validate();

            if (x == true)
            {
                Client c = new Client();
                c.Name = NameCli;
                c.Age = Convert.ToInt32(AgeCli);
                c.Phone = PhoneCli;
                if(!Path_Photo.Equals("drawable-xhdpi/person.png"))
                {
                    c.PathPhoto = Path_Photo;
                    SavingDB(c);
                    SavePhotoLocalFile(Path_Photo);   
                    Clean();
                }
                else
                {
                    _pageDialog.DisplayAlertAsync("Foto não adicionada", "Confirme uma foto para o cliente !", "OK");                    
                }              
            }
        }

        private void SavingDB(Client c)
        {
            try
            {
                if(_clienteService.SaveClient(c) == true)
                {
                    _pageDialog.DisplayAlertAsync("Salvo", "Cliente salvo com sucesso", "OK");
                }              
                                 
            }
            catch (Exception e)
            {
                _pageDialog.DisplayAlertAsync("Erro", "Ocorreu um erro para salvar: " + e, "OK");
            }
        }

        private async void Validate()
        {

            string tel = "^(?:(?([0-9]{2}))?[-. ]?)?([0-9]{4})[-. ]?([0-9]{4})$";

            if ((NameCli != null) && (AgeCli != null) && (PhoneCli != null))
            {
                if (!Regex.IsMatch(NameCli, @"^[a-zA-Z]"))
                {
                    await _pageDialog.DisplayAlertAsync("ATENÇÃO", "Campo nome inválido,digite apenas caracteres !", "OK");
                }
                else if (Convert.ToInt32(AgeCli) < 0)
                {
                    await _pageDialog.DisplayAlertAsync("ATENÇÃO", "Campo idade inválido, digite valores positivos !", "OK");
                }
                else if (Regex.IsMatch(PhoneCli, tel) == false)
                {
                    await _pageDialog.DisplayAlertAsync("ATENÇÃO", "Campo telefone inválido ! Digite como o exemplo: 3333-3333 ou 33333333", "OK");
                }
                else
                {
                    x = true;
                }
            }
            else
            {
                await _pageDialog.DisplayAlertAsync("Campo vazio", "Verifique se foram preenchidos todos os campos", "OK");
            }
        }

        private async void ChoosePhotoAlbumAsync()
        {
            ExistsDirectory();

            var media = CrossMedia.Current;

            var file = await media.PickPhotoAsync();

            SavePhotoToClientAsync(file);
        }

        private async void AcessCameraAsync()
        {
            await CrossMedia.Current.Initialize();

            ExistsDirectory();          

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await _pageDialog.DisplayAlertAsync("Nenhuma câmera", "Nenhuma câmera disponível.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                SaveToAlbum=true,
                Directory = "Photos_Clients",
                Name = string.Format("Photo_Client_{0}", DateTime.Now.ToString("yyMMddhhmmss")),
                PhotoSize = PhotoSize.Custom,CustomPhotoSize=100,
                CompressionQuality=100
            });
           

            if (file == null)
                return;            

            SavePhotoToClientAsync(file);
        }

        private async void SavePhotoToClientAsync(MediaFile file)
        {
            var result = await _pageDialog.DisplayAlertAsync("Confirmar foto", "Deseja salvar essa foto para o cliente ?", "SIM", "NÃO");

            if (result)
            {
                Photo = file.Path;                
            }
            else
            {
                Photo = "drawable-xhdpi/person.png";
            }
        }

        private void CreateDirectory()
        {
            var documents = _fileSystem.GetStoragePath();
            var directoryname = Path.Combine(documents, "Photos");
            Directory.CreateDirectory(directoryname);            
        }   

        private void ExistsDirectory()
        {
            if (_fileSystem.DirectoryExists(Path.Combine(_fileSystem.GetStoragePath(), "Photos")) == false)
            {
                CreateDirectory();
            }
        }

        private void SavePhotoLocalFile(string path)
        {
            string destiny = Path.Combine(_fileSystem.GetStoragePath(), "Photos");
            string imageLocal = Path.Combine(destiny, Path.GetFileName(path));
            File.Copy(path, imageLocal);
        }

        private void Clean()
        {
            NameCli = null;
            AgeCli = null;
            PhoneCli = null;
            Photo="drawable-xhdpi/person.png";
        }
    }
 }
