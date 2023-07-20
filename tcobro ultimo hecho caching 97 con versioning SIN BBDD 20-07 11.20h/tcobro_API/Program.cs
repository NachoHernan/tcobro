using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

//Personalizacion de tiempo de guardado de cache para no consultar la BBDD 
builder.Services.AddControllers(option =>
{
    option.CacheProfiles.Add("Default30",
        new CacheProfile()
        {
            Duration = 30
        });
}).AddNewtonsoftJson();//Paquete instalado
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
    //Documentacion de Swagger, instrucciones de versiones de API
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "tcobro v1",
        Description = "API para Empresas"
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "tcobro v2",
        Description = "API para Empresas"
    });
});
//Caching para personalizar las peticiones sin tener que pedirlas reiteradamente a la BBDD
builder.Services.AddResponseCaching();

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

//Identidad de roles y usuarios
builder.Services.AddIdentity<UsuarioAplicacion, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

//Servicio de Automapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

//Agrega interfaz con su implementacion
    //Scoped: se crea una vez por solicitud y luego se destruye
    //Singleton: se crea la primera vez que se solicita y cada solicitud posterior utiliza la misma instancia
    //Transient: se crea cada vez que se solicita (servicios livianos y sin estado)
builder.Services.AddScoped<IEmpresaRepositorio, EmpresaRepositorio>();
builder.Services.AddScoped<IMaquinaRepositorio, MaquinaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true; //Coge la version por defecto
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;//Muestra en los Headers de la API la version soportada
});

//Configuracion de versiones de API
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";//Formato que acepta la Version
    options.SubstituteApiVersionInUrl = true;//Reemplaza la URL con el numero de version manejada
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    //Manejo de versiones soportadas
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json","tcobroV1");//Ruta donde se encuentra la documentacion de version de API,nombre de version
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "tcobroV2");//Ruta donde se encuentra la documentacion de version de API,nombre de version
    });
    app.UseSwaggerUI();
}
//Importante el orden de agregacion de servicios
app.UseHttpsRedirection();

app.UseAuthentication();//Authentication siempre antes que Authorization

app.UseAuthorization();

app.MapControllers();

app.Run();
