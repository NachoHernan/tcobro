using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using tcobro_API.Modelos;

namespace tcobro_API.Datos
{
    public class ApplicationDbContext : IdentityDbContext<UsuarioAplicacion>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //Ejecutar migracion en consola de administrador de paquetes -> ProyectoAPI
        //Creaccion de tablas en la BBDD
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Maquina> Maquinas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioAplicacion> UsuariosAplicacion { get; set; }


        //Habilita modificacion de BBDD por EntityFramework
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Relaciones sin problemas de PrimaryKey
            base.OnModelCreating(modelBuilder);
        }

    }
}
