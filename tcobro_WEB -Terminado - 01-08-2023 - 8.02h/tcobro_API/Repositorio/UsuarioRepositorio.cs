using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using tcobro_API.Datos;
using tcobro_API.Modelos;
using tcobro_API.Modelos.Dto;
using tcobro_API.Repositorio.IRepositorio;

namespace tcobro_API.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;
        private readonly UserManager<UsuarioAplicacion> _userManager;//Acceso a datos de usuario (Identity)
        private readonly RoleManager<IdentityRole> _roleManager;//Crea y asigna rol
        private readonly IMapper _mapper;

        public UsuarioRepositorio(ApplicationDbContext db,
                                  IConfiguration configuration,
                                  UserManager<UsuarioAplicacion> userManager,
                                  IMapper mapper,
                                  RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");//Token appsettings.json
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }


        //Verificacion de si el usuario es unico
        public bool IsUsuarioUnico(string Email)
        {
            var usuario = _db.UsuariosAplicacion.FirstOrDefault(u => u.Email.ToLower() == Email.ToLower());

            if(usuario == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var usuario = await _db.UsuariosAplicacion.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequestDTO.Email.ToLower());    
            
            //Verificacion de Password
            bool isValido = await _userManager.CheckPasswordAsync(usuario, loginRequestDTO.Password);

            if(usuario == null || isValido == false)
            {
                return new LoginResponseDTO()
                {
                    Token = "", //Retorna Token vacio
                    Usuario = null
                };
            }

            //Captura de Rol de Usuario para su verificacion
            var roles = await _userManager.GetRolesAsync(usuario);

            //Si el Usuario existe Generamos el JW Token            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //Crear Claim
                    new Claim(ClaimTypes.Name, usuario.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7), //Expira en 7 dias
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) //Encriptar contraseña
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new()
            {
                Token = tokenHandler.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDTO>(usuario)
            };

            return loginResponseDTO;
        }

        public async Task<UsuarioDTO> Registrar(RegistroRequestDTO registroRequestDTO)
        {
            UsuarioAplicacion usuario = new()
            {
                //Relacion de campos aspnetusers con registroRequestDTO
                UserName = registroRequestDTO.Email,
                Email = registroRequestDTO.Email,
                NormalizedEmail = registroRequestDTO.Email.ToUpper(),    
                Nombre = registroRequestDTO.Nombre,
            };

            //Grabacion de Usuarios en BBDD
            try
            {
                var resultado = await _userManager.CreateAsync(usuario, registroRequestDTO.Password);

                if(resultado.Succeeded)
                {
                    //Verificacion de si existe Rol en Usuario, si el Rol no existe lo crea y si ya existe no lo vuelve a crear
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        //Crear todo tipo de Rol aquí
                        await _roleManager.CreateAsync(new IdentityRole("admin"));  //Rol de Admin
                        await _roleManager.CreateAsync(new IdentityRole("cliente"));//Rol de Cliente
                    }

                    await _userManager.AddToRoleAsync(usuario, "admin");
                    
                    var usuarioAplicacion = _db.UsuariosAplicacion.FirstOrDefault(u=>u.UserName == registroRequestDTO.Email);
                    return _mapper.Map<UsuarioDTO>(usuarioAplicacion);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new UsuarioDTO();
        }
                
    }
}
