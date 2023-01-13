using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AppRecepcionDespacho.Models
{
    public  class clsRecepcion : AppRecepcionDespacho.Conexion.Conex
    {
        public string AnotacionId { get; set; }
        public DateTime Fecha { get; set; }
        public string Id_Camion { get; set; }
        public string Chofer { get; set; }
        public string Placa { get; set; }
        public string CI { get; set; }
        public int Procedencia { get; set; }
        public string Propietario { get; set; }
        public string Login { get; set; }
        public string Status { get; set; }
        public int SucursalId { get; set; }
        public string Tipo { get; set; }
        public bool EsDeCliente { get; set; }
        public string Manifiesto { get; set; }  
        public string Obs { get; set; }

        public clsRecepcion()
        {

        }

        public int VerificarPacking(string sPacking)
        {
            string sentencia = String.Format("select * from tblproductosrecep where spackinglist = '" + sPacking + "'");
            DataTable dts = this.ejecutarConsulta(sentencia);
            if (dts.Rows.Count > 0)
                return 1;
            else
                return 0;
            //var data = CON.ejecutarConsulta(sentencia);
        }

        public string TraerSecuencia(int Suscursal, string sTipo)
        {
            string sSelet = "select * from Sys_Secuencia where Operacion = '{0}' and sucursal={1}";
            sSelet = string.Format(sSelet, sTipo, Suscursal);
            DataTable dtsResult = this.ejecutarConsulta1(sSelet);
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

        public int InsertarAnotacion(SqlTransaction trnProxy)
        {
            string sError = string.Empty;
            string sInsert = @"INSERT INTO [dbo].[tblAnotacion]
           ([AnotacionId] ,[Fecha],[Chofer],[Id_Camion] ,[Placa],[CI],[Procedencia] ,[Propietario],[Login],[Obs]  ,[Status],[SucursalId],[Tipo],[EsDeCliente],[Manifiesto])
                             VALUES ('{0}','{1}','{2}',{3},'{4}','{5}',{6},'{7}','{8}','{9}','{10}',{11},'{12}','{13}','{14}')";
            sInsert = string.Format(sInsert,AnotacionId,Fecha.ToString("dd/MM/yyyy"),Chofer,Id_Camion,Placa,CI,Procedencia,Propietario,Login,Obs,Status,SucursalId,Tipo,EsDeCliente,Manifiesto);
            return this.ejecutar(ref sError, sInsert, trnProxy);
        }

        public int InsertandoAnotacion()
        {
           using( SqlTransaction trnSql = this.inicio_tr("LYBK"))
           {
                SysSecuencia oSecuencia = new SysSecuencia();
            int i = this.InsertarAnotacion(trnSql);
            if (i > 0)
                i = oSecuencia.ActualizarSecuencia(SucursalId,"ANOTACION",AnotacionId,trnSql);
            if (i > 0)
                trnSql.Commit();
            else
                trnSql.Rollback();
            trnSql.Dispose();
            return i;
            }
        }

        public DataTable TraerAnotacionesAbiertas(int Sucursal)
        {
            string sentencia = String.Format(@"select AnotacionId,p.Nombre from tblAnotacion a inner join 
                                                    tblSucursal p on a.Procedencia = p.SucursalID
                                                     where Status = 'OPEN' and a.SucursalId = "+ Sucursal);
            DataTable dts = this.ejecutarConsulta(sentencia);
            return dts;
        }
    }
}
