using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAlquileres.Managers.Entidades.Auditoria
{
    public class Audit
    {
        //public int IdUsuarioAlta { get; set; }
        public DateTime CheckIn { get; set; }
        //public int? IdUsuarioModificacion { get; set; }

        //public DateTime? FechaModificacion { get; set; }

        //public int? IdUsuarioBaja { get; set; }

        public DateTime? CheckOut { get; set; }

    }
}
