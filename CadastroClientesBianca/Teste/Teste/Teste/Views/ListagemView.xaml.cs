using System;
using Xamarin.Forms;

namespace AppClientes.Views
{
    public partial class ListagemView : ContentPage
    {
        public ListagemView _ListagemView { get; set; }

        public ListagemView()
        {
            InitializeComponent();
            _ListagemView = new ListagemView();
            BindingContext = _ListagemView;
            
        }

       
        
    }
}
