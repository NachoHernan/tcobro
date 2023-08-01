using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;
using tcobro_API.Modelos;

namespace tcobro_API.Modelos
{
    public class Maquina
    {
        //Propiedades del Modelo de la BBDD
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(45)]
        public string NumeroDeSerie { get; set; }

        [MaxLength(45)]
        public string Descripcion { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        [ForeignKey("EmpresaId")] //Relacion de campo EmpresaId con tabla Empresa manteniendo nombre Empresa
        public Empresa Empresa { get; set; }

    }
}
