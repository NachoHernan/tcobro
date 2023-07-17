using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using tcobro_WEB.Models;

namespace tcobro_WEB.Models.Dto
{
    //Propiedades a exponer en la API para manipular

    public class MaquinaUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        [MaxLength(45)]
        public string NumeroDeSerie { get; set; }

        [MaxLength(45)]
        public string Descripcion { get; set; }



    }
}

