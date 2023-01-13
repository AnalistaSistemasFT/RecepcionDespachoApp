using AppRecepcionDespacho.Models;
using AppRecepcionDespacho.Vistas;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppRecepcionDespacho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        Conexion.Conex CON = new Conexion.Conex();
        public Login()
        {
            InitializeComponent();
        }
        private async void btnIngresar_Clicked(object sender, EventArgs e)
        {
            indicator_login.IsRunning = true;
            await Task.Delay(800);
            if (CrossConnectivity.Current.IsConnected)
            {
                if (!string.IsNullOrWhiteSpace(entryUsuario.Text) || (!string.IsNullOrEmpty(entryUsuario.Text)))
                {
                    if (!string.IsNullOrWhiteSpace(entryPassword.Text) || (!string.IsNullOrEmpty(entryPassword.Text)))
                    {
                        try
                        {
                            string user = entryUsuario.Text;
                            string pass = entryPassword.Text;
                            string sentencia = String.Format("select EmpleadoId from empleados.dbo.tbluser where Login='{0}' and Clave='{1}'", user, pass);
                            int[] IdEmpleado = null;
                            var data = CON.ejecutarConsultaLogin(sentencia);
                            foreach (DataRow item in data.Rows)
                            {
                                IdEmpleado = new int[] { (int)item[0] };
                            }

                            if (IdEmpleado != null)
                            {
                                Usuario oUsuario = new Usuario();
                                var dtAlmacen = oUsuario.TraerAlmacenPorUsuario(user);
                                if (dtAlmacen != null)
                                {
                                    if (dtAlmacen.Rows.Count > 1)
                                    {
                                        App._idPersonal = IdEmpleado[0];
                                        App._nombrePersonal = entryUsuario.Text;
                                        App._susuario = user;
                                        await Navigation.PushAsync(new Almacen());
                                    }
                                    else
                                    {
                                        App._idPersonal = IdEmpleado[0];
                                        App._nombrePersonal = entryUsuario.Text;
                                        App._susuario = user;
                                        App._idSucursal = Convert.ToInt32(dtAlmacen.Rows[0]["SucursalId"].ToString());
                                        App._idAlmacen = Convert.ToInt32(dtAlmacen.Rows[0]["AlmacenId"].ToString());
                                        //await Navigation.PushAsync(new AdmRecepcion());
                                        //Application.Current.MainPage = new MainPage();
                                        await Navigation.PushAsync(new AdmRecepcion());
                                        Application.Current.MainPage = new AdmRecepcion();

                                    }
                                    indicator_login.IsRunning = false;
                                    Navigation.RemovePage(this);
                                }
                                else
                                {
                                    await DisplayAlert("Error", "Usuario no tiene asignado un almacen", "Ok");
                                    indicator_login.IsRunning = false;
                                }

                            }
                            else
                            {
                                await DisplayAlert("Error", "Usuario no encontrado", "Ok");
                                indicator_login.IsRunning = false;
                            }
                        }
                        catch (Exception err)
                        {
                            Console.WriteLine("################=== " + err.ToString());
                            await DisplayAlert("Error", err.ToString(), "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "El campo de 'Password' esta vacio", "Ok");
                        indicator_login.IsRunning = false;
                    }
                }
                else
                {
                    await DisplayAlert("Error", "El campo de 'Contraseña' esta vacio", "Ok");
                    indicator_login.IsRunning = false;
                }
            }
            else
            {
                await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
                indicator_login.IsRunning = false;
            }
        }
    }
}