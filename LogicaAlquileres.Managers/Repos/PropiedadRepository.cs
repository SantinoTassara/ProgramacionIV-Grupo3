using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LogicaAlquileres.Managers.Entidades;
using LogicaAlquileres.Managers.ModelFactories;

namespace LogicaAlquileres.Managers.Repos
{
    public interface IPropiedadRepository
    {
        Propiedad GetPropiedad(int idPropiedad);

        IEnumerable<Propiedad> GetPropiedades(bool? SoloActivos = true);

        IEnumerable<PropiedadCompleto> GetPropiedadesCompleto();

        int CrearPropiedad(Propiedad propiedad);
        bool ModificarPropiedad(int IdPropiedad, Propiedad propiedad);
        bool EliminarPropiedad(int IdPropiedad, int IdUsuarioBaja);
    }

    internal class PropiedadRepository
    {
        private string _connectionString;//falta base

        public PropiedadRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
   
        public Propiedad GetPropiedad(int IdPropiedad)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                Propiedad result = conn.QuerySingle<Propiedad>("Select * from Propiedad Where IdPropiedad = " + IdPropiedad.ToString());
                return result;
            }
        }

        public IEnumerable<PropiedadCompleto> GetPropiedadesCompleto()
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                IEnumerable<PropiedadCompleto> result =
                    conn.Query<PropiedadCompleto>(@"select Container.*, EstadosContainer.Descripcion Estado 
                                                    from container 
                                                    left join EstadosContainer on Container.IdEstadoContainer = EstadosContainer.IdEstadoContainer
                                                   where container.fechabaja is null");
                return result;
            }

        }
       
        public int CrearPropiedad(Propiedad propiedad)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Container (DescripcionContainer, IdEstadoContainer, IdUsuarioAlta, FechaAlta, IdUsuarioModificacion, FechaModificacion, IdUsuarioBaja, FechaBaja)  
                                VALUES ( @DescripcionContainer, @IdEstadoContainer, @IdUsuarioAlta, @FechaAlta, @IdUsuarioModificacion, @FechaModificacion, @IdUsuarioBaja, @FechaBaja);                    
                                SELECT CAST(SCOPE_IDENTITY() AS INT) ";//editar con nuestra tabla

                propiedad.IdPropiedad = db.QuerySingle<int>(query, propiedad);

                return propiedad.IdPropiedad;
            }
        }

        public bool ModificarPropiedad(int IdPropiedad, Propiedad propiedad)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE 
                                    Container 
                                SET 
                                    DescripcionContainer = @DescripcionContainer, 
                                    IdEstadoContainer = @IdEstadoContainer, 
                                    FechaModificacion = @FechaModificacion, 
                                    IdUsuarioModificacion  = @IdUsuarioModificacion    
                                    WHERE IdContainer = " + IdPropiedad.ToString();
                //db.execute devuelve un entero que representa la cantidad de filas afectadas. 
                //Se espera que se haya modificado solo un registro, por eso se lo compara con un 1.
                return db.Execute(query, propiedad) == 1;
            }
        }
  
        public bool EliminarPropiedad(int IdPropiedad, int IdUsuarioBaja)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE 
                                    Container 
                                SET 
                                    
                                    FechaBaja = '" + DateTime.Now.ToString("yyyyMMdd") + "'," +
                                    " IdUsuarioBaja  = " + IdUsuarioBaja +
                                "WHERE IdContainer = " + IdPropiedad.ToString();
                //db.execute devuelve un entero que representa la cantidad de filas afectadas. 
                //Se espera que se haya modificado solo un registro, por eso se lo compara con un 1.
                return db.Execute(query) == 1;
            }
        }
    }


}
