using LogicaAlquileres.Managers.Entidades;
using LogicaAlquileres.Managers.ModelFactories;
using LogicaAlquileres.Repos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAlquileres.Managers
{
    public interface IPropiedadManager
    {
        IEnumerable<PropiedadCompleto> GetPropiedades();
        Propiedad GetPropiedad(int IdPropiedad);
        int CrearPropiedad(Propiedad propiedad/*, int IdUsuarioAlta*/);
        bool ModificarPropiedad(int IdPropiedad, Propiedad propiedad, int IdUsuarioModificacion);
        bool EliminarPropiedad(int IdPropiedad, int IdUsuarioBaja);
    }

    
    public class PropiedadManager : IPropiedadManager
    {
        private IPropiedadRepository _repo;
        public PropiedadManager(IPropiedadRepository repo)
        {
            _repo = repo;
        }

        // Obtiene un PropiedadVM por Id 
        public Propiedad GetPropiedad(int IdPropiedad)
        {
            var propiedad = _repo.GetPropiedad(IdPropiedad);
            return propiedad;


        }
        // Obtiene una lista de Containers
        public IEnumerable<PropiedadCompleto> GetPropiedades()
        {
            return _repo.GetPropiedadesCompleto();
        }

        // Crea un Container en la Base de Datos
        public int CrearPropiedad(Propiedad propiedad/*, int IdUsuarioAlta*/)
        {
            //propiedad.id_Usuario_Propiedad = IdUsuarioAlta;
            //propiedad.id_Alquiler = propiedad.id_Alquiler;
            //propiedad.descripcion_Propiedad = propiedad.descripcion_Propiedad;
           // propiedad.precio_Propiedad = propiedad.precio_Propiedad;
           // propiedad.estado_Propiedad = propiedad.estado_Propiedad;
           // propiedad.nombre_Propiedad = propiedad.nombre_Propiedad;
           // propiedad.direccion_Propiedad = propiedad.direccion_Propiedad;
            propiedad.fechaAlta_Propiedad = DateTime.Now;
            var cont = _repo.CrearPropiedad(propiedad);

            return cont;

        }

   
        public bool EliminarPropiedad(int IdPropiedad, int IdUsuarioBaja)
        {
            return _repo.EliminarPropiedad(IdPropiedad, IdUsuarioBaja);

        }

       
        public bool ModificarPropiedad(int IdPropiedad, Propiedad propiedad, int IdUsuarioModificacion)
        {

            //Obtengo lo que viene de la base de datos
            var propiedadEnDb = _repo.GetPropiedad(IdPropiedad);

            //En el objeto que viene de la base de datos, le "pego" los valores que me vienen del formulario
            propiedadEnDb.descripcion_Propiedad = propiedad.descripcion_Propiedad;
            propiedadEnDb.precio_Propiedad = propiedad.precio_Propiedad;
            propiedadEnDb.estado_Propiedad = propiedad.estado_Propiedad;
            propiedadEnDb.nombre_Propiedad = propiedad.nombre_Propiedad;
            propiedadEnDb.direccion_Propiedad = propiedad.direccion_Propiedad;
          //  propiedadEnDb.FechaModificacion = DateTime.Now; // nos falta esto
            var cont = _repo.ModificarPropiedad(IdPropiedad, propiedadEnDb);

            return cont;
        }
    }
}

