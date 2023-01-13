using AppRecepcionDespacho.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppRecepcionDespacho.VistasDespacho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaPendientes : ContentPage
    {
        Conexion.Conex CON = new Conexion.Conex();
        List<Models.Despacho> _listDespachos = new List<Models.Despacho>();
        int _idSucursal = App._idSucursal;
        public ListaPendientes()
        {
            InitializeComponent();
            TraerData();
        }
        async void TraerData()
        {
            try
            {
                //string sentencia = String.Format("select tblDespacho.despachoid,tblDespacho.fecha,tbldespacho.nroorden,tblDespacho.Placa,tblcatcamion.Marca,tblcatchofer.nombre as chofer, tblDespacho.Naturaleza,DESTINO = Case Naturaleza When 'TRASPASO' THEN(SELECT NOMBRE FROM empleados.dbo.TBLSUCURSAL WHERE SUCURSALID = tblDespacho.SUCDestino) Else TBLDESPACHO.Destino END, tblDespacho.numtraspaso,tipo,tblDespacho.status,tblDespacho.LOGIN from(tblDespacho left join tblcatchofer on tbldespacho.ci= tblcatchofer.ci) left join tblcatcamion on tbldespacho.placa = tblcatcamion.placa where sucursalid = '" + _idSucursal + "' AND STATUS IN('INPROCESS') order by tblDespacho.despachoid");
                string sentencia = String.Format("select DespachoId, Fecha, Status, Naturaleza FROM tblDespacho where Status IN('INPROCESS') AND SucursalId = " + _idSucursal + " order by Despachoid");
                var data = CON.ejecutarConsulta(sentencia);
                int count = 0;
                foreach (var item in data.Rows)
                {
                    count++;
                }
                for (int i = 0; i < count; i++)
                {
                    Despacho _despacho = new Despacho();
                    _despacho.DespachoId = data.Rows[i][0].ToString();
                    _despacho.Fecha = Convert.ToDateTime(data.Rows[i][1]);
                    _despacho.Status = data.Rows[i][2].ToString();
                    _despacho.Naturaleza = data.Rows[i][3].ToString();

                    _listDespachos.Add(_despacho);
                }
                if (_listDespachos.Count <= 0)
                {
                    txtAviso.Text = "No existen ordenes pendientes";
                }
                else
                {
                    txtAviso.HeightRequest = 10;
                }
                listPendientes.ItemsSource = _listDespachos;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.ToString(), "Ok");
            }
        }
        private async void listPendientes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var detalles = e.Item as Despacho;
            await Navigation.PushAsync(new ListaLecturados(detalles.DespachoId, detalles.Fecha));
        }
    }
}