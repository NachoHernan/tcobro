using System.Net;

namespace tcobro_API.Modelos
{
    //No se crea como tabla por no pasarle por el ApplicationDbContext.cs
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessages = new List<string>(); //Inicializar lista para que no de futuros problemas
        }


        public HttpStatusCode statusCode { get; set; } //Almacenador de codigo de estado que retorne el endpoint
        public bool IsExitoso { get; set; } = true;
        public List<string> ErrorMessages { get; set; }//Mensaje de error del endpoint
        public object Resultado { get; set; }
    }
}
