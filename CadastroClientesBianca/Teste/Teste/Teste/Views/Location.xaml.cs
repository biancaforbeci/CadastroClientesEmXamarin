using Xamarin.Forms;

namespace AppClientes.Views
{
    public partial class Location : ContentPage
    {
        public Location()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            Device.BeginInvokeOnMainThread(async () => {

                await this.DisplayAlert("Navegue com o menu de opções", "Utilize o menu de opções arrastando a tela lateral !", "OK");

            });

            return true; //Do not navigate backwards by pressing the button
        }
    }
}
