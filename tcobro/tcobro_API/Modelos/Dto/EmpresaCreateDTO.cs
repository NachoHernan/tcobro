using System.ComponentModel.DataAnnotations;

namespace tcobro_API.Modelos.Dto
{
    //Propiedades a exponer en la API
    public class EmpresaCreateDTO
    {
        [Required]
        [MaxLength(30)]
        public string Nombre { get; set; }
    }
}
