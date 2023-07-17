using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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

        public UsuarioRepositorio(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");//Token appsettings.json
        }



        public bool IsUsuarioUnico(string Email)
        {
            var usuario = _db.Usuarios.FirstOrDefault(u => u.Email.ToLower() == Email.ToLower());

            if(usuario == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequestDTO.Email.ToLower() &&
                                                                      u.Password == loginRequestDTO.Password);    
            
            if(usuario == null)
            {
                return new LoginResponseDTO()
                {
                    Token = "", //Retorna Token vacio
                    Usuario = null
                };
            }

            //Si el Usuario existe Generamos el JW Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //Crear Claim
                    new Claim(ClaimTypes.Name, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Rol)
                }),
                Expires = DateTime.UtcNow.AddDays(7), //Expira en 7 dias
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) //Encriptar contraseña
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new()
            {
                Token = tokenHandler.WriteToken(token),
                Usuario = usuario
            };

            return loginResponseDTO;
        }

        public async Task<Usuario> Registrar(RegistroRequestDTO registroRequestDTO)
        {
            Usuario usuario = new()
            {
                Email = registroRequestDTO.Email,
                Password = registroRequestDTO.Password,
                Nombre = registroRequestDTO.Nombre,
                Rol = registroRequestDTO.Rol
            };

            await _db.Usuarios.AddAsync(usuario);
            await _db.SaveChangesAsync();

            usuario.Password = ""; //Para que no se transmita el Password

            return usuario;
        }
    }
}
