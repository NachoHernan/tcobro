using tcobro_API.Modelos;
using tcobro_API.Modelos.Dto;

namespace tcobro_API.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        bool IsUsuarioUnico(string Email);
        Task<LoginResponseDTO> Login (LoginRequestDTO loginRequestDTO);
        Task<Usuario> Registrar (RegistroRequestDTO registroRequestDTO);
    }
}
