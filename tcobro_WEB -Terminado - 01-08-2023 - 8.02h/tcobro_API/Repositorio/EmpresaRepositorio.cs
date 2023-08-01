using System.Runtime.CompilerServices;
using tcobro_API.Datos;
using tcobro_API.Modelos;
using tcobro_API.Repositorio.IRepositorio;

namespace tcobro_API.Repositorio
{
    public class EmpresaRepositorio : Repositorio<Empresa>, IEmpresaRepositorio //Acepta la entidad Empresa y hereda de IEmpresaRepositorio
    {

        private readonly ApplicationDbContext _db;

        public EmpresaRepositorio(ApplicationDbContext db) : base(db) //Pasamos el db del hijo al padre
        {
            _db = db;
        }



        public async Task<Empresa> Actualizar(Empresa empresa)
        {
            //Agregar actualizacion(por ejemplo fecha de actualizacion)


            _db.Empresas.Update(empresa);
            await _db.SaveChangesAsync();
            return empresa;
        }
    }
}
