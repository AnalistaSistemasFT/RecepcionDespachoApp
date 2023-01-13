using AppRecepcionDespacho.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppRecepcionDespacho.VistasDespacho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaLecturados : ContentPage
    {
        ObservableCollection<PaqueteLecturado> _listPaquetes = new ObservableCollection<PaqueteLecturado>();
        string _idPaquete = String.Empty;
        DateTime _fecha = DateTime.Now;
        Conexion.Conex CON = new Conexion.Conex();
        public ListaLecturados(string DespachoId, DateTime Fecha)
        {
            InitializeComponent();
            _idPaquete = DespachoId;
            _fecha = DateTime.Now;
            listLecturado.ItemsSource = _listPaquetes;
            TraerData();
        }
        public async void TraerData()
        {
            try
            {
                int _idSucursal = App._idSucursal;
                string _codigoPr = _idPaquete;
                //string sentencia = String.Format("SELECT a.ItemId, b.Descripcion, a.PaqueteId, a.Piezas, a.Peso FROM tblPaquetes a " +
                //"INNER JOIN tblItem b ON a.ItemId = b.ItemId WHERE a.PaqueteId = '" + _idPaquete + "'");
                string sentencia = String.Format("select a.ItemId, b.Descripcion, a.ProductoId, a.Piezas, a.Peso from tblDespProductos a INNER JOIN tblItem b ON a.ItemId = b.ItemId WHERE a.DespachoId = '" + _idPaquete + "'");
                var data = CON.ejecutarConsulta(sentencia);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    if (!_listPaquetes.Any(n => n.PaqueteId == _codigoPr))
                    {
                        _listPaquetes.Add(new PaqueteLecturado
                        {
                            ItemId = data.Rows[i][0].ToString(),
                            Descripcion = data.Rows[i][1].ToString(),
                            PaqueteId = data.Rows[i][2].ToString(),
                            Piezas = Convert.ToInt32(data.Rows[i][3]),
                            Peso = Convert.ToDecimal(data.Rows[i][4])
                        });
                    }
                    else
                    {

                    }
                }
                listLecturado.ItemsSource = _listPaquetes;
                if (_listPaquetes.Count <= 0)
                {
                    txtAviso.Text = "No existen paquetes lecturados";
                }
                else
                {
                    txtAviso.HeightRequest = 5;
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("Error", err.ToString(), "Ok");
            }
        }
        private async void btnScanner_Clicked(object sender, EventArgs e)
        {
            string DespachoId = _idPaquete;
            await Navigation.PushAsync(new LecturarDespacho(DespachoId, _fecha), true);
        }
        private void listLecturado_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //var detalles = e.Item as PaqueteLecturado;
            //await Shell.Current.Navigation.PushAsync(new ListaLecturados(detalles.DespachoId), true);
        }
    }
}