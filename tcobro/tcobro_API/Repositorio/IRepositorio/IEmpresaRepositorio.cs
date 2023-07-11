using tcobro_API.Modelos;

namespace tcobro_API.Repositorio.IRepositorio
{
    public interface IEmpresaRepositorio : IRepositorio<Empresa> //Repositorio que trabaja con entidad Empresa
    {
        //Metodo de actualizacion
        Task<Empresa> Actualizar (Empresa empresa);
    }
}
