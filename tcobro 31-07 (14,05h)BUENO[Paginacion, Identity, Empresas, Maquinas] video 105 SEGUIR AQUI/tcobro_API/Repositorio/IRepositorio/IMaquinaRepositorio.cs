using tcobro_API.Modelos;

namespace tcobro_API.Repositorio.IRepositorio
{
    public interface IMaquinaRepositorio : IRepositorio<Maquina> //Repositorio que trabaja con entidad Empresa
    {
        //Metodo de actualizacion
        Task<Maquina> Actualizar (Maquina maquina);
    }
}
