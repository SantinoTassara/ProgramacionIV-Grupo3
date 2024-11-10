using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaAlquileres.Managers.Entidades;
using Dapper;

namespace LogicaAlquileres.Managers.Repos
{
    public interface IUsuarioRepository
    {
        int CrearUsuario(Usuario usuario);
        Usuario? GetUsuarioPorGoogleSubject(string googleSubject);
        Usuario? GetUsuarioPorId(int IdUsuario);
        IEnumerable<Usuario> GetUsuarios();
        Usuario? GetUsuarioPorEmailYContraseña(string email, string contraseña);
    }
    public class UsuarioRepository : IUsuarioRepository
    {
        private string _connectionString;

        //constructor
        public UsuarioRepository(string connectionStrings)
        {

            //Connection string 
            _connectionString = connectionStrings;



        }

        public IEnumerable<Usuario> GetUsuarios()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                List<Usuario> usuarios = db.Query<Usuario>("SELECT * FROM Grupo3.usuario").ToList();

                return usuarios;

            }

            //			return PersonasDePrueba;
        }

        public Usuario? GetUsuarioPorId(int IdUsuario)
        {

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Usuario usuarios = db.Query<Usuario>("SELECT * FROM Grupo3.usuario WHERE id_usuario = " + IdUsuario.ToString()).FirstOrDefault();

                return usuarios;
            }


        }

        public Usuario? GetUsuarioPorGoogleSubject(string googleSubject)
        {

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Usuario usuarios = db.Query<Usuario>("SELECT * FROM Grupo3.usuario WHERE GoogleIdentificador = '" + googleSubject.ToString() + "'").FirstOrDefault();

                return usuarios;
            }
        }
        public int CrearUsuario(Usuario usuario)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Grupo3.usuario (GoogleIdentificador, NombreCompleto, Nombre, Apellido, Email, Borrado, IdUsuarioAlta, FechaAlta, IdUsuarioModificacion, FechaModificacion, IdUsuarioBaja, FechaBaja)  
                         VALUES (@GoogleIdentificador, @NombreCompleto, @Nombre, @Apellido, @Email, @Borrado, @IdUsuarioAlta, @FechaAlta, @IdUsuarioModificacion, @FechaModificacion, @IdUsuarioBaja, @FechaBaja);                    
                         SELECT CAST(SCOPE_IDENTITY() AS INT)";//hay q modificar esto segun nuestra tabla

                usuario.id_usuario = db.QuerySingle<int>(query, usuario);

                return usuario.id_usuario;
            }
        }
        //************ Login ****************

        public Usuario? GetUsuarioPorEmailYContraseña(string email, string contraseña)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                // verifica si el correo y la contraseña coinciden
                string query = @"
            SELECT * 
            FROM Grupo3.usuario 
            WHERE email_Usuario = @Email 
              AND password_Usuario = @Contraseña"; 
                Usuario usuario = db.Query<Usuario>(query, new { Email = email, Contraseña = contraseña }).FirstOrDefault();

                return usuario;
            }
        }
    }
}
