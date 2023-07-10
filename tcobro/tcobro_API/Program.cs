using Microsoft.EntityFrameworkCore;
using tcobro_API;
using tcobro_API.Datos;
using tcobro_API.Modelos;

var builder = WebApplication.CreateBuilder(args);

//Servicios a consumir
builder.Services.AddControllers().AddNewtonsoftJson();//Paquete instalado
//Servicios ya predefinidos en VisualStudio
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Relacion clase 'ApplicationDbContext' con cadena de conexion y con MySQL
var connectionString = builder.Configuration.GetConnectionString("AppDbConnectionString");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

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
