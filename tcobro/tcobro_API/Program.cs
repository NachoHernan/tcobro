using Microsoft.EntityFrameworkCore;
using tcobro_API;
using tcobro_API.Datos;
using tcobro_API.Modelos;
using tcobro_API.Repositorio;
using tcobro_API.Repositorio.IRepositorio;

var builder = WebApplication.CreateBuilder(args);
    
             /*Servicios a consumir*/


builder.Services.AddControllers().AddNewtonsoftJson();//Paquete instalado
//Servicios ya predefinidos en VisualStudio
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Relacion clase 'ApplicationDbContext' con cadena de conexion y con MySQL
var connectionString = builder.Configuration.GetConnectionString("AppDbConnectionString");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

//Servicio de Automapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

//Agrega interfaz con su implementacion
    //Scoped: se crea una vez por solicitud y luego se destruye
    //Singleton: se crea la primera vez que se solicita y cada solicitud posterior utiliza la misma instancia
    //Transient: se crea cada vez que se solicita (servicios livianos y sin estado)
builder.Services.AddScoped<IEmpresaRepositorio, EmpresaRepositorio>();
builder.Services.AddScoped<IMaquinaRepositorio, MaquinaRepositorio>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
