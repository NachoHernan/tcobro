using tcobro_WEB.Models.Dto;

namespace tcobro_WEB.Services.IServices
{
    public interface IEmpresaService
    {
        Task<T> ObtenerTodos<T>();
        Task<T> Obtener<T>(int id);
        Task<T> Crear<T>(EmpresaCreateDTO dto);
        Task<T> Actualizar<T>(EmpresaUpdateDTO dto);
        Task<T> Remover<T>(int id);
    }
}
