using System;
using System.Collections.Generic;
using System.Text;

namespace AppRecepcionDespacho.Models
{
    public class DespProductos
    {
        public string DespachoId { get; set; }
        public string ProductoId { get; set; }
        public string ItemId { get; set; }
        public DateTime Fecha { get; set; }
        public string Status { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Peso { get; set; }
        public string CeldaId { get; set; }
        public int SucursalId { get; set; }
        public string ItemFId { get; set; }
        public int Piezas { get; set; }
        public decimal Metros { get; set; }
        public string Colada { get; set; }
    }
}
