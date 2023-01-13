using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace AppRecepcionDespacho.Models
{
    public  class ProductoRecep : AppRecepcionDespacho.Conexion.Conex
    {
        //Conexion.Conex CON = new Conexion.Conex();

        public string CodigoBarra { get; set; }
        public string AnotacionId { get; set; }
        public string Fabricante { get; set; }
        public string ItemId { get; set; }
        public string Descripcion { get; set; }
        public decimal Piezas { get; set; }
        public decimal Peso { get; set; }
        public string Colada { get; set; }
        public int SucursalId { get; set; }
        public int AlmacenId { get; set; }
        public string CeldaId { get; set; }
        public DateTime Fecha { get; set; }
        public int Correlativo { get; set; }
        public string Login { get; set; }
        public string Status { get; set; }
        public string CodPackList { get; set; }
        public decimal PesoNetoProveedor { get; set; }
        public decimal PesoBrutoProveedor { get; set; }
        public bool EsDeCliente { get; set; }
        public int Sincronizado { get; set; }


        public ProductoRecep()
        { 

        }

        public int VerificarPacking(string sPacking)
        {
            string sentencia = String.Format(" select * from tblAnotacionDet where Colada = '" + sPacking + "'");
            DataTable dts = this.ejecutarConsulta(sentencia);
            
            if (dts.Rows.Count > 0)
                return 1;
            else
                return 0;
            //var data = CON.ejecutarConsulta(sentencia);
        }
        public DataTable TaerDetalleAnotacion(string sAnotacion)
        {
            string sentencia = String.Format(" select d.*,i.Descripcion from tblAnotacionDet d inner join tblItem i on d.ItemId = i.ItemId where AnotacionId  = '" + sAnotacion + "'");
            DataTable dts = this.ejecutarConsulta(sentencia);
            return dts;
        }
        public DataTable VerificarItemPlanta(string item)
        {
            string sentencia = String.Format(" select * from tblItem where ItemFId = '"+item+"'");
            DataTable dts = this.ejecutarConsulta(sentencia);
            return dts;
        }

       
        public DataTable ConsultarPackingList(string sPacking)
        {
            
            string sentencia = String.Format(@"select tblPackingList.*,scmadesc from tblPackingList 
                                         inner join
                                         [INFORMIX].[siscome].[sistemas].scmaster on iItem_id = scmanart
                                        where sSerie = '" + sPacking + "'");
           return this.ejecutarConsultaftodo(sentencia);

           
        }

        public int InsertarAnotacionDet(SqlTransaction trnProxy)
        {
            CultureInfo ci = new CultureInfo("es-ES");
            string sError = string.Empty;
            string sInsert = @"INSERT INTO tblAnotacionDet ([CodigoBarra],[AnotacionId],[Fabricante],[ItemId],[Piezas],[Peso],[Colada],[SucursalId],[AlmacenId],[CeldaId],[Fecha]
           ,[Correlativo],[Login],[Status],[CodPackList],[PesoNetoProveedor],[PesoBrutoProveedor],[EsDeCliente] ,[Sincronizado])
                    VALUES('{0}','{1}','{2}','{3}',{4} ,{5} ,'{6}',{7} ,{8} ,'{9}','{10}',{11},'{12}','{13}','{14}',{15},{16},'{17}',{18})";
            sInsert = string.Format(sInsert, CodigoBarra, AnotacionId, Fabricante, ItemId, Piezas, Peso, Colada, SucursalId, AlmacenId, CeldaId, Fecha.ToString("dd/MM/yyyy")
           , Correlativo, Login, Status, CodPackList, PesoNetoProveedor, PesoBrutoProveedor, EsDeCliente, Sincronizado);
            return this.ejecutar(ref sError, sInsert, trnProxy);
        }
        public int UpdatePackingList(SqlTransaction trnProxy)
        {
           
            string sError = string.Empty;
            string sInsert = @" UPDATE tblPackingList SET dPesocc = "+ Peso + " where sSerie = '"+ CodPackList + "'";
            sInsert = string.Format(sInsert,CodPackList);
            return this.ejecutar(ref sError, sInsert, trnProxy);
        }

       
        public int InsertandoAnotacionDet()
        {
            using (SqlTransaction trnSql = this.inicio_tr("LYBK"))
            {
                SqlTransaction trnSql1 = this.inicio_tr("FTODO");

                SysSecuencia oSecuencia = new SysSecuencia();
                this.CodigoBarra = oSecuencia.TraerSecuencia(this.SucursalId, "PAQUETE", trnSql);
                int i = this.InsertarAnotacionDet(trnSql);
                if (i > 0)
                {
                    i = oSecuencia.ActualizarSecuencia(SucursalId, "PAQUETE", CodigoBarra, trnSql);
                    if (i > 0)
                       i = this.UpdatePackingList(trnSql1);
                }
                if (i > 0) { 
                    trnSql.Commit();
                    trnSql1.Commit();
                }
                else { 
                    trnSql.Rollback();
                    trnSql1.Rollback();
                }
                trnSql.Dispose();
                trnSql1.Dispose();
                return i;
            }
        }
    }
}
