using Microsoft.AspNetCore.Authentication.Cookies;
using tcobro_WEB;
using tcobro_WEB.Services;
using tcobro_WEB.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//Servicio de Automapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

//Agrega interfaz con su implementacion

/*Scoped: se crea una vez por solicitud y luego se destruye
  Singleton: se crea la primera vez que se solicita y cada solicitud posterior utiliza la misma instancia
  Transient: se crea cada vez que se solicita (servicios livianos y sin estado)*/
builder.Services.AddHttpClient<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();

builder.Services.AddHttpClient<IMaquinaService, MaquinaService>();
builder.Services.AddScoped<IMaquinaService, MaquinaService>();

builder.Services.AddHttpClient<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

//Configuraciones para manejo de sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//Sesion de usuarios
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Manejo de autenticacion por medio de cookies para su aceptacion
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                                    .AddCookie(options=>
                                     {
                                         options.Cookie.HttpOnly = true;
                                         options.ExpireTimeSpan = TimeSpan.FromMinutes(100);
                                         options.LoginPath = "/Usuario/Login";
                                         options.AccessDeniedPath = "/Usuario/AccesoDenegado";
                                         options.SlidingExpiration = true;
                                     });


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();//Authentication siempre antes de Authorizacion

app.UseAuthorization();
//Manejo de sesiones de usuario
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
