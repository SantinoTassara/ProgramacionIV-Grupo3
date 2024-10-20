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
using Dapper;
using LogicaAlquileres.Managers.Entidades;
using LogicaAlquileres.Managers.ModelFactories;

namespace LogicaAlquileres.Repos
{
    public interface IPropiedadRepository
    {
        Propiedad GetPropiedad(int id_Propiedad);

        IEnumerable<Propiedad> GetPropiedades(bool? SoloActivos = true);

        IEnumerable<PropiedadCompleto> GetPropiedadesCompleto();

        int CrearPropiedad(Propiedad propiedad);
        bool ModificarPropiedad(int IdPropiedad, Propiedad propiedad);
        bool EliminarPropiedad(int IdPropiedad/*, int IdUsuarioBaja*/);
    }

    public class PropiedadRepository : IPropiedadRepository
    {
        private string _connectionString;

        public PropiedadRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

       

        public Propiedad GetPropiedad(int id_Propiedad)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                
                var query = "SELECT * FROM Grupo3.propiedad WHERE id_Propiedad = @Id";
                var result = conn.QuerySingleOrDefault<Propiedad>(query, new { Id = id_Propiedad });

                
                if (result == null)
                {
                    throw new KeyNotFoundException($"No se encontró ninguna propiedad con el ID {id_Propiedad}.");
                }

                return result;
            }
        }

        // Consulta a la base de datos por la lista de las propiedades

        public IEnumerable<Propiedad> GetPropiedades(bool? SoloActivos = true) 
        {
            using (IDbConnection conn = new SqlConnection(_connectionString)) 
            {
                string query = "Select * from Grupo3.propiedad";
                /*if (SoloActivos == true)
                    query += " where FechaBaja is null";//crear audit para esto*/
                IEnumerable<Propiedad> results = conn.Query<Propiedad>(query);

                return results;
            }
        }


        // Obtiene una lista completa de las propiedades
        public IEnumerable<PropiedadCompleto> GetPropiedadesCompleto()
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                IEnumerable<PropiedadCompleto> result =
                    conn.Query<PropiedadCompleto>("Select * from Grupo3.propiedad"/*@"select propiedad.*, alquiler.Descripcion Estado 
                                                    from container 
                                                    left join EstadosContainer on Container.IdEstadoContainer = EstadosContainer.IdEstadoContainer
                                                   where container.fechabaja is null"*/);//modificar nuestra tabla o relacionar?
                return result;
            }

        }


        // Crear nueva Propiedad en la base de datos
     
        public int CrearPropiedad(Propiedad propiedad)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Grupo3.propiedad (id_Usuario_Propiedad, id_Alquiler, direccion_Propiedad, estado_Propiedad, precio_Propiedad, nombre_Propiedad, descripcion_Propiedad, fechaAlta_Propiedad)  
                        VALUES (@id_Usuario_Propiedad, @id_Alquiler, @direccion_Propiedad, @estado_Propiedad, @precio_Propiedad, @nombre_Propiedad, @descripcion_Propiedad, @fechaAlta_Propiedad);
                        SELECT CAST(SCOPE_IDENTITY() AS INT)";

                /* var id = db.QuerySingle<int>(query, propiedad);
                 return id;*/
                propiedad.id_Propiedad = db.QuerySingle<int>(query, propiedad);

                return propiedad.id_Propiedad;
            }
        }

        // Modificar Propiedad en la base de Datos
       
        public bool ModificarPropiedad(int IdPropiedad, Propiedad propiedad)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"
            UPDATE Grupo3.propiedad
            SET 
                id_Usuario_Propiedad = @id_Usuario_Propiedad, 
                id_Alquiler = @id_Alquiler, 
                direccion_Propiedad = @direccion_Propiedad, 
                estado_Propiedad = @estado_Propiedad, 
                precio_Propiedad = @precio_Propiedad, 
                nombre_Propiedad = @nombre_Propiedad, 
                descripcion_Propiedad = @descripcion_Propiedad,
                fechaModificacion_Propiedad = @fechaModificacion_Propiedad
            WHERE id_Propiedad = @id_Propiedad";

                // Mapear los parámetros de forma segura.
                var parameters = new
                {
                    id_Usuario_Propiedad = propiedad.id_Usuario_Propiedad,
                    id_Alquiler = propiedad.id_Alquiler,
                    direccion_Propiedad = propiedad.direccion_Propiedad,
                    estado_Propiedad = propiedad.estado_Propiedad,
                    precio_Propiedad = propiedad.precio_Propiedad,
                    nombre_Propiedad = propiedad.nombre_Propiedad,
                    descripcion_Propiedad = propiedad.descripcion_Propiedad,
                    fechaModificacion_Propiedad = propiedad.fechaModificacion_Propiedad,
                    id_Propiedad = IdPropiedad
                };

                // Ejecuta la consulta y verifica si se afectó una fila.
                return db.Execute(query, parameters) == 1;
            }
        }
        // Eliminar de manera lógica una Propiedad de la base de datos
        //public bool EliminarPropiedad(int IdPropiedad/*, int IdUsuarioBaja*/)
        //{
        //    using (IDbConnection db = new SqlConnection(_connectionString))
        //    {
        //        //hay que modificar la tabla para que elimine una propiedad con fecha de salida, checkout, osea mover el checkout a la tabla propiedad
        //        string query = @"UPDATE 
        //                            Grupo3.alquiler  
        //                        SET 

        //                            checkOut_Alquiler = '" + DateTime.Now.ToString("yyyyMMdd") + "'," +
        //                            " id_Inquilino_alquiler  = " + IdUsuarioBaja +
        //                        "WHERE id_propiedad = " + IdPropiedad.ToString();//modificado a nuestra base, probar..
        //        //db.execute devuelve un entero que representa la cantidad de filas afectadas. 
        //        //Se espera que se haya modificado solo un registro, por eso se lo compara con un 1.
        //        return db.Execute(query) == 1;
        //    }
        //}
        //public bool EliminarPropiedad(int IdPropiedad)
        //{
        //    using (IDbConnection db = new SqlConnection(_connectionString))
        //    {
        //        string query = @"DELETE FROM Grupo3.propiedad WHERE id_Propiedad = @IdPropiedad";
        //        return db.Execute(query, new { IdPropiedad }) == 1; // Devuelve verdadero si se eliminó una fila
        //    }
        //}
        public bool EliminarPropiedad(int IdPropiedad)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Grupo3.propiedad WHERE id_Propiedad = @IdPropiedad";

                // Ejecutar la consulta y devolver si se eliminó un registro
                return db.Execute(query, new { IdPropiedad }) == 1;
            }
        }
    }


}
