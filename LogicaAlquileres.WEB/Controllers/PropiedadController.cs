using LogicaAlquileres.Managers.Entidades;
using LogicaAlquileres.Managers.Managers;
using LogicaAlquileres.WEB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogicaAlquileres.WEB.Controllers
{
    public class PropiedadController : Controller
    {
        private IPropiedadManager _propiedadManager;
        private IEstadoPropiedadRepository _estadoPropiedadRepository; //Falta crear este repositorio

        public PropiedadController (IPropiedadManager propiedadManager, IEstadoPropiedadRepository estadoPropiedadRepository)
        {
            _propiedadManager = propiedadManager;
            _estadoPropiedadRepository = estadoPropiedadRepository;
        }
        // GET: PropiedadController
        public ActionResult Index()
        {
            var propiedades = _propiedadManager.GetPropiedades();

            return View();
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
            propiedadVM.ListaEstadosItem = new List<SelectListItem>();
            var estados = _estadoPropiedadRepository.GetEstadosPropiedad();
            foreach (var estado in estados)
            {
                propiedadVM.ListaEstadosItem.Add(new SelectListItem { Value = estado.IdEstadoPropiedad.ToString(), Text = estado.Descripcion });//comprobar idEstado y descripcio
            }

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

                    Direccion = collection["model.Direccion"],
                    Estado = collection["model.Estado"],
                    Precio = double.Parse(collection["model.Precio"]),
                    Nombre = collection["model.Nombre"],
                    Descripcion = collection["model.Descripcion"]

                };
                int idUsuario = GetUserIdentityId();

                _propiedadManager.CrearPropiedad(propiedad, idUsuario);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
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
            propiedadVM.ListaEstadosItem = new List<SelectListItem>();
            foreach (var estado in estados)
            {
                propiedadVM.ListaEstadosItem.Add(new SelectListItem { Value = estado.IdEstadoPropiedad.ToString(), Text = estado.Descripcion });//comprobar idEstado y descripcio

            }

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

                    Direccion = collection["model.Direccion"],
                    Estado = collection["model.Estado"],
                    Precio = double.Parse(collection["model.Precio"]),
                    Nombre = collection["model.Nombre"],
                    Descripcion = collection["model.Descripcion"]

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
            propiedadVM.ListaEstadosItem = new List<SelectListItem>();
            foreach (var estado in estados)
            {
                propiedadVM.ListaEstadosItem.Add(new SelectListItem { Value = estado.IdEstadoPropiedad.ToString(), Text = estado.Descripcion });//comprobar idEstado y descripcio
            }

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
            return int.Parse(HttpContext.User.Claims.First(x => x.Type == "usuarioPropiedad").Value);//Ver en la clase 7
        }
    }
}
