using static tcobro_Utilidad.DefinicionesEstaticas;

namespace tcobro_WEB.Models
{
    public class APIRequest
    {
        public APITipo APITipo { get; set; } = APITipo.GET; //Valor inicial
        public string Url { get; set; } //Referencia a la URL que necesitamos manejar
        public object Datos { get; set; } //Manejo de datos
    }
}
