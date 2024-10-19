using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAlquileres.Managers.Entidades.Auditoria;

namespace LogicaAlquileres.Managers.Entidades
{
    public class Propiedad : Audit
    {
        public int IdPropiedad {  get; set; }
        public int IdUsuario { get; set; }
        public int? IdAquiler{ get; set; }
        public string Direccion { get; set; }
        public string Estado { get; set; }
        public double Precio { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

    }
}
