using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using LogicaAlquileres.Managers.Entidades;
using LogicaAlquileres.WEB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogicaAlquileres.WEB.Controllers
{
    public class PropiedadController : Controller
    {
        private static readonly List<Propiedad> propiedades = new List<Propiedad>
        {
            new Propiedad { IdPropiedad = 1, IdUsuario = 1, IdAquiler = 12414, Direccion = "Calle 123", Estado = "Disponible", Precio = 1000, Nombre = "Casa en el centro", Descripcion = "Ubicada en el centro." },
            new Propiedad { IdPropiedad = 2, IdUsuario = 2, IdAquiler = 12415, Direccion = "Calle 233", Estado = "Disponible", Precio = 1000, Nombre = "Casa en el centro", Descripcion = "Ubicada en el centro." },
            new Propiedad { IdPropiedad = 3, IdUsuario = 3, IdAquiler = 12416, Direccion = "Calle 55", Estado = "Disponible", Precio = 1000, Nombre = "Casa en la costa xd", Descripcion = "Ubicada en la costa." },
           
        };

        // GET: PropiedadController
        public ActionResult Index()
        {
            return View(propiedades);
        }

        // GET: PropiedadController/Create
        public ActionResult Create()
        {
            var model = new PropiedadVM
            {
                model = new Propiedad(),
                ListaEstadosItem = new List<SelectListItem>
        {
            new SelectListItem { Value = "Disponible", Text = "Disponible" },
            new SelectListItem { Value = "Alquilada", Text = "Alquilada" }
        }
            };
            return View(model);
        }

        // POST: PropiedadController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PropiedadVM propiedadVM)
        {
            
            var nuevaPropiedad = propiedadVM.model;

            
            nuevaPropiedad.IdPropiedad = propiedades.Count + 1;

           
            nuevaPropiedad.IdUsuario = propiedades.Count + 1;

          
            nuevaPropiedad.IdAquiler = propiedades.Count + 1;

            propiedades.Add(nuevaPropiedad); 
            return RedirectToAction(nameof(Index));
        }



        // GET: PropiedadController/Edit/5
        public ActionResult Edit(int id)
        {
            var propiedad = propiedades.FirstOrDefault(p => p.IdPropiedad == id);
            if (propiedad == null)
            {
                return NotFound();
            }

            var model = new PropiedadVM
            {
                model = propiedad,
                ListaEstadosItem = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Disponible", Text = "Disponible", Selected = propiedad.Estado == "Disponible" },
                    new SelectListItem { Value = "Alquilada", Text = "Alquilada", Selected = propiedad.Estado == "Alquilada" }
                }
            };

            return View(model);
        }

        // POST: PropiedadController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PropiedadVM propiedadVM)
        {
           
            {
                var propiedad = propiedades.FirstOrDefault(p => p.IdPropiedad == id);
                if (propiedad != null)
                {
                    propiedad.IdUsuario = propiedadVM.model.IdUsuario;
                    propiedad.IdAquiler = propiedadVM.model.IdAquiler;
                    propiedad.Direccion = propiedadVM.model.Direccion;
                    propiedad.Estado = propiedadVM.model.Estado;
                    propiedad.Precio = propiedadVM.model.Precio;
                    propiedad.Nombre = propiedadVM.model.Nombre;
                    propiedad.Descripcion = propiedadVM.model.Descripcion;

                    return RedirectToAction(nameof(Index));
                }
            }

            // Si hay errores, vuelve a cargar la lista de estados
            propiedadVM.ListaEstadosItem = new List<SelectListItem>
            {
                new SelectListItem { Value = "Disponible", Text = "Disponible" },
                new SelectListItem { Value = "Alquilada", Text = "Alquilada" }
            };
            return View(propiedadVM);
        }

        // GET: PropiedadController/Delete/5
        public ActionResult Delete(int id)
        {
            var propiedad = propiedades.FirstOrDefault(p => p.IdPropiedad == id);
            if (propiedad == null)
            {
                return NotFound();
            }
            return View(propiedad);
        }

        // POST: PropiedadController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var propiedad = propiedades.FirstOrDefault(p => p.IdPropiedad == id);
            if (propiedad != null)
            {
                propiedades.Remove(propiedad);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
