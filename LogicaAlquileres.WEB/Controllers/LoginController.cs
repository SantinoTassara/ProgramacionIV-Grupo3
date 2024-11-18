using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using LogicaAlquileres.Managers.Entidades;
using LogicaAlquileres.Managers.Repos;

namespace LogicaAlquileres.WEB.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public LoginController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult Index()
        {
            if (HttpContext.User.Identities.First().IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // Login con Google
        public async Task Login()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }

        [HttpGet("/signin-google")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;

            // Asignar rol de Administrador si el email coincide
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.Principal.FindFirst(ClaimTypes.Name)?.Value ?? ""),
                new Claim(ClaimTypes.Email, email)
            };

            if (email == "admin@example.com") // reemplaza con el email del administrador con google, alguno de nuestros mails
            {
                claims.Add(new Claim(ClaimTypes.Role, "Administrador"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Usuario"));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // Login con base de datos
        [HttpPost]
        public async Task<IActionResult> LoginWithCredentials(string email, string password)
        {
            var usuario = _usuarioRepository.GetUsuarioPorEmailYContraseña(email, password);
            if (usuario != null)
            {
                // Crear claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.nombre_Usuario),
                    new Claim(ClaimTypes.Email, usuario.email_Usuario),
                    new Claim(ClaimTypes.NameIdentifier, usuario.id_usuario.ToString()) //para el boton alquilar
                };

                // Asignar rol en función del correo electrónico
                if (usuario.email_Usuario == "pepe@admin.com") // reemplaza con el email del administrador
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Administrador"));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Usuario"));
                }

                // Crear la identidad y firmar la cookie de autenticación
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Si el usuario no es encontrado, muestra el mensaje de error en la vista
                ViewBag.ErrorMessage = "Email o contraseña incorrectos";
                return View("Index");
            }

            // Manejar login fallido
            //ModelState.AddModelError("", "Email o contraseña incorrectos");
            //return View("Index");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}

