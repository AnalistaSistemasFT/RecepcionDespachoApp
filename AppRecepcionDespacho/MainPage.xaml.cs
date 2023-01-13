using AppRecepcionDespacho.Vistas;
using AppRecepcionDespacho.VistasDespacho;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppRecepcionDespacho
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void btnRecepcion_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AdmRecepcion());
        }

        private void btnDespacho_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ListaPendientes());
        }
    }
}
