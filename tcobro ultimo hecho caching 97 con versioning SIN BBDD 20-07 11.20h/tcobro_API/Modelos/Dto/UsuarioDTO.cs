using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tcobro_API.Modelos.Dto
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El Email es requerido")]

        public string Email { get; set; }
        [Required(ErrorMessage = "El Password es requerido")]

        public string Password { get; set; }
        [Required(ErrorMessage = "El Nombre es requerido")]

        public string Nombre { get; set; }
        public string Rol { get; set; }
    }

}

