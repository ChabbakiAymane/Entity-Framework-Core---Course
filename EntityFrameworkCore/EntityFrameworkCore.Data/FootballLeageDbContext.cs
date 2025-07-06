using EntityFrameworkCore.Data.Configurations;
using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static System.Environment;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EntityFrameworkCore.Data;

public class FootballLeagueDbContext : DbContext
{
    //public FootballLeageDbContext()
    //{
    //    var folder = Environment.SpecialFolder.LocalApplicationData; //%LOCALAPPDATA%
    //    var path = Environment.GetFolderPath(folder); //C:\Users\chbymn\AppData\Local\
    //    DbPath = Path.Combine(path, "FootballLeage_EfCore.db"); //C:\Users\chbymn\AppData\Local\FootballLeague_EfCore.db
    //}

    //WebApp
    public FootballLeagueDbContext(DbContextOptions<FootballLeagueDbContext> options) : base(options)
    {

    }

    public DbSet<Team> Teams { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<League> Leagues { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<TeamsAndLeaguesView> TeamsAndLeaguesView { get; set; }
    public string DbPath { get; private set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    //SQL Server
    //    //optionsBuilder.UseSqlServer($"Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=FootballLeague_EfCore; Encrypt=False");
    //    optionsBuilder.UseSqlite($"Data Source={DbPath}")
    //                  .UseLazyLoadingProxies() //Abilito Lazy Loading (dopo aver aggiunto a EntityFrameworkCore.Data 'Microsoft.EntityFrameworkCore.Proxies'
    //                                           //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
    //                  .LogTo(Console.WriteLine, LogLevel.Information)
    //                  .EnableSensitiveDataLogging()
    //                  .EnableDetailedErrors();
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TeamConfiguration());
        modelBuilder.ApplyConfiguration(new LeagueConfiguration());
        modelBuilder.ApplyConfiguration(new CoachConfiguration());

        //Se ho molti file di configurazione
        //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<TeamsAndLeaguesView>().HasNoKey().ToView("vw_TeamsAndLeagues");//Per aggiungere una View senza PK
        //modelBuilder.Entity<TableClass>().HasNoKey().ToTable("tableNameDB");

        modelBuilder.HasDbFunction(typeof(FootballLeagueDbContext).GetMethod(nameof(GetEarliestTeamMatch), new[] { typeof(int) }))
                    .HasName("fn_GetEarliestMatch"); //Nome della function nel Database
    }

    //Mi permette di settare delle configurazioni globali (tipo di dato, ecc)
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        //Configuro tutte le stringhe per avere delle caratteristiche specifiche
        configurationBuilder.Properties<string>().HaveMaxLength(100);
        configurationBuilder.Properties<decimal>().HavePrecision(16, 2);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseDomainModel>().Where(q => q.State == EntityState.Modified || q.State == EntityState.Added);
        foreach (var entry in entries)
        {
            entry.Entity.ModifiedDate = DateTime.UtcNow;
            entry.Entity.ModifiedBy = "Sample User 1";

            if(entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
                entry.Entity.CreatedBy = "Sample User";
            }

            entry.Entity.Version = Guid.NewGuid();
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    public DateTime GetEarliestTeamMatch(int teamID) => throw new NotImplementedException(); //Placeholder, posso dichiararlo dove voglio
    /*
        CREATE FUNCTION[dbo].[GetEarliestMatch] (@teamId int)
	        RETURNS datetime
            BEGIN
                DECLARE @result datetime
                SELECT TOP 1 @result = date
                FROM MATCHES[dbo].[Matches]
                ORDER BY Date
                RETURN @result
            END
        END
    */
}

public class FootballLeagueDbContextFactory : IDesignTimeDbContextFactory<FootballLeagueDbContext>
{
    public FootballLeagueDbContext CreateDbContext(string[] args)
    {
        SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        string path = Environment.GetFolderPath(folder);

        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                     .AddJsonFile("appsettings.json")
                                                                     //.AddJsonFile("appsettings2.json")
                                                                     .Build();

        string dbPath = Path.Combine(path, configuration.GetConnectionString("SqliteDatabaseConnectionString"));

        DbContextOptionsBuilder<FootballLeagueDbContext> optionsBuilder = new DbContextOptionsBuilder<FootballLeagueDbContext>();
        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        return new FootballLeagueDbContext(optionsBuilder.Options);
    }
}