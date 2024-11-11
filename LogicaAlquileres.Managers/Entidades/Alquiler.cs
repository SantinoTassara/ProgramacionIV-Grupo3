using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAlquileres.Managers.Entidades
{
    public class Alquiler
    {
        public int id_Alquiler { get; set; }
        public int id_Inquilino_Alquiler { get; set; }
        public int id_Propiedad { get; set; }  // Nueva propiedad
        public string nombre_Propiedad { get; set; }  // Nueva propiedad
        public DateTime? checkIn_Alquiler { get; set; }
        public DateTime? checkOut_Alquiler { get; set; }
        public decimal precioTotal_Alquiler { get; set; }
        public string direccion_Alquiler { get; set; }
    }

}
