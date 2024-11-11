using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
=======
>>>>>>> 819df21461be7bd76b98b2a5db9783ab93eaceb0
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAlquileres.Managers.Entidades.Auditoria;

namespace LogicaAlquileres.Managers.Entidades
{
    public class Propiedad : Audit
    {
<<<<<<< HEAD
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

=======
        public int id_Propiedad {  get; set; }
        //public int id_Usuario_Propiedad { get; set; }
        //public int? id_Alquiler{ get; set; }
        public string direccion_Propiedad { get; set; }
        public string estado_Propiedad { get; set; }
        public decimal precio_Propiedad { get; set; }
        public string nombre_Propiedad { get; set; }
        public string descripcion_Propiedad { get; set; }
        
>>>>>>> 819df21461be7bd76b98b2a5db9783ab93eaceb0


    }
}
