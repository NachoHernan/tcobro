using System.ComponentModel.DataAnnotations;

namespace tcobro_API.Modelos.Dto
{
    //Propiedades a exponer en la API para manipular
    public class EmpresaDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El Nombre es requerido")]
        [MaxLength(100)]
        public string Nombre { get; set; }
    }
}
