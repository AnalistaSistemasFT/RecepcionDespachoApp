using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AppRecepcionDespacho.Models
{
    public class Usuario : AppRecepcionDespacho.Conexion.Conex
    {
        public string Login { get; set; }
        public string Clave { get; set; }
        public int Activo { get; set; }
        public int EmpleadoId { get; set; }
        public int SucursalId { get; set; }
        public int AlmacenId { get; set; }
        public string Nombre { get; set; }

        public Usuario() { }

        public DataTable TraerAlmacenPorUsuario(string Usuario)
        {
            string sentencia = String.Format(@"select u.SucursalId, AlmacenId, Nombre from empleados.dbo.tblUserSuc u inner join
                                        tblAlmacen a on u.SucursalId = a.SucursalId
                                         where Login = '" + Usuario + "'");
            DataTable dts = this.ejecutarConsulta(sentencia);
            return dts;
        }
    }

   
    
}