using Microsoft.AspNetCore.Identity;

namespace tcobro_API.Modelos
{
    public class UsuarioAplicacion : IdentityUser
    {
        public string Nombre { get; set; }
        public string Email { get; set; }

    }
}
