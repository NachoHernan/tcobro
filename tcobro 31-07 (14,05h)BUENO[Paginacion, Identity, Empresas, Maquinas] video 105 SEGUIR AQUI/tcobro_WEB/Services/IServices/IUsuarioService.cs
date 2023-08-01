using tcobro_WEB.Models.Dto;

namespace tcobro_WEB.Services.IServices
{
    public interface IUsuarioService
    {
        //Metodos genericos a usar
        Task<T>Login<T>(LoginRequestDTO loginRequestDTO);
        Task<T> Registrar<T>(RegistroRequestDTO registroRequestDTO);
    }
}
