using System.Runtime.CompilerServices;
using tcobro_API.Datos;
using tcobro_API.Modelos;
using tcobro_API.Repositorio.IRepositorio;

namespace tcobro_API.Repositorio
{
    public class MaquinaRepositorio : Repositorio<Maquina>, IMaquinaRepositorio //Acepta la entidad Empresa y hereda de IEmpresaRepositorio
    {

        private readonly ApplicationDbContext _db;

        public MaquinaRepositorio(ApplicationDbContext db) : base(db) //Pasamos el db del hijo al padre
        {
            _db = db;
        }



        public async Task<Maquina> Actualizar(Maquina maquina)
        {
            //Agregar actualizacion(por ejemplo fecha de actualizacion)


            _db.Maquinas.Update(maquina);
            await _db.SaveChangesAsync();
            return maquina;
        }
    }
}
