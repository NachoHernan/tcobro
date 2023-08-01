using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace tcobro_WEB.Models.Dto
{
    //Propiedades a exponer en la API para manipular

    public class MaquinaCreateDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Id de la empresa es requerido")]
        public int EmpresaId { get; set; }

        [Required(ErrorMessage = "El Numero De Serie es requerido")]
        [MaxLength(45)]
        public string NumeroDeSerie { get; set; }

        [MaxLength(45)]
        public string Descripcion { get; set; }



    }
}
