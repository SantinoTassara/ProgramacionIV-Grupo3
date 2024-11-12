using System.Security.Claims;
using LogicaAlquileres.Managers;
using LogicaAlquileres.Managers.Entidades;
using LogicaAlquileres.Managers.Repos;
using LogicaAlquileres.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogicaAlquileres.WEB.Controllers
{
    public class AlquilerController : Controller
    {
        private readonly IPropiedadManager _propiedadManager;
        private readonly IAlquilerRepository _alquilerRepository;
        private readonly IPropiedadRepository _propiedadRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        public AlquilerController(IPropiedadManager propiedadManager, IAlquilerRepository alquilerRepository, IPropiedadRepository propiedadRepository, IUsuarioRepository usuarioRepository)
        {
            _propiedadManager = propiedadManager;
            _alquilerRepository = alquilerRepository;
            _propiedadRepository = propiedadRepository;
            _usuarioRepository = usuarioRepository;
        }

        // Acción que muestra las propiedades disponibles para alquiler
        [Authorize(Roles = "Usuario, Administrador")]  // Lo ven ambos
        public IActionResult Index()
        {
            // Obtener las propiedades solo disponibles
            var propiedadesDisponibles = _propiedadManager.GetPropiedadesDisponibles();

            // Devolverlas a la vista
            return View(propiedadesDisponibles);
        }

        // Acción para alquilar una propiedad
        public ActionResult Alquilar(int id)
        {
            // Obtener el ID del usuario autenticado
            //int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int idUsuario;

            // Verificar si el NameIdentifier es numérico
            if (int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out idUsuario))
            {
                // Si es numérico, continuar normalmente
            }
            else
            {
                // Si no es numérico (es un usuario de Google), buscar el id_usuario en la base de datos
                var googleId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var usuario = _usuarioRepository.GetUsuarioPorGoogleSubject(googleId);
                if (usuario == null)
                {
                    // Manejar el caso donde el usuario no se encuentra
                    return Unauthorized();
                }
                idUsuario = usuario.id_usuario;
            }


            // Obtener los detalles de la propiedad
            var propiedad = _propiedadRepository.GetPropiedad(id);

            // Crear el alquiler en la tabla, incluyendo id_Propiedad y nombre_Propiedad
            int idAlquiler = _alquilerRepository.CrearAlquiler(
                idUsuario,
                DateTime.Now,
                DateTime.Now.AddDays(30),
                propiedad.precio_Propiedad,
                propiedad.direccion_Propiedad,
                propiedad.id_Propiedad,
                propiedad.nombre_Propiedad
            );

            Console.WriteLine("ID de alquiler creado: " + idAlquiler);

            // Cambiar el estado de la propiedad a "Ocupado" si el alquiler se creó correctamente
            if (idAlquiler > 0)
            {
                _alquilerRepository.ActualizarEstadoPropiedad(propiedad.id_Propiedad, "Ocupado");
            }

            return RedirectToAction("MisAlquileres");
        }

        [Authorize(Roles = "Usuario, Administrador")] // Ambos roles pueden ver los detalles
        public IActionResult Details(int id)
        {
            
            var propiedad = _propiedadManager.GetPropiedad(id);

            // Si no se encuentra la propiedad, devolver un error 404
            if (propiedad == null)
            {
                return NotFound();
            }

            return View(propiedad);
        }
        // Método para mostrar los alquileres del usuario
        public IActionResult MisAlquileres()
        {
            // Obtener el ID del usuario autenticado
            //int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int idUsuario;

            // Verificar si el NameIdentifier es numérico
            if (int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out idUsuario))
            {
                // Si es numérico, continuar normalmente
            }
            else
            {
                // Si no es numérico (es un usuario de Google), buscar el id_usuario en la base de datos
                var googleId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var usuario = _usuarioRepository.GetUsuarioPorGoogleSubject(googleId);
                if (usuario == null)
                {
                    // Manejar el caso donde el usuario no se encuentra
                    return Unauthorized();
                }
                idUsuario = usuario.id_usuario;
            }


            IEnumerable<Alquiler> alquileres;
            // Verificar si el usuario es un administrador
            if (User.IsInRole("Administrador"))
            {
                // Si es administrador, obtener todos los alquileres
                alquileres = _alquilerRepository.ObtenerTodosLosAlquileres();  // Método que obtiene todos los alquileres
            }
            else
            {
                // Si no es administrador, solo obtener los alquileres del usuario autenticado
                alquileres = _alquilerRepository.ObtenerAlquileresPorUsuario(idUsuario);
            }

            return View(alquileres);
        }

        [HttpPost]
        public IActionResult DarDeBaja(int idAlquiler)
        {
            // Obtener el alquiler por ID
            var alquiler = _alquilerRepository.ObtenerAlquilerPorId(idAlquiler);
            if (alquiler != null)
            {
                // Cambiar el estado de la propiedad a "Disponible"
                _alquilerRepository.ActualizarEstadoPropiedad(alquiler.id_Propiedad, "Disponible");

                // Eliminar el alquiler de la tabla alquiler
                _alquilerRepository.EliminarAlquiler(idAlquiler);
            }

            return RedirectToAction("MisAlquileres");
        }
    }
}
