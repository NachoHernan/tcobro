using System.Linq.Expressions;

namespace tcobro_API.Repositorio.IRepositorio
{
    //Interfaz Generica, de la que pueden heredar todos los modelos que agregamos => <T>
    public interface IRepositorio <T> where T : class
    {
        //Recibe la entidad de tipo T
        Task Crear(T entidad); 

        //Devuelve una lista segun la entidad enviada //Expresion para añadir funcion: e => e.Nombre.ToLower() // ? para que no sea obligatorio el filtro
        Task <List<T>> ObtenerTodos(Expression<Func<T,bool>> ? filtro = null); 

        Task<T> Obtener(Expression<Func<T,bool>> filtro = null , bool tracked = true);// Tracked para quitar error de AsNoTracking(EmpresaController)

        Task Remover(T entidad);

        Task Grabar();//Manejador del Metodo SaveChanges()

    }
}
