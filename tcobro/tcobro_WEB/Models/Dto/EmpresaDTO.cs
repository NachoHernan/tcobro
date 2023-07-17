using System.ComponentModel.DataAnnotations;

namespace tcobro_WEB.Models.Dto
{
    //Propiedades a exponer en la API para manipular
    public class EmpresaDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }
    }
}
