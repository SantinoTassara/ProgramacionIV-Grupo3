using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAlquileres.Managers.Entidades.Auditoria
{
    public class Audit
    {
        public int IdUsuarioAlta { get; set; }
        //public DateTime CheckIn { get; set; }

        public DateTime fechaAlta_Propiedad { get; set; }
        public DateTime fechaBaja_Propiedad { get; set; }
        public DateTime fechaModificacion_Propiedad { get; set; }
       // public DateTime? CheckOut { get; set; }

    }
}
