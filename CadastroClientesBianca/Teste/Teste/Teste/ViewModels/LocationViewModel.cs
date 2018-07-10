using Plugin.ExternalMaps;
using Plugin.Geolocator;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppClientes.ViewModels
{
	public class LocationViewModel : BindableBase
	{
        public LocationViewModel(IPageDialogService pageDialog)
        {
            Title = "Posição Atual";
            TitleLocalization = "Obter Geolocalização: Latitude / Longitude";
            TitlePosition = "Exibir posição no Mapa";
            _pageDialog = pageDialog;
            Localization = new DelegateCommand(GetPositionAsync);
            Position = new DelegateCommand(ShowPositionAsync);
            Image = "localizacao.png";
        }

        private double latitude;
        private double longitude;
        public string Title { get; set; }
        public string TitleLocalization { get; set; }
        public string TitlePosition { get; set; }
        public string Image { get; set; }
        IPageDialogService _pageDialog;


        public DelegateCommand Localization { get; set; }
        public DelegateCommand Position { get; set; }

        private async void GetPositionAsync()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                latitude = position.Latitude;
                longitude = position.Longitude;
                await _pageDialog.DisplayAlertAsync("Detalhes da Localização", "\nStatus: " + position.Timestamp + "\nLatitude: " + position.Latitude + "\nLongitude: " + position.Longitude, "OK");
            }
            catch (Exception ex)
            {
                await _pageDialog.DisplayAlertAsync("Erro : ", ex.Message, "OK");
            }
            
        }

        private async void ShowPositionAsync()
        {
            try
            {
                await CrossExternalMaps.Current.NavigateTo("Posição Atual", latitude, longitude);
            }
            catch (Exception ex)
            {
                await _pageDialog.DisplayAlertAsync("Erro : ", ex.Message, "OK");
            }
        }
    }
}
