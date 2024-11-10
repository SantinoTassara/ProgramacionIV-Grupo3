using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAlquileres.Managers.Entidades.Auditoria;

namespace LogicaAlquileres.Managers.Entidades
{
    public class Usuario:Audit
    {
        public int id_usuario { get; set; }
        public string nombre_Usuario { get; set; }
        public string apellido_Usuario { get; set; }
        public int? telefono_Usuario { get; set; }
        public string email_Usuario { get; set; }
        public string? password_Usuario {  get; set; }
        public string? GoogleIdentificador { get; set; }

        //public string NombreCompleto { get; set; }
       
        //public bool Borrado { get; set; }
    }
}
