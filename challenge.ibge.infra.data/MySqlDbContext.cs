using System.Reflection;
using challenge.ibge.core.Entities;
using Microsoft.EntityFrameworkCore;

namespace challenge.ibge.infra.data;

public class MySqlDbContext : DbContext
{
    public DbSet<Locality> Localities { get; set; }
    public DbSet<User> Users { get; set; }

    public MySqlDbContext()
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseMySql("server=localhost;user=dev;password=dev;database=db_ibge_challenge;AllowLoadLocalInfile=true",
                ServerVersion.AutoDetect("server=localhost;user=dev2;password=dev2;database=db_ibge_challenge"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}