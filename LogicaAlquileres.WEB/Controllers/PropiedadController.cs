using LogicaAlquileres.Managers;
using LogicaAlquileres.Managers.Entidades;
using LogicaAlquileres.Repos;
using LogicaAlquileres.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogicaAlquileres.WEB.Controllers
{
    public class PropiedadController : Controller
    {
        private IPropiedadManager _propiedadManager;
        private IEstadoPropiedadRepository _estadoPropiedadRepository;

        public PropiedadController (IPropiedadManager propiedadManager, IEstadoPropiedadRepository estadoPropiedadRepository)
        {
            _propiedadManager = propiedadManager;
            _estadoPropiedadRepository = estadoPropiedadRepository;
        }
        // GET: PropiedadController
        public ActionResult Index()
        {
            var propiedades = _propiedadManager.GetPropiedades();

            return View(propiedades);
        }

        // GET: PropiedadController/Details/5
        public ActionResult Details(int id)
        {
            //esta vista no la cree todavia

            return View();
        }

        // GET: PropiedadController/Create
        public ActionResult Create()
        {
            PropiedadVM propiedadVM = new PropiedadVM();
            propiedadVM.model = null;
            /*propiedadVM.ListaEstadosItem = new List<SelectListItem>();
            var estados = _estadoPropiedadRepository.GetEstadosPropiedad();
            foreach (var estado in estados)
            {
                propiedadVM.ListaEstadosItem.Add(new SelectListItem { Value = estado.IdEstadoPropiedad.ToString(), Text = estado.Descripcion });//comprobar idEstado y descripcio
            }
            */
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
                    id_Usuario_Propiedad = int.Parse(collection["model.id_Usuario_Propiedad"]),
                    id_Alquiler = int.Parse(collection["model.id_Alquiler"]),
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
            /* catch
             {
                 return View();
             }
            */
            catch (Exception ex)
            {
                // Registra el error
                Console.WriteLine($"Error al crear la propiedad: {ex.ToString()}");
                ModelState.AddModelError("", "Ocurrió un error al crear la propiedad. Detalles: " + ex.Message);
                return View();
            }
        }

        

        // GET: PropiedadController/Edit/5
        public ActionResult Edit(int id)
        {
            var propiedad = _propiedadManager.GetPropiedad(id);
            var estados = _estadoPropiedadRepository.GetEstadosPropiedad();

            PropiedadVM propiedadVM = new PropiedadVM();
            propiedadVM.model = propiedad;
            /*propiedadVM.ListaEstadosItem = new List<SelectListItem>();
            foreach (var estado in estados)
            {
                propiedadVM.ListaEstadosItem.Add(new SelectListItem { Value = estado.IdEstadoPropiedad.ToString(), Text = estado.Descripcion });//comprobar idEstado y descripcio

            }
            */
            return View(propiedadVM);
        }

        // POST: PropiedadController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                Propiedad propiedad = new Propiedad
                {

                    //hay q ver si entran todos, id alquiler e id usuario

                    direccion_Propiedad = collection["model.Direccion"],
                    estado_Propiedad = collection["model.Estado"],
                    precio_Propiedad = decimal.Parse(collection["model.Precio"]),
                    nombre_Propiedad = collection["model.Nombre"],
                    descripcion_Propiedad = collection["model.Descripcion"]

                };
                int idUsuario = GetUserIdentityId();

                _propiedadManager.ModificarPropiedad(id, propiedad, idUsuario);


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PropiedadController/Delete/5
        public ActionResult Delete(int id)
        {
            var propiedad = _propiedadManager.GetPropiedad(id);
            var estados = _estadoPropiedadRepository.GetEstadosPropiedad();

            PropiedadVM propiedadVM = new PropiedadVM();
            propiedadVM.model = propiedad;
            /*propiedadVM.ListaEstadosItem = new List<SelectListItem>();
            foreach (var estado in estados)
            {
                propiedadVM.ListaEstadosItem.Add(new SelectListItem { Value = estado.IdEstadoPropiedad.ToString(), Text = estado.Descripcion });//comprobar idEstado y descripcio
            }
            */
            return View(propiedadVM);
        }
        
        // POST: PropiedadController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                int idUsuario = GetUserIdentityId();

                _propiedadManager.EliminarPropiedad(id, idUsuario);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
        
        
        private int GetUserIdentityId()
        {
            //return int.Parse(HttpContext.User.Claims.First(x => x.Type == "usuarioPropiedad").Value);
            var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "usuarioPropiedad");
            if (claim == null) throw new Exception("Claim 'usuarioPropiedad' no encontrado.");
            return int.Parse(claim.Value);
        }
        
    }
}
