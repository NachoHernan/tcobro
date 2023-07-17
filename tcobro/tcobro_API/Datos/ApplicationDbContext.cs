using Microsoft.EntityFrameworkCore;
using tcobro_API.Modelos;

namespace tcobro_API.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //Ejecutar migracion en consola de administrador de paquetes -> ProyectoAPI
        //Creaccion de tablas en la BBDD
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Maquina> Maquinas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        //Habilita modificacion de BBDD por EntityFramework
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
