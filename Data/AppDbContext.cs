using DesafioFSBR.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioFSBR.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Processo> Processos { get; set; }
    }
}
