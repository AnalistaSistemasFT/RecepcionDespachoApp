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
    public partial class AdmRecepcion : ContentPage
    {
        List<Procedencia> ListaProcedencia = new List<Procedencia>();
        string _sprocedencia;
        int _iprocedencia;
        clsRecepcion oRecepcion = new clsRecepcion();
        string sAnotacion;

        public AdmRecepcion()
        {
            InitializeComponent();
            GetProcedencia();
            pickerProcedencia.ItemsSource = ListaProcedencia;
            pickFecha.Date = DateTime.Now;
            txtestado.Text = "OPEN";
            txtusuario.Text = App._susuario;
            sAnotacion = oRecepcion.TraerSecuencia(App._idSucursal, "ANOTACION");
            txtRecepcion.Text = sAnotacion;
            CargarListaAnotacionesAbiertas();
        }
        private void GetProcedencia()
        {
            ListaProcedencia.Add(new Procedencia { iprocedenciaid = 12094, Nombre = "PTO MAATARANI" });
            ListaProcedencia.Add(new Procedencia { iprocedenciaid = 12095, Nombre = "PTO AGUIRRE" });
            ListaProcedencia.Add(new Procedencia { iprocedenciaid = 12096, Nombre = "PTO AGESA" });
            ListaProcedencia.Add(new Procedencia { iprocedenciaid = 12097, Nombre = "PTO ARICA" });
            ListaProcedencia.Add(new Procedencia { iprocedenciaid = 12098, Nombre = "PTO ILO" });
            ListaProcedencia.Add(new Procedencia { iprocedenciaid = 12115, Nombre = "ALMACEN FOSA" });
            ListaProcedencia.Add(new Procedencia { iprocedenciaid = 12114, Nombre = "PTO. JENNEFER" });
            ListaProcedencia.Add(new Procedencia { iprocedenciaid = 12100, Nombre = "TRJ-FRONTERA-ARG" });
            ListaProcedencia.Add(new Procedencia { iprocedenciaid = 12101, Nombre = "YCB-FRONTERO-ARG" });

        }
        private void pickerProcedencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var picker = (Picker)sender;
                int selectedIndex = picker.SelectedIndex;
                if (selectedIndex != -1)
                {
                    _sprocedencia = picker.Items[selectedIndex];
                }
                foreach (var item in ListaProcedencia)
                {
                    if (_sprocedencia == item.Nombre)
                    {
                        _iprocedencia = item.iprocedenciaid;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void btnRegistrar_Clicked(object sender, EventArgs e)
        {
            oRecepcion.AnotacionId = oRecepcion.TraerSecuencia(App._idSucursal, "ANOTACION");
            oRecepcion.Chofer = "TREN";
            oRecepcion.Id_Camion = "1";
            oRecepcion.Placa = "0000-TREN";
            oRecepcion.CI = "00";
            oRecepcion.Procedencia = Convert.ToInt32(_iprocedencia);
            oRecepcion.Propietario = "S/N";
            oRecepcion.Login = App._susuario;
            oRecepcion.Obs = "S/O";
            oRecepcion.Status = "OPEN";
            oRecepcion.SucursalId = App._idSucursal;
            oRecepcion.Tipo = "001";
            oRecepcion.EsDeCliente = false;
            oRecepcion.Manifiesto = "S/M";
            oRecepcion.Fecha = DateTime.Now;
            if (oRecepcion.InsertandoAnotacion() > 0)
            {
                //await DisplayAlert("MENSAJE", "SE REGISTRO CORRECTAMENTE", "Ok");
                await Navigation.PushAsync(new RegistroProductosRecp(oRecepcion.AnotacionId));
            }
            else
                await DisplayAlert("ERROR", "PROBLEMAS EN LA TRANSACCION", "Ok");
            CargarListaAnotacionesAbiertas();
        }

        private async void listServicios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //var NewsItem = e.Item;
            //int a = e.ItemIndex;
            var detalles = e.Item as clsRecepcion;
            //await DisplayAlert("ERROR"+ a, "PROBLEMAS EN LA TRANSACCION"+ detalles.AnotacionId, "Ok");
            await Navigation.PushAsync(new RegistroProductosRecp(detalles.AnotacionId));
        }

        public void CargarListaAnotacionesAbiertas()
        {
            ListaAnotacion.ItemsSource = null;
            List<clsRecepcion> ListaAnoacion = new List<clsRecepcion>();
            clsRecepcion oDetalle = new clsRecepcion();
            var data = oDetalle.TraerAnotacionesAbiertas(App._idSucursal);
            for (int a = 0; a < data.Rows.Count; a++)
            {
                clsRecepcion oDetalle1 = new clsRecepcion();
                oDetalle1.AnotacionId = data.Rows[a]["AnotacionId"].ToString();
                oDetalle1.Propietario = data.Rows[a]["Nombre"].ToString();
                ListaAnoacion.Add(oDetalle1);
            }
            ListaAnotacion.ItemsSource = ListaAnoacion;

        }
    }
}