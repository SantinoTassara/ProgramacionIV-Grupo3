﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAlquileres.Managers.Entidades.Auditoria;

namespace LogicaAlquileres.Managers.Entidades
{
    public class Propiedad : Audit
    {
        public int id_Propiedad {  get; set; }
        public int id_Usuario_Propiedad { get; set; }
        public int? id_Alquiler{ get; set; }
        public string direccion_Propiedad { get; set; }
        public string estado_Propiedad { get; set; }
        public decimal precio_Propiedad { get; set; }
        public string nombre_Propiedad { get; set; }
        public string descripcion_Propiedad { get; set; }
        


    }
}
