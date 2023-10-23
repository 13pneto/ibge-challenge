using System.Reflection;
using challenge.ibge.core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        var connectionString =
            "server=ibge-challenge.ddns.net;user=dev;password=dev;database=db_ibge_challenge;AllowLoadLocalInfile=true"; 
        
        optionsBuilder
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}