using LogicaAlquileres.Managers;
using LogicaAlquileres.Managers.Entidades;
using LogicaAlquileres.Managers.ModelFactories;
using LogicaAlquileres.Repos;
using LogicaAlquileres.WEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogicaAlquileres.WEB.Controllers
{
    public class PropiedadController : Controller
    {
        private IPropiedadManager _propiedadManager;
        private IAlquilerRepository _estadoPropiedadRepository;

        public PropiedadController (IPropiedadManager propiedadManager, IAlquilerRepository estadoPropiedadRepository)
        {
            _propiedadManager = propiedadManager;
            _estadoPropiedadRepository = estadoPropiedadRepository;
        }
        // GET: PropiedadController
        [Authorize(Roles = "Administrador")]//solo para admin
        public ActionResult Index()
        {
            var propiedades = _propiedadManager.GetPropiedades();

            return View(propiedades);
        }

        // GET: PropiedadController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var propiedad = _propiedadManager.GetPropiedad(id);
                var propiedadCompleto = new PropiedadCompleto
                {
                    id_Propiedad = propiedad.id_Propiedad,
                    //id_Usuario_Propiedad = propiedad.id_Usuario_Propiedad,
                    //id_Alquiler = propiedad.id_Alquiler,
                    direccion_Propiedad = propiedad.direccion_Propiedad,
                    estado_Propiedad = propiedad.estado_Propiedad,
                    precio_Propiedad = propiedad.precio_Propiedad,
                    nombre_Propiedad = propiedad.nombre_Propiedad,
                    descripcion_Propiedad = propiedad.descripcion_Propiedad,
                    fechaAlta_Propiedad = propiedad.fechaAlta_Propiedad,
                    fechaBaja_Propiedad = propiedad.fechaBaja_Propiedad,
                    fechaModificacion_Propiedad = propiedad.fechaModificacion_Propiedad
                };

                return View(propiedadCompleto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: PropiedadController/Create
        public ActionResult Create()
        {
            PropiedadVM propiedadVM = new PropiedadVM();
            propiedadVM.model = null;
           
            return View(propiedadVM);
        }

        // POST: PropiedadController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Propiedad propiedad = new Propiedad
                {
                    //hay q ver si entran todos, id alquiler e id usuario
                    //id_Propiedad = int.Parse(collection["model.id_Propiedad=3"]),
                    //id_Propiedad = int.Parse(collection["model.id_Propiedad"]),
                    //id_Usuario_Propiedad = int.Parse(collection["model.id_Usuario_Propiedad"]),
                    //id_Alquiler = int.Parse(collection["model.id_Alquiler"]),
                    direccion_Propiedad = collection["model.direccion_Propiedad"],
                    estado_Propiedad = collection["model.estado_Propiedad"],
                    precio_Propiedad = decimal.Parse(collection["model.precio_Propiedad"]),
                    nombre_Propiedad = collection["model.nombre_Propiedad"],
                    descripcion_Propiedad = collection["model.descripcion_Propiedad"]

                };
                //int idUsuario = GetUserIdentityId();

                _propiedadManager.CrearPropiedad(propiedad/*, idUsuario*/);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Registra el error
                Console.WriteLine($"Error al crear la propiedad: {ex.ToString()}");
                ModelState.AddModelError("", "Ocurrió un error al crear la propiedad. Detalles: " + ex.Message);
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var propiedad = _propiedadManager.GetPropiedad(id);
            if (propiedad == null)
            {
                return NotFound(); 
            }

            PropiedadVM propiedadVM = new PropiedadVM
            {
                model = propiedad
            };

            return View(propiedadVM);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                Propiedad propiedad = new Propiedad
                {
                    id_Propiedad = id,
                    direccion_Propiedad = collection["model.direccion_Propiedad"],
                    estado_Propiedad = collection["model.estado_Propiedad"],
                    precio_Propiedad = decimal.Parse(collection["model.precio_Propiedad"]),
                    nombre_Propiedad = collection["model.nombre_Propiedad"],
                    descripcion_Propiedad = collection["model.descripcion_Propiedad"]
                };

                bool result = _propiedadManager.ModificarPropiedad(id, propiedad); // Verifica que la llamada a la modificación devuelva verdadero

                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo actualizar la propiedad.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error: " + ex.Message);
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            // Obtener la propiedad por ID
            var propiedad = _propiedadManager.GetPropiedad(id);

            
            if (propiedad == null)
            {
                return NotFound(); 
            }

            // Crear y devolver la vista con el modelo
            PropiedadVM propiedadVM = new PropiedadVM { model = propiedad };
            return View(propiedadVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // Intentar eliminar la propiedad
                bool eliminado = _propiedadManager.EliminarPropiedad(id);

                // Comprobar si se eliminó correctamente
                if (!eliminado)
                {
                    ModelState.AddModelError("", "No se pudo eliminar la propiedad. Puede que no exista.");
                    return View(); // Vuelve a mostrar la vista de eliminación
                }

                // Redirigir a la lista de propiedades si se elimina con éxito
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ocurrió un error: {ex.Message}");
                return View(); // Vuelve a mostrar la vista de eliminación
            }
        }

        //private int GetUserIdentityId()
        //{
        //    //return int.Parse(HttpContext.User.Claims.First(x => x.Type == "usuarioPropiedad").Value);
        //    var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "usuarioPropiedad");
        //    if (claim == null) throw new Exception("Claim 'usuarioPropiedad' no encontrado.");
        //    return int.Parse(claim.Value);
        //}

    }
}
