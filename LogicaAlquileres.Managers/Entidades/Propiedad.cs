using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAlquileres.Managers.Entidades.Auditoria;

namespace LogicaAlquileres.Managers.Entidades
{
    public class Propiedad : Audit
    {
        [Display(Name = "ID Propiedad")]
        public int id_Propiedad { get; set; }

        [Display(Name = "Nombre Propiedad")]
        public string nombre_Propiedad { get; set; }

        [Display(Name = "Dirección Propiedad")]
        public string direccion_Propiedad { get; set; }

        [Display(Name = "Estado Propiedad")]
        public string estado_Propiedad { get; set; }

        [Display(Name = "Precio Propiedad")]
        public decimal precio_Propiedad { get; set; }

        [Display(Name = "Descripción Propiedad")]
        public string descripcion_Propiedad { get; set; }



    }
}
