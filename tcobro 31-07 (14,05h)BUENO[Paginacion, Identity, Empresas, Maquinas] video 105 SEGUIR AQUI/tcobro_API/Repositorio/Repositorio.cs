using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using tcobro_API.Datos;
using tcobro_API.Modelos.Especificaciones;
using tcobro_API.Repositorio.IRepositorio;

namespace tcobro_API.Repositorio
{
    //Repositorio Generico, Metodos que pueden heredar todos los modelos que agregamos => <T> de la que su implementacion sea la interfaz
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet; //Variable interna de tipo DbSet

        
        public Repositorio(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>(); //Hace la conversion del <T> recibido en una entidad
        }





        public async Task Crear(T entidad)//Agrega un nuevo registro de la entidad Empresa
        {
            await dbSet.AddAsync(entidad); 
            await Grabar(); //Guarda con SaveChanges()
        }

        public async Task Grabar()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<T> Obtener(Expression<Func<T, bool>> filtro = null, bool tracked = true, string? incluirPropiedades = null)
        {
            IQueryable<T> query = dbSet; //Variable de tipo T para poder hacer consultas

            if (!tracked) //Si tracked es diferente de true
            {
                query = query.AsNoTracking();
            }
            if(filtro != null)
            {
                query = query.Where(filtro);
            }

            if(incluirPropiedades != null)
            {
                //Bucle que separa cada propiedad al encontrarse con una coma y elimina espacios vacios
                foreach (var incluirProp in incluirPropiedades.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);// Incluye en sus datos los datos del modelo Empresa
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null, string? incluirPropiedades = null)
        {
            IQueryable<T> query = dbSet; //Variable de tipo T para poder hacer consultas

            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            if (incluirPropiedades != null)
            {
                //Bucle que separa cada propiedad al encontrarse con una coma y elimina espacios vacios
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);// Incluye en sus datos los datos del modelo Empresa
                }
            }

            return await query.ToListAsync();
        }

        public PagedList<T> ObtenerTodosPaginado(Parametros parametros, Expression<Func<T, bool>>? filtro = null, string? incluirPropiedades = null)
        {
            IQueryable<T> query = dbSet; //Variable de tipo T para poder hacer consultas

            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            if (incluirPropiedades != null)
            {
                //Bucle que separa cada propiedad al encontrarse con una coma y elimina espacios vacios
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);// Incluye en sus datos los datos del modelo Empresa
                }
            }
            return PagedList<T>.ToPagedList(query, parametros.NumeroDePagina, parametros.RegistrosPorPagina);
        }

        public async Task Remover(T entidad)
        {
            dbSet.Remove(entidad);
            await Grabar();
        }

        //Update separado porque cada entidad tiene diferentes propiedades
    }
}
