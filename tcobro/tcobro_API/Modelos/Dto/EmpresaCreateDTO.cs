using System.ComponentModel.DataAnnotations;

namespace tcobro_API.Modelos.Dto
{
    //Propiedades a exponer en la API para manipular

    public class EmpresaCreateDTO
    {
        
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }
    }
}
