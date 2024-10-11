using Microsoft.EntityFrameworkCore;
using WebAppTurnos.Models;

namespace WebAppTurnos.Context
{
    public class AppDbContext_context: DbContext
    {
        public AppDbContext_context(DbContextOptions<AppDbContext_context> options):base(options)
        {
                    
        }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }


    }
}
