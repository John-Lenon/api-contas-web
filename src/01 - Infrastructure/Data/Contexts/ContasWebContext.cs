using Domain.Entities.Cobranca;
using Domain.Entities.Usuarios;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts
{
    public class ContasWebContext : IdentityDbContext
    {
        public DbSet<Conta> Conta { get; set; }
        public DbSet<RegraDiaAtraso> RegraDiaAtraso { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        public ContasWebContext(DbContextOptions<ContasWebContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContasWebContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
