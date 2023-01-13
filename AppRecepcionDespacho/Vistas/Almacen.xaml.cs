using AppRecepcionDespacho.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppRecepcionDespacho.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Almacen : ContentPage
    {
        public Almacen()
        {
            InitializeComponent();
            CargarAlmacenes();
        }
        public void CargarAlmacenes()
        {
            ListaAlmacen.ItemsSource = null;
            List<Usuario> ListaAlmacenes = new List<Usuario>();
            Usuario oDetalle = new Usuario();
            var data = oDetalle.TraerAlmacenPorUsuario(App._susuario);
            for (int a = 0; a < data.Rows.Count; a++)
            {
                Usuario oDetalle1 = new Usuario();
                oDetalle1.SucursalId = Convert.ToInt32(data.Rows[a]["SucursalId"].ToString());
                oDetalle1.AlmacenId = Convert.ToInt32(data.Rows[a]["AlmacenId"].ToString());
                oDetalle1.Nombre = data.Rows[a]["Nombre"].ToString();
                ListaAlmacenes.Add(oDetalle1);
            }
            ListaAlmacen.ItemsSource = ListaAlmacenes;
        }
        private async void ListaAlmacen_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var detalles = e.Item as Usuario;
            if (detalles != null)
            {
                App._idSucursal = detalles.SucursalId;
                App._idAlmacen = detalles.AlmacenId;
                //await Navigation.PushAsync(new AdmRecepcion());
                await Navigation.PushAsync(new MainPage());
                //Application.Current.MainPage = new MainPage();
            }
            else
                await DisplayAlert("ERROR", "PROBLEMAS EN LA TRANSACCION", "Ok");

        }
    }
}