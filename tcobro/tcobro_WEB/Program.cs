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


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
