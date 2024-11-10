using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogicaAlquileres.WEB.Controllers
{
    public class AlquilerController : Controller
    {
        [Authorize(Roles = "Usuario, Administrador")]//lo ven ambos
        public IActionResult Index()
        {
            return View();
        }
    }
}
