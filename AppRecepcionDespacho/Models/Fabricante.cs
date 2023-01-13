using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AppRecepcionDespacho.Models
{
    public  class Fabricante : AppRecepcionDespacho.Conexion.Conex
    {
        public int ProveedorId { get; set; }
        public string Nombre { get; set; }
        public Fabricante()
        {

        }
        public DataTable TraerFabricantes()
        {
            string sentencia = String.Format("select ProveedorId,Nombre from tblCatProveedor");
            DataTable dts = this.ejecutarConsulta(sentencia);
            return dts;
        }

    }
}
