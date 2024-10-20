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
    public interface IEstadoPropiedadRepository 
    {
        EstadoPropiedad GetEstadoPropiedad(int IdEstadoPropiedad);
        IEnumerable<EstadoPropiedad> GetEstadosPropiedad(bool? SoloActivos = true);
    }

    public class EstadoPropiedadRepository : IEstadoPropiedadRepository
    {
        private string _connectionString;

        public EstadoPropiedadRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        //Consulta a la base de datos por Id
        public EstadoPropiedad GetEstadoPropiedad(int IdEstadoPropiedad) 
        {
            using (IDbConnection conn = new SqlConnection(_connectionString)) 
            {
                EstadoPropiedad result = conn.QuerySingle<EstadoPropiedad>("Select * from Grupo3.alquiler Where id_alquiler = " + IdEstadoPropiedad.ToString());
                return result;
            }
        }

        // Consulta a la base de datos por la lista de las propiedades activas

        public IEnumerable<EstadoPropiedad> GetEstadosPropiedad(bool? SoloActivos = true) 
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                //revisar querys segun tabla
                string query = "Select * from Grupo3.alquiler";
                /*if (SoloActivos == true)
                    query += "where FechaBaja is null";*/ 
                //nuestra tabla alquiler no admite null en las fechas, hay q modificar eso
                IEnumerable<EstadoPropiedad> result = conn.Query<EstadoPropiedad>(query);
                return result;
            }
        }
    }
}
