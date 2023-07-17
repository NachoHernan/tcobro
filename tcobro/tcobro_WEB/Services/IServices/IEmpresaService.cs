using tcobro_WEB.Models.Dto;

namespace tcobro_WEB.Services.IServices
{
    //Pasar el Token a cada uno de los metodos para mantener autenticacion y mostrar todo
    public interface IEmpresaService
    {
        Task<T> ObtenerTodos<T>(string token);
        Task<T> Obtener<T>(int id, string token);
        Task<T> Crear<T>(EmpresaCreateDTO dto, string token);
        Task<T> Actualizar<T>(EmpresaUpdateDTO dto, string token);
        Task<T> Remover<T>(int id, string token);
    }
}
