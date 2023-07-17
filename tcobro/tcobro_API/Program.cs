using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
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

//Documentacion de Swagger, instrucciones para autorizacion con Token
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingresar Bearer [space] tuToken \r\n\r\n " +
                      "Ejemplo: Bearer 123456abcder",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id= "Bearer"
                },
                Scheme = "oauth2",
                Name="Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

//Servicio de authenticacion para utilizacion de Bearer con su configuracion correspondiente
var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x => {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });



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
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Importante el orden de agregacion de servicios
app.UseHttpsRedirection();

app.UseAuthentication();//Authentication siempre antes que Authorization

app.UseAuthorization();

app.MapControllers();

app.Run();
