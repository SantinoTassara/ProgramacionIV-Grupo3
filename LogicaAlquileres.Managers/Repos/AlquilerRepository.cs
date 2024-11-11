using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using LogicaAlquileres.Managers.Entidades;


namespace LogicaAlquileres.Repos
{
    public interface IAlquilerRepository 
    {
        public int CrearAlquiler(int idInquilino, DateTime checkIn, DateTime checkOut, decimal precioTotal, string direccion, int idPropiedad, string nombrePropiedad);
        public void ActualizarEstadoPropiedad(int idPropiedad, string nuevoEstado);
        public IEnumerable<Alquiler> ObtenerAlquileresPorUsuario(int idUsuario);
        public void EliminarAlquiler(int idAlquiler);
        public Alquiler ObtenerAlquilerPorId(int idAlquiler);
        public IEnumerable<Alquiler> ObtenerTodosLosAlquileres();
    }

    public class AlquilerRepository : IAlquilerRepository
    {
        private readonly string _connectionString;

        public AlquilerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int CrearAlquiler(int idInquilino, DateTime checkIn, DateTime checkOut, decimal precioTotal, string direccion, int idPropiedad, string nombrePropiedad)
        {
         
            using (var connection = new SqlConnection(_connectionString))
            {
                    connection.Open();
                    var query = @"INSERT INTO Grupo3.alquiler 
                    (id_Inquilino_Alquiler, checkIn_Alquiler, checkOut_Alquiler, precioTotal_Alquiler, direccion_Alquiler, id_Propiedad, nombre_Propiedad)
                  VALUES 
                    (@idInquilino, @checkIn, @checkOut, @precioTotal, @direccion, @idPropiedad, @nombrePropiedad);
                  SELECT CAST(SCOPE_IDENTITY() as int)";  // Retorna el ID generado

                    int idAlquiler = connection.QuerySingle<int>(query, new
                    {
                        idInquilino,
                        checkIn,
                        checkOut,
                        precioTotal,
                        direccion,
                        idPropiedad,
                        nombrePropiedad
                    });

                    Console.WriteLine("ID de alquiler generado: " + idAlquiler);
                    return idAlquiler;
            }
            
        }


        public void ActualizarEstadoPropiedad(int idPropiedad, string nuevoEstado)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"UPDATE Grupo3.propiedad
                      SET estado_propiedad = @nuevoEstado
                      WHERE id_Propiedad = @idPropiedad";

                connection.Execute(query, new { idPropiedad, nuevoEstado });
            }
        }

        public IEnumerable<Alquiler> ObtenerAlquileresPorUsuario(int idUsuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"SELECT 
                            id_Alquiler, 
                            id_Inquilino_Alquiler, 
                            checkIn_Alquiler, 
                            checkOut_Alquiler, 
                            precioTotal_Alquiler, 
                            direccion_Alquiler,
                            id_Propiedad,
                            nombre_Propiedad
                          FROM Grupo3.alquiler
                          WHERE id_Inquilino_Alquiler = @idUsuario";
                return connection.Query<Alquiler>(query, new { idUsuario }).ToList();
            }
        }
        public void EliminarAlquiler(int idAlquiler)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"DELETE FROM Grupo3.alquiler WHERE id_Alquiler = @idAlquiler";
                connection.Execute(query, new { idAlquiler });
            }
        }

        public Alquiler ObtenerAlquilerPorId(int idAlquiler)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"SELECT 
                        id_Alquiler, 
                        id_Inquilino_Alquiler, 
                        checkIn_Alquiler, 
                        checkOut_Alquiler, 
                        precioTotal_Alquiler, 
                        direccion_Alquiler,
                        id_Propiedad,
                        nombre_Propiedad
                      FROM Grupo3.alquiler
                      WHERE id_Alquiler = @idAlquiler";
                return connection.QueryFirstOrDefault<Alquiler>(query, new { idAlquiler });
            }
        }
        public IEnumerable<Alquiler> ObtenerTodosLosAlquileres()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"SELECT 
                    id_Alquiler, 
                    id_Inquilino_Alquiler, 
                    checkIn_Alquiler, 
                    checkOut_Alquiler, 
                    precioTotal_Alquiler, 
                    direccion_Alquiler,
                    id_Propiedad,
                    nombre_Propiedad
                  FROM Grupo3.alquiler";
                return connection.Query<Alquiler>(query).ToList();
            }
        }


    }
}
