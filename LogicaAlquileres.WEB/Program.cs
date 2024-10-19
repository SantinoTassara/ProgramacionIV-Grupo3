using LogicaAlquileres.Managers;
using LogicaAlquileres.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IPropiedadManager, PropiedadManager>();

builder.Services.AddScoped<IPropiedadRepository>(
        _ => new PropiedadRepository(builder.Configuration["Db:ConnectionString"]));

builder.Services.AddScoped<IEstadoPropiedadRepository>(
        _ => new EstadoPropiedadRepository(builder.Configuration["Db:ConnectionString"]));

/*builder.Services.AddScoped<IUsuarioRepository>(
        _ => new UsuarioRepository(builder.Configuration["Db:ConnectionString"]));
*/


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
