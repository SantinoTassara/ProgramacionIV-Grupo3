using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAlquileres.Managers.Entidades
{
    public class Alquiler
    {
        public int IdAlquiler { get; set; }
        public int IdInquilino { get; set; }
        public DateTime FechaCheckIn { get; set; }
        public DateTime FechaCheckOut { get; set; }
        public double PrecioTotal { get; set; }
    }
}
