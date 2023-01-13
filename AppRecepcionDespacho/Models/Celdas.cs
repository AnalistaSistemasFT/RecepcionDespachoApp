using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AppRecepcionDespacho.Models
{
    public class Celdas : AppRecepcionDespacho.Conexion.Conex
    {
        public string CeldaId { get; set; } 
        public Celdas ()
        {

        }

        public DataTable TraerCeldas(int Almacen)
        {
            string sentencia = String.Format("select CeldaId from tblCeldas where AlmacenId = "+ Almacen);
            DataTable dts = this.ejecutarConsulta(sentencia);
            return dts;
        }
    }
}
