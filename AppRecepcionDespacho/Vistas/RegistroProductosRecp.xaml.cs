using AppRecepcionDespacho.Models;
using AppRecepcionDespacho.Service;
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
    public partial class RegistroProductosRecp : ContentPage
    {
        List<ProductoRecep> ListaProductos = new List<ProductoRecep>();
        List<Fabricante> ListaFabricante = new List<Fabricante>();
        List<Celdas> ListaCeldas = new List<Celdas>();
        string AnotacionId;
        SysSecuencia oSecuencia = new SysSecuencia();
        string _Fabricante;
        string _Celda;
        public RegistroProductosRecp(string sAnotacion)
        {
            InitializeComponent();
            AnotacionId = sAnotacion;
            lblAnotacion.Text = "Anotación: " + sAnotacion;
            GetFabricantes();
            CargarAnotacionesDetalle();
            GetCeldas();
        }
        private void GetFabricantes()
        {
            try
            {
                Fabricante oFabricante = new Fabricante();
                var dataResul = oFabricante.TraerFabricantes();
                for (int a = 0; a < dataResul.Rows.Count; a++)
                {
                    oFabricante = new Fabricante();
                    oFabricante.ProveedorId = Convert.ToInt32(dataResul.Rows[a]["ProveedorId"].ToString());
                    oFabricante.Nombre = dataResul.Rows[a]["Nombre"].ToString();
                    ListaFabricante.Add(oFabricante);
                }
                pickerFabricante.ItemsSource = ListaFabricante;
            }
            catch(Exception err)
            {
                DisplayAlert("Error", "Algo salio mal, intentelo nuevamente", "Ok");
                Console.WriteLine("################## = " + err.ToString());
            } 
        }
        private void GetCeldas()
        {
            try
            {
                Celdas oCelda = new Celdas();
                var dataResul = oCelda.TraerCeldas(App._idAlmacen);
                for (int a = 0; a < dataResul.Rows.Count; a++)
                {
                    oCelda = new Celdas();
                    oCelda.CeldaId = dataResul.Rows[a]["CeldaId"].ToString();

                    ListaCeldas.Add(oCelda);
                }
                pickerCelda.ItemsSource = ListaCeldas;
            }
            catch (Exception err)
            {
                DisplayAlert("Error", "Algo salio mal, intentelo nuevamente", "Ok");
                Console.WriteLine("################## = " + err.ToString());
            }
        }
        private async void btnScan_Clicked(object sender, EventArgs e)
        {
            try
            {
                var scanner = DependencyService.Get<IQrScanningService>();
                var result = await scanner.ScanAsync();
                if (result != null)
                {
                    txtBarcode.Text = result;
                    string sResult = result;
                    txtspacking.Text = sResult.Substring(0, 9);
                    if (VerificarPackingListAnotacionDet() == false)
                    {
                        AddPackingList();
                    }
                    else
                    {
                        await DisplayAlert("MENSAJE", "PACKING LIST YA REGISTRADO", "Ok");
                    }

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Algo salio mal, intentelo nuevamente", "Ok");
                Console.WriteLine("################## = " + ex.ToString());
            }
        }
        private async void btnVer_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtspacking.Text))
                {
                    if (VerificarPackingListAnotacionDet() == false)
                    {
                        AddPackingList();
                    }
                    else
                    {
                        await DisplayAlert("MENSAJE", "PACKING LIST YA REGISTRADO", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("MENSAJE", "CAMPO VACIO", "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("MENSAJE", "PROBLEMAS EN LA APP  " + ex.ToString(), "Ok");
                Console.WriteLine("################## = " + ex.ToString());
            }
        }
        private async void AddPackingList()
        {
            try
            {
                ProductoRecep oDetalle = new ProductoRecep();
                var DATA = oDetalle.ConsultarPackingList(txtspacking.Text.Trim());
                if (DATA.Rows.Count > 0)
                {
                    var itemPdv = oDetalle.VerificarItemPlanta(DATA.Rows[0]["iItem_id"].ToString());
                    if (itemPdv.Rows.Count > 0)
                    {
                        txtdpeso.Text = DATA.Rows[0]["dPesoNeto"].ToString();
                        txtItem.Text = itemPdv.Rows[0]["ItemId"].ToString();
                        txtDesc.Text = itemPdv.Rows[0]["Descripcion"].ToString();
                        txtpieza.Text = "1";
                    }
                    else
                    {
                        await DisplayAlert("MENSAJE", "ITEM: " + DATA.Rows[0]["iItem_id"].ToString() + " NO EXISTE EN PRY", "Ok");
                        txtdpeso.Text = "";
                        txtItem.Text = "";
                        txtDesc.Text = "";
                    }
                }
                else
                {
                    await DisplayAlert("MENSAJE", "PACKING LIST: " + txtspacking.Text.Trim() + " NO EXISTE", "Ok");
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("Error", "Algo salio mal, intentelo nuevamente", "Ok");
                Console.WriteLine("################## = " + err.ToString());
            }
        }
        private async void btnRec_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtItem.Text) && !string.IsNullOrEmpty(txtdpeso.Text) && !string.IsNullOrEmpty(txtDesc.Text) && pickerFabricante.SelectedIndex != 0 && pickerCelda.SelectedIndex != 0)
                {
                    ListaItems.ItemsSource = null;
                    ProductoRecep oDetalle = new ProductoRecep();
                    oDetalle.CodigoBarra = "";
                    oDetalle.AnotacionId = AnotacionId;
                    oDetalle.Fabricante = _Fabricante;
                    oDetalle.ItemId = txtItem.Text;
                    oDetalle.Piezas = Convert.ToDecimal(txtpieza.Text);
                    oDetalle.Peso = Convert.ToDecimal(txtdpeso.Text);
                    oDetalle.Colada = txtspacking.Text;
                    oDetalle.SucursalId = App._idSucursal;
                    oDetalle.AlmacenId = App._idAlmacen;
                    oDetalle.CeldaId = _Celda;
                    oDetalle.Fecha = DateTime.Now;
                    oDetalle.Correlativo = Convert.ToInt32(lblcontador.Text) + 1;
                    oDetalle.Login = App._susuario;
                    oDetalle.Status = "OPEN";
                    oDetalle.CodPackList = txtspacking.Text;
                    oDetalle.PesoNetoProveedor = Convert.ToDecimal(txtdpeso.Text);
                    oDetalle.PesoBrutoProveedor = Convert.ToDecimal(txtdpeso.Text);
                    oDetalle.EsDeCliente = false;
                    oDetalle.Sincronizado = 0;
                    if (oDetalle.InsertandoAnotacionDet() > 0)
                    {
                        await DisplayAlert("MENSAJE", "SE REGISTRO CORRECTAMENTE---" + lblcontador.Text, "Ok");
                    }
                    else
                        await DisplayAlert("ERROR", "PROBLEMAS EN LA TRANSACCION", "Ok");
                }
                else
                {
                    await DisplayAlert("ERROR", "CAMPOS VACOS FAVOR LLENAR ITEM-DESCRIPCION-PESO-PIEZA", "Ok");
                }
                CargarAnotacionesDetalle();
            }
            catch (Exception ex)
            {
                await DisplayAlert("MENSAJE", "PROBLEMAS EN LA APP  " + ex.ToString(), "Ok");
                Console.WriteLine("################## = " + ex.ToString());
            }

        }

        public void CargarAnotacionesDetalle()
        {
            try
            {
                ListaItems.ItemsSource = null;
                List<ProductoRecep> ListaAnoacion = new List<ProductoRecep>();
                ProductoRecep oDetalle = new ProductoRecep();
                var data = oDetalle.TaerDetalleAnotacion(AnotacionId);
                int cont = 0;
                for (int a = 0; a < data.Rows.Count; a++)
                {
                    cont++;
                    ProductoRecep oDetalle1 = new ProductoRecep();
                    oDetalle1.CodigoBarra = data.Rows[a]["CodigoBarra"].ToString();
                    oDetalle1.ItemId = data.Rows[a]["ItemId"].ToString();
                    oDetalle1.Descripcion = data.Rows[a]["Descripcion"].ToString();
                    oDetalle1.Peso = Convert.ToDecimal(data.Rows[a]["Peso"].ToString());
                    ListaAnoacion.Add(oDetalle1);
                }
                ListaItems.ItemsSource = ListaAnoacion;

                lblcontador.Text = cont.ToString();
                txtspacking.Text = "";
                txtdpeso.Text = "";
                txtpieza.Text = "";
                txtItem.Text = "";
                txtDesc.Text = "";
            }
            catch (Exception err)
            {
                DisplayAlert("Error", "Algo salio mal, intentelo nuevamente", "Ok");
                Console.WriteLine("################## = " + err.ToString());
            }
        }
        private async void pickerFabricante_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var picker = (Picker)sender;
                int selectedIndex = picker.SelectedIndex;
                if (selectedIndex != -1)
                {
                    _Fabricante = picker.Items[selectedIndex];
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("Error", "Algo salio mal, intentelo nuevamente", "Ok");
                Console.WriteLine("################## = " + err.ToString());
            }
        }
        private async void pickerCelda_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                var picker = (Picker)sender;
                int selectedIndex = picker.SelectedIndex;
                if (selectedIndex != -1)
                {
                    _Celda = picker.Items[selectedIndex];
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("Error", "Algo salio mal, intentelo nuevamente", "Ok");
                Console.WriteLine("################## = " + err.ToString());
            }
        }

        public bool VerificarPackingListAnotacionDet()
        {
            ProductoRecep oDetalle = new ProductoRecep();
            if (oDetalle.VerificarPacking(txtspacking.Text) == 0)
                return false;
            else
                return true;
        }
        private async void txtBarcode_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtBarcode.Text.Length > 0)
                {
                    ProductoRecep oDetalle = new ProductoRecep();
                    var DATA = oDetalle.ConsultarPackingList(txtspacking.Text.Trim());
                    if (DATA.Rows.Count > 0)
                    {
                        var itemPdv = oDetalle.VerificarItemPlanta(DATA.Rows[0]["iItem_id"].ToString());
                        if (itemPdv.Rows.Count > 0)
                        {
                            txtdpeso.Text = DATA.Rows[0]["dPeso"].ToString();
                            txtItem.Text = itemPdv.Rows[0]["ItemId"].ToString();
                            txtDesc.Text = itemPdv.Rows[0]["Descripcion"].ToString();
                            txtpieza.Text = "1";
                        }
                        else
                        {
                            await DisplayAlert("MENSAJE", "ITEM: " + DATA.Rows[0]["iItem_id"].ToString() + " NO EXISTE EN PRY", "Ok");
                            txtdpeso.Text = "";
                            txtItem.Text = "";
                            txtDesc.Text = "";
                        }
                    }
                    if (VerificarPackingListAnotacionDet() == false)
                    {
                        AddPackingList();
                    }
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("Error", "Algo salio mal, intentelo nuevamente", "Ok");
                Console.WriteLine("################## = " + err.ToString());
            }
        }
    }
}