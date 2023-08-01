using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tcobro_API.Modelos.Dto
{
    public class UsuarioDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required(ErrorMessage = "El Nombre es requerido")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El Email es requerido")]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required(ErrorMessage = "El Password es requerido")]
        [MaxLength(255)]
        public string Password { get; set; }
        public string Rol { get; set; }

    }

}

