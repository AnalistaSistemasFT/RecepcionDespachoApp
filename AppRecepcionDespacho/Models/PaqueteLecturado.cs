using System;
using System.Collections.Generic;
using System.Text;

namespace AppRecepcionDespacho.Models
{
    public class PaqueteLecturado
    {
        public string ItemId { get; set; }
        public string Descripcion { get; set; }
        public string PaqueteId { get; set; }
        public int Piezas { get; set; }
        public decimal Peso { get; set; }
        public string display_piezas_peso { get { return $"Piezas: {Piezas} - Peso: {Peso} Kgs."; } }
    }
}
