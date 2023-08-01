using System.Reflection.Metadata.Ecma335;
using static tcobro_Utilidad.DefinicionesEstaticas;

namespace tcobro_WEB.Models
{
    public class APIRequest
    {
        public APITipo APITipo { get; set; } = APITipo.GET; //Valor inicial
        public string Url { get; set; } //Referencia a la URL que necesitamos manejar
        public object Datos { get; set; } //Manejo de datos
        public string Token { get; set; } //Autenticacion de usuario
        public Parametros Parametros { get; set; } //Clase que engloba las propiedades de la clase Parametros
    }


    //Clase de API con las mismas propiedades creada en WEB
    public class Parametros
    {
        public int NumeroDePagina { get; set; }
        public int RegistrosPorPagina { get; set; }
    }
}
