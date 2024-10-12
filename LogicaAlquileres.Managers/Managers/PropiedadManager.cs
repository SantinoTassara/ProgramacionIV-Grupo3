using LogicaAlquileres.Managers.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAlquileres.Managers.Managers
{
    public interface IPropiedadManager
    {
        Propiedad CrearPropiedad();
    }
    public class PropiedadManager : IPropiedadManager
    {
        public PropiedadManager() { }

        public Propiedad CrearPropiedad()
        {
            Propiedad propiedad = new Propiedad
            {
                IdPropiedad = 1,
                IdUsuario = 1,
                Direccion = "Avenida Rivadavia 1111",
                Estado = "Disponible",
                Precio = 1000,
                Nombre = "Alquiler Dpto Av Rivadavia 1111 ",
                Descripcion = "Dpto a 4 cuadras de obelisco y a 1 del subte A"

            };
            return propiedad;
        }
    }

}

