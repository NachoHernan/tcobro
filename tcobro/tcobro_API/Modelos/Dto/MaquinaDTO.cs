using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace tcobro_API.Modelos.Dto
{
    //Propiedades a exponer en la API para manipular
    public class MaquinaDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        [Required]
        [MaxLength(45)]
        public string NumeroDeSerie { get; set; }

        [MaxLength(45)]
        public string Descripcion { get; set; }

        public EmpresaDTO Empresa { get; set; } //Navegacion a propiedades de EmpresaDTO desde MaquinaDTO

    }
}
