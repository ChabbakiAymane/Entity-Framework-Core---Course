using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EntityFrameworkCore.Data;

public class FootballLeageSqlServerDbContext : DbContext
{
    public FootballLeageSqlServerDbContext() { }

    public DbSet<Team> Teams { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<League> Leagues { get; set; }
    public DbSet<Match> Matches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //SQL Server
        optionsBuilder.UseSqlServer($"Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=FootballLeague_EfCore; Encrypt=False", sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 5,
                                            maxRetryDelay: TimeSpan.FromSeconds(5),
                                            errorNumbersToAdd: null);
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}