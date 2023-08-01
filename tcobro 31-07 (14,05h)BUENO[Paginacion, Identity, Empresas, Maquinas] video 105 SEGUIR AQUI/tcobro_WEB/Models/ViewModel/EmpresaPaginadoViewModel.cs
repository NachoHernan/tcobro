using tcobro_WEB.Models.Dto;

namespace tcobro_WEB.Models.ViewModel
{
    public class EmpresaPaginadoViewModel
    {
        public int NumeroDePagina { get; set; }
        public int PaginasTotales { get; set; }
        public string Anterior { get; set; } = "disabled"; //Deshabilita el boton si no existen paginas anteriores
        public string Siguiente { get; set; } = ""; //Cambia de valores segun la pagina en la que nos encontremos
        public IEnumerable<EmpresaDTO> EmpresaList { get; set; }

    }
}
