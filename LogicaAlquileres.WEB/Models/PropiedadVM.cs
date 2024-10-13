using LogicaAlquileres.Managers.Entidades;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogicaAlquileres.WEB.Models
{
    public class PropiedadVM
    {
        public Propiedad model { get; set; } //agregue esto

        public List<SelectListItem> ListaEstadosItem { get; set; }//agregue esto


    }
}
