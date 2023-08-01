using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using tcobro_API.Modelos;

namespace tcobro_API.Modelos.Dto
{
    //Propiedades a exponer en la API para manipular

    public class MaquinaUpdateDTO
    {
        [Required(ErrorMessage = "El Id es requerido")]

        public int Id { get; set; }

        [Required(ErrorMessage = "El Id de Empresa es requerido")]

        public int EmpresaId { get; set; }
        [Required(ErrorMessage = "El Numero de Serie es requerido")]

        [MaxLength(45)]
        public string NumeroDeSerie { get; set; }

        [MaxLength(45)]
        public string Descripcion { get; set; }



    }
}

