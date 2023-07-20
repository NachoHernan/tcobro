using tcobro_WEB.Models;

namespace tcobro_WEB.Services.IServices
{
    //Interfaz de servicio base generico centralizado necesario para conectarnos con la API para la utilizacion de todas las llamadas (GET,POST,PUT,DELETE,PATCH)
    public interface IBaseService
    {
        public APIResponse responseModel { get; set; }
        
        Task<T> SendAsync<T>(APIRequest apiRequest);
    }
}
