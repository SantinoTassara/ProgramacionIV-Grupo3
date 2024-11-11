using LogicaAlquileres.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogicaAlquileres.WEB.Controllers
{
    public class AlquilerController : Controller
    {
        private readonly IPropiedadManager _propiedadManager;

        public AlquilerController(IPropiedadManager propiedadManager)
        {
            _propiedadManager = propiedadManager;
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
            // Obtener la propiedad por ID
            var propiedad = _propiedadManager.GetPropiedad(id);

            if (propiedad == null)
            {
                return NotFound(); // Si no se encuentra la propiedad, redirigir a la página de error o a la lista
            }

            // Pasar el modelo de la propiedad a la vista
            return View(propiedad);
        }

        // Acción para ver los detalles de una propiedad
        [Authorize(Roles = "Usuario, Administrador")] // Ambos roles pueden ver los detalles
        public IActionResult Details(int id)
        {
            // Obtener la propiedad desde el manager
            var propiedad = _propiedadManager.GetPropiedad(id);

            // Si no se encuentra la propiedad, devolver un error 404
            if (propiedad == null)
            {
                return NotFound();
            }

            // Pasamos la propiedad directamente a la vista
            return View(propiedad);
        }
    }
}
