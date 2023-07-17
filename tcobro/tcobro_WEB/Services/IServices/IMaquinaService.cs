using tcobro_WEB.Models.Dto;

namespace tcobro_WEB.Services.IServices
{
    public interface IMaquinaService
    {
        Task<T> ObtenerTodos<T>();
        Task<T> Obtener<T>(int id);
        Task<T> Crear<T>(MaquinaCreateDTO dto);
        Task<T> Actualizar<T>(MaquinaUpdateDTO dto);
        Task<T> Remover<T>(int id);
    }
}
