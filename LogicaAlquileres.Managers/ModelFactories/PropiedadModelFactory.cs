using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAlquileres.Managers.ModelFactories
{
    public class PropiedadCompleto
    {
        //agregue esto
        public int IdPropiedad { get; set; }
        public int IdUsuario { get; set; }
        public int? IdAquiler { get; set; }
        public string Direccion { get; set; }
        public string Estado { get; set; }
        public double Precio { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } 
    }
}
