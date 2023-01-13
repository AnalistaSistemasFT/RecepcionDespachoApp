using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AppRecepcionDespacho.Models
{
    public class Lecturado : AppRecepcionDespacho.Conexion.Conex
    {
        public int inroid { get; set; }
        public int ilecturado_id { get; set; }
        public string scalidad { get; set; }
        public decimal despesor { get; set; }
        public string spackinglist { get; set; }
        public string sbobina { get; set; }
        public decimal dpeso { get; set; }

        public Lecturado()
        {

        }

        
    }
}
