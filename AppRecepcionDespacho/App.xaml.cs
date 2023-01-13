using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppRecepcionDespacho
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            MainPage = new NavigationPage(new Login());
        }
        public static int _idPersonal = 0;
        public static int _idAlmacen = 0;
        public static int _idSucursal = 0;
        public static string _susuario = "";
        public static string _nombrePersonal = "";
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
