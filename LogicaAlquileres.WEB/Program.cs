using LogicaAlquileres.Managers;
using LogicaAlquileres.Managers.Entidades;
using LogicaAlquileres.Managers.Repos;
using LogicaAlquileres.Repos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IPropiedadManager, PropiedadManager>();
builder.Services.AddScoped<IPropiedadRepository>(
        _ => new PropiedadRepository(builder.Configuration["Db:ConnectionString"]));
builder.Services.AddScoped<IAlquilerRepository>(
        _ => new AlquilerRepository(builder.Configuration["Db:ConnectionString"]));
builder.Services.AddScoped<IUsuarioRepository>(
        _ => new UsuarioRepository(builder.Configuration["Db:ConnectionString"]));

// Configuración de autenticación
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Login/Index"; // Redirecciona a la página de login cuando no está autenticado
})
.AddGoogle(GoogleDefaults.AuthenticationScheme, opciones =>
{
    opciones.ClientId = builder.Configuration.GetSection("GooglaKeys:ClientId").Value + ".apps.googleusercontent.com";
    opciones.ClientSecret = builder.Configuration.GetSection("GooglaKeys:ClientPriv").Value;

    opciones.Events.OnCreatingTicket = async ctx =>
    {
        var usuarioServicio = ctx.HttpContext.RequestServices.GetRequiredService<IUsuarioRepository>();
        var email = ctx.Identity.FindFirst(ClaimTypes.Email)?.Value;

        string googleNameIdentifier = ctx.Identity.Claims
            .First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
            .Value.ToString();

        var usuario = usuarioServicio.GetUsuarioPorGoogleSubject(googleNameIdentifier);

        int idUsuario = 0;
        if (usuario == null)
        {
            // Crea un nuevo usuario si no existe
            Usuario usuarioNuevo = new Usuario
            {
                apellido_Usuario = ctx.Identity.FindFirst(ClaimTypes.Surname)?.Value,
                nombre_Usuario = ctx.Identity.FindFirst(ClaimTypes.GivenName)?.Value,
                //NombreCompleto = ctx.Identity.FindFirst(ClaimTypes.Name)?.Value,
                GoogleIdentificador = googleNameIdentifier,
                //Borrado = false,
                email_Usuario = email,
                //IdUsuarioAlta = 1,
               
            };
            idUsuario = usuarioServicio.CrearUsuario(usuarioNuevo);
        }
        else
        {
            idUsuario = usuario.id_usuario;
        }
        
        // Asignar rol basado en el email
        var role = email == "admin@example.com" ? "Administrador" : "Usuario";
        //ctx.Identity.AddClaim(new Claim("UNLZRole", role));
        ctx.Identity.AddClaim(new Claim(ClaimTypes.Role, role));
        //ctx.Identity.AddClaim(new Claim("usuarioContainer", idUsuario.ToString()));
       /* ctx.Identity.RemoveClaim(ctx.Identity.FindFirst(ClaimTypes.NameIdentifier));*/ // Eliminar el NameIdentifier existente, si existe
        ctx.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()));
        
        


        await Task.CompletedTask;
    };
});

var app = builder.Build();

// Configura el HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();

