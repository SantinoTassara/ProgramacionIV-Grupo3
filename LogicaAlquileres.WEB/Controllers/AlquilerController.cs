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

        [Authorize(Roles = "Usuario, Administrador")]  // Lo ven ambos
        public IActionResult Index()
        {
            // Obtener las propiedades (las mismas que en el controlador Propiedad)
            var propiedades = _propiedadManager.GetPropiedades();

            // Devolverlas a la vista
            return View(propiedades);
        }
        public ActionResult Alquilar(int id)
        {
            // Puedes obtener la propiedad por ID u otros parámetros si es necesario
            var propiedad = _propiedadManager.GetPropiedad(id);

            if (propiedad == null)
            {
                return NotFound(); // Si no se encuentra la propiedad, redirigir a la página de error o a la lista
            }

            // Puedes pasar el modelo de la propiedad a la vista
            return View(propiedad);
        }

        // Acción que maneja la solicitud de ver los detalles de una propiedad
        [Authorize(Roles = "Usuario, Administrador")] // Ambos roles lo pueden ver
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
