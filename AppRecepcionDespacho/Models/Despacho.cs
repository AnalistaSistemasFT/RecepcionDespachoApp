using System;
using System.Collections.Generic;
using System.Text;

namespace AppRecepcionDespacho.Models
{
    public class Despacho
    {
        public string DespachoId { get; set; }
        public DateTime Fecha { get; set; }
        public string Status { get; set; }
        public string Naturaleza { get; set; }
    }
}
