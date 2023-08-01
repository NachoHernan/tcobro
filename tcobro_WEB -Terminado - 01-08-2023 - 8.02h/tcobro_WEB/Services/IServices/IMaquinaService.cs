using tcobro_WEB.Models.Dto;

namespace tcobro_WEB.Services.IServices
{
    public interface IMaquinaService
    {
        //Pasar el Token a cada uno de los metodos para mantener autenticacion y mostrar todo
        Task<T> ObtenerTodos<T>(string token);
        Task<T> Obtener<T>(int id, string token);
        Task<T> Crear<T>(MaquinaCreateDTO dto, string token);
        Task<T> Actualizar<T>(MaquinaUpdateDTO dto, string token);
        Task<T> Remover<T>(int id, string token);
    }
}
