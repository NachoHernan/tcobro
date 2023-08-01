using System.ComponentModel.DataAnnotations;

namespace tcobro_WEB.Models.Dto
{
    public class UsuarioDTO
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "El Email es requerido")]

        public string Email { get; set; }
        [Required(ErrorMessage = "El Nombre es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El Password es requerido")]

        public string Password { get; set; }

    }
}
