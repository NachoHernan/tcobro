using Microsoft.EntityFrameworkCore;
using tcobro_API.Modelos;

namespace tcobro_API.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Empresa> empresas { get; set; }
        
    }
}
