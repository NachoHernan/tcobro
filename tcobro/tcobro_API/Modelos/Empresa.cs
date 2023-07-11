using System.ComponentModel.DataAnnotations;

namespace tcobro_API.Modelos
{
    public class Empresa
    {
        //Propiedades del Modelo de la BBDD
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }
    }
}
 