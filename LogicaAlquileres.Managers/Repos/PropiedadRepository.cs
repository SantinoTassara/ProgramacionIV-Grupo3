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
        Propiedad GetPropiedad(int idPropiedad);

        IEnumerable<Propiedad> GetPropiedades(bool? SoloActivos = true);

        IEnumerable<PropiedadCompleto> GetPropiedadesCompleto();

        int CrearPropiedad(Propiedad propiedad);
        bool ModificarPropiedad(int IdPropiedad, Propiedad propiedad);
        bool EliminarPropiedad(int IdPropiedad, int IdUsuarioBaja);
    }

    public class PropiedadRepository : IPropiedadRepository
    {
        private string _connectionString;

        public PropiedadRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Consulta a la base de datos por Id
        public Propiedad GetPropiedad(int IdPropiedad)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                Propiedad result = conn.QuerySingle<Propiedad>("Select * from Grupo3.propiedad Where id_Propiedad = " + IdPropiedad.ToString());
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
        /*
        public int CrearPropiedad(Propiedad propiedad)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Grupo3.propiedad (id_Usuario_Propiedad, id_Alquiler, direccion_Propiedad, estado_Propiedad, precio_Propiedad, nombre_Propiedad, descripcion_Propiedad, fechaAlta_Propiedad, fechaBaja_Propiedad, fechaModificacion_Propiedad)  
                                VALUES ( @id_Usuario_Propiedad, @id_Alquiler, @direccion_Propiedad, @estado_Propiedad, @precio_Propiedad, @nombre_Propiedad, @descripcion_Propiedad, @fechaAlta_Propiedad, @fechaBaja_Propiedad, @fechaModificacion_Propiedad);
                                SELECT CAST(SCOPE_IDENTITY() AS INT)";

                propiedad.id_Propiedad = db.QuerySingle<int>(query, propiedad);

                return propiedad.id_Propiedad;
            }
        }
        */
        public int CrearPropiedad(Propiedad propiedad)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Grupo3.propiedad (id_Usuario_Propiedad, id_Alquiler, direccion_Propiedad, estado_Propiedad, precio_Propiedad, nombre_Propiedad, descripcion_Propiedad, fechaAlta_Propiedad)  
                        VALUES (@id_Usuario_Propiedad, @id_Alquiler, @direccion_Propiedad, @estado_Propiedad, @precio_Propiedad, @nombre_Propiedad, @descripcion_Propiedad, @fechaAlta_Propiedad);
                        SELECT CAST(SCOPE_IDENTITY() AS INT)";

                var id = db.QuerySingle<int>(query, propiedad);
                return id;
            }
        }

        // Modificar Propiedad en la base de Datos
        public bool ModificarPropiedad(int IdPropiedad, Propiedad propiedad)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE 
                                    Grupo3.propiedad 
                                SET 
                                    id_Usuario_Propiedad = @IdUsuario, 
                                    id_Alquiler = @IdAquiler, 
                                    direccion_Propiedad = @Direccion, 
                                    estado_Propiedad  = @Estado
                                    precio_Propiedad = @Precio
                                    nombre_Propiedad = @Nombre
                                    descripcion_Propiedad = @Descripcion
                                    WHERE id_Propiedad = " + IdPropiedad.ToString();//modificado a nuestra base, probar..
                //db.execute devuelve un entero que representa la cantidad de filas afectadas. 
                //Se espera que se haya modificado solo un registro, por eso se lo compara con un 1.
                return db.Execute(query, propiedad) == 1;
            }
        }

        // Eliminar de manera lógica una Propiedad de la base de datos
        public bool EliminarPropiedad(int IdPropiedad, int IdUsuarioBaja)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                //hay que modificar la tabla para que elimine una propiedad con fecha de salida, checkout, osea mover el checkout a la tabla propiedad
                string query = @"UPDATE 
                                    Grupo3.alquiler  
                                SET 
                                    
                                    checkOut_Alquiler = '" + DateTime.Now.ToString("yyyyMMdd") + "'," +
                                    " id_Inquilino_alquiler  = " + IdUsuarioBaja +
                                "WHERE id_propiedad = " + IdPropiedad.ToString();//modificado a nuestra base, probar..
                //db.execute devuelve un entero que representa la cantidad de filas afectadas. 
                //Se espera que se haya modificado solo un registro, por eso se lo compara con un 1.
                return db.Execute(query) == 1;
            }
        }
    }


}
