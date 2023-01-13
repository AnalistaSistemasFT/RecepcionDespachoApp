using AppRecepcionDespacho.Conexion;
using AppRecepcionDespacho.Models;
using AppRecepcionDespacho.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppRecepcionDespacho.VistasDespacho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LecturarDespacho : ContentPage
    {
        Conexion.Conex CON = new Conexion.Conex();
        Conex _conexion = new Conex();
        int _cantTotal = 0;
        decimal _pesoTotal = 0;
        string _despachoId = "0";
        DateTime _FechaDesp = DateTime.Now;
        string _celdaId = "0";
        string _itemFid = "0";
        decimal _metros = 0;
        string _colada = "0";
        public LecturarDespacho(string DespachoId, DateTime _fecha)
        {
            InitializeComponent();
            _despachoId = DespachoId;
            _FechaDesp = _fecha;
            txtTitulo.Text = "Despacho: " + DespachoId.ToString();
            txtCantAct.Text = _cantTotal.ToString();
            txtPesoAct.Text = _pesoTotal.ToString();
        }
        //int cont = 0;
        private async void btnLecturar_Clicked(object sender, EventArgs e)
        {
            try
            {
                var scanner = DependencyService.Get<IQrScanningService>();
                var result = await scanner.ScanAsync();

                if (result != null)
                {
                    string _idPaquete = result;
                    Lecturar(_idPaquete);
                }
            }
            catch (Exception err)
            {  
                await DisplayAlert("Error", "No se pudo lecturar", "Ok");
                Console.WriteLine("########### = " + err.ToString());
            }
        }
        public async void Lecturar(string _idPaquete)
        {
            try
            {
                int _idSucursal = App._idSucursal;
                string _codigoPr = _idPaquete;
                //string sentencia = String.Format("SELECT a.ItemId, b.Descripcion, a.PaqueteId, a.Piezas, a.Peso FROM tblPaquetes a " +
                //"INNER JOIN tblItem b ON a.ItemId = b.ItemId WHERE a.SucursalId = " + _idSucursal + " AND a.PaqueteId = '" + _codigoPr + "'");
                string sentencia = String.Format("SELECT a.ItemId, b.Descripcion, a.PaqueteId, a.Piezas, a.Peso FROM tblPaquetes a " +
                "INNER JOIN tblItem b ON a.ItemId = b.ItemId WHERE a.PaqueteId = '" + _codigoPr + "'");
                var data = CON.ejecutarConsulta(sentencia);
                if (data.Rows.Count < 1)
                {
                    await DisplayAlert("Aviso", "Codigo de barras no registrado", "Ok");
                }
                decimal cantScan = 0;
                decimal pesoScan = 0;
                decimal cantTotal = 0;
                decimal pesoTotal = 0;
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    txtCodigo.Text = data.Rows[i][0].ToString();
                    txtDescripcion.Text = data.Rows[i][1].ToString();
                    txtPaqueteId.Text = data.Rows[i][2].ToString();
                    txtPiezas.Text = data.Rows[i][3].ToString();
                    txtPeso.Text = data.Rows[i][4].ToString();
                    cantScan = Convert.ToInt32(data.Rows[i][3]);
                    pesoScan = Convert.ToDecimal(data.Rows[i][4]);
                }
                int cantQuery = 0;
                decimal pesoQuery = 0;
                string query = String.Format("select a.PesoPaq, b.Cantidad from tblItem a INNER JOIN tblDespDetalle b ON a.ItemId = b.ItemId WHERE b.DespachoId = '" + _codigoPr + "'");
                var query_data = CON.ejecutarConsulta(query);
                for (int i = 0; i < query_data.Rows.Count; i++)
                {
                    pesoQuery = Convert.ToDecimal(query_data.Rows[i][0]);
                    cantQuery = Convert.ToInt32(query_data.Rows[i][1]);
                }
                txtCantPlan.Text = "Cantidad: " + cantQuery.ToString();
                txtPesoPlan.Text = "Peso: " + pesoQuery.ToString("0.###") + " Kgs.";
                cantTotal = cantTotal + cantScan;
                txtCantAct.Text = "Cantidad: " + cantTotal.ToString();
                pesoTotal = pesoTotal + pesoScan;
                txtPesoAct.Text = "Peso: " + pesoTotal.ToString("0.###") + " Kgs.";
                //
                string query2 = String.Format("SELECT a.CeldaId, b.ItemFId, a.Metros, a.Colada from tblPaquetes a INNER JOIN tblItem b ON a.ItemId = b.ItemId WHERE PaqueteId = '" + txtPaqueteId.Text + "'");
                var query_data2 = CON.ejecutarConsulta(query2);
                for (int i = 0; i < query_data2.Rows.Count; i++)
                {
                    _celdaId = query_data2.Rows[i][0].ToString();
                    _itemFid = query_data2.Rows[i][1].ToString();
                    _metros = Convert.ToDecimal(query_data2.Rows[i][2] is DBNull ? 0 : query_data2.Rows[i][2]);
                    _colada = query_data2.Rows[i][3].ToString();
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("Error", err.ToString(), "Ok");
                Console.WriteLine("Error ############################ = " + err.Message);
            }
        }
        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtCodigo.Text) || (!string.IsNullOrEmpty(txtCodigo.Text)))
                {
                    if (!string.IsNullOrWhiteSpace(txtDescripcion.Text) || (!string.IsNullOrEmpty(txtDescripcion.Text)))
                    {
                        if (!string.IsNullOrWhiteSpace(txtPaqueteId.Text) || (!string.IsNullOrEmpty(txtPaqueteId.Text)))
                        {
                            if (!string.IsNullOrWhiteSpace(txtPiezas.Text) || (!string.IsNullOrEmpty(txtPiezas.Text)))
                            {
                                if (!string.IsNullOrWhiteSpace(txtPeso.Text) || (!string.IsNullOrEmpty(txtPeso.Text)))
                                {
                                    string q_dup = String.Format("select count(*) as cont from tblDespProductos where ProductoId = '" + txtPaqueteId.Text + "'");
                                    var q_data = CON.ejecutarConsulta(q_dup);
                                    int _duplicado = 0;
                                    for (int i = 0; i < q_data.Rows.Count; i++)
                                    {
                                        if (Convert.ToInt32(q_data.Rows[i][0]) != 0)
                                        {
                                            _duplicado++;
                                        }
                                    }
                                    if (_duplicado == 0)
                                    {
                                        DespProductos _despProductos = new DespProductos()
                                        {
                                            DespachoId = _despachoId,
                                            ProductoId = txtPaqueteId.Text,
                                            ItemId = txtCodigo.Text,
                                            Fecha = _FechaDesp,
                                            Status = "OPEN",
                                            Cantidad = Convert.ToDecimal(txtPiezas.Text),
                                            Peso = Convert.ToDecimal(txtPeso.Text),
                                            CeldaId = _celdaId,
                                            SucursalId = App._idSucursal,
                                            ItemFId = _itemFid,
                                            Piezas = Convert.ToInt32(txtPiezas.Text),
                                            Metros = _metros,
                                            Colada = _colada
                                        };
                                        try
                                        {
                                            _conexion.GuardarDespProducto(_despProductos);
                                            bool GuardarData = _conexion.GuardarDespProducto(_despProductos);
                                            if (GuardarData == true)
                                            {
                                                try
                                                {
                                                    Vibration.Vibrate();
                                                }
                                                catch (FeatureNotSupportedException ex)
                                                {
                                                    await DisplayAlert("Agregado", ex.ToString(), "Ok");
                                                }
                                                catch (Exception err)
                                                {
                                                    Console.WriteLine("#######################= " + err.ToString());
                                                }
                                                await DisplayAlert("Agregado", "Se guardo la informacion", "Ok");
                                                //
                                                txtCodigo.Text = String.Empty;
                                                txtDescripcion.Text = String.Empty;
                                                txtPaqueteId.Text = String.Empty;
                                                txtPiezas.Text = String.Empty;
                                                txtPeso.Text = String.Empty;
                                            }
                                            else
                                            {
                                                await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
                                            }
                                        }
                                        catch (Exception err)
                                        {
                                            if (err.HResult == -2146232060)
                                            {
                                                Console.WriteLine("#######################= " + err.ToString());
                                                try
                                                {
                                                    Vibration.Vibrate();
                                                }
                                                catch (FeatureNotSupportedException ex)
                                                {
                                                    await DisplayAlert("Agregado", ex.ToString(), "Ok");
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine("#######################= " + ex.ToString());
                                                }
                                                await DisplayAlert("Agregado", "Se guardo la informacion", "Ok");
                                                //
                                                txtCodigo.Text = String.Empty;
                                                txtDescripcion.Text = String.Empty;
                                                txtPaqueteId.Text = String.Empty;
                                                txtPiezas.Text = String.Empty;
                                                txtPeso.Text = String.Empty;
                                                Console.WriteLine("#######################= " + err.Message);
                                            }
                                            else
                                            {
                                                await DisplayAlert("Error", err.ToString(), "Ok");
                                                //await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
                                                Console.WriteLine("#######################= " + err.ToString());
                                                Console.WriteLine("#######################= " + err.HResult);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        await DisplayAlert("Aviso", "Producto ya lecturado", "Ok");
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Error", "El campo de 'Peso' esta vacio", "Ok");
                                }
                            }
                            else
                            {
                                await DisplayAlert("Error", "El campo de 'Piezas' esta vacio", "Ok");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Error", "El campo de 'Paquete' esta vacio", "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "El campo de 'Descripcion' esta vacio", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "El campo de 'Codigo' esta vacio", "Ok");
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("Error", err.ToString(), "Ok");
            }
        }
    }
}