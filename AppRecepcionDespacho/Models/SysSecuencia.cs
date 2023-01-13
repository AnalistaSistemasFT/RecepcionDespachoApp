using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AppRecepcionDespacho.Models
{
    public  class SysSecuencia : AppRecepcionDespacho.Conexion.Conex
    {
        public string TraerSecuencia(int Suscursal, string sTipo)
        {
            string sSelet = "select * from Sys_Secuencia where Operacion = '{0}' and sucursal={1}";
            sSelet = string.Format(sSelet, sTipo, Suscursal);
            DataTable dtsResult = this.consultarD(sSelet);
            string sSecuencia = "";
            if (dtsResult.Rows.Count > 0)
            {
                string sContador = dtsResult.Rows[0]["Contador"].ToString();
                string sFijo = dtsResult.Rows[0]["Fijo"].ToString();
                int iSiguiente = 0;
                string year = "0";
                switch (dtsResult.Rows[0]["Reset"].ToString())
                {

                    case "y":
                        year = (sContador.Substring(2, 2));
                        if (year != (DateTime.Now.Year.ToString().Substring(2, 2)))
                            year = DateTime.Now.Year.ToString().Substring(2, 2);
                        iSiguiente = Convert.ToInt32(sContador.Substring(4, 6)) + 1;
                        sSecuencia = sFijo + year + (iSiguiente.ToString("D6"));
                        break;
                    case "m":
                        year = (sContador.Substring(0, 2));
                        if (year != (DateTime.Now.Year.ToString().Substring(2, 2)))
                            year = DateTime.Now.Year.ToString().Substring(2, 2);
                        string Mes = (sContador.Substring(2, 2));
                        if (Mes != DateTime.Now.Month.ToString())
                            Mes = DateTime.Now.Month.ToString("D2");
                        iSiguiente = Convert.ToInt32(sContador.Substring(6, 5)) + 1;
                        sSecuencia = year + Mes + sFijo + (iSiguiente.ToString("D5"));
                        break;
                }
            }
            return sSecuencia;
        }
        public string TraerSecuencia(int Suscursal, string sTipo, SqlTransaction trnProxy)
        {
            string sSelet = "select * from Sys_Secuencia where Operacion = '{0}' and sucursal={1}";
            sSelet = string.Format(sSelet, sTipo, Suscursal);
            DataTable dtsResult = this.consultarD(sSelet, trnProxy);
            string sSecuencia = "";
            if (dtsResult.Rows.Count > 0)
            {
                string sContador = dtsResult.Rows[0]["Contador"].ToString();
                string sFijo = dtsResult.Rows[0]["Fijo"].ToString();
                int iSiguiente = 0;
                string year = "0";
                switch (dtsResult.Rows[0]["Reset"].ToString())
                {

                    case "y":
                        year = (sContador.Substring(2, 2));
                        if (year != (DateTime.Now.Year.ToString().Substring(2, 2)))
                            year = DateTime.Now.Year.ToString().Substring(2, 2);
                        iSiguiente = Convert.ToInt32(sContador.Substring(4, 6)) + 1;
                        sSecuencia = sFijo + year + (iSiguiente.ToString("D6"));
                        break;
                    case "m":
                        year = (sContador.Substring(0, 2));
                        if (year != (DateTime.Now.Year.ToString().Substring(2, 2)))
                            year = DateTime.Now.Year.ToString().Substring(2, 2);
                        string Mes = (sContador.Substring(2, 2));
                        if (Mes != DateTime.Now.Month.ToString())
                            Mes = DateTime.Now.Month.ToString("D2");
                        iSiguiente = Convert.ToInt32(sContador.Substring(6, 5)) + 1;
                        sSecuencia = year + Mes + sFijo + (iSiguiente.ToString("D5"));
                        break;
                }
            }
            return sSecuencia;
        }
        public int ActualizarSecuencia(int iSucursal, string Operacion, string Contador, SqlTransaction trnProxy)
        {
            string sError = string.Empty;
            string sUpdate = " update Sys_Secuencia set Contador = '" + Contador + "' where  sucursal=" + iSucursal + " and Operacion = '" + Operacion + "'";
            sUpdate = string.Format(sUpdate);
            return this.ejecutar(ref sError, sUpdate, trnProxy);
        }

    }
}
