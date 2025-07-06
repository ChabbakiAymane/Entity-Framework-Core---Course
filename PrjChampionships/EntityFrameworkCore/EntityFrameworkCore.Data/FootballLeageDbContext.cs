using EntityFrameworkCore.Data.Configurations;
using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EntityFrameworkCore.Data;

public class FootballLeageDbContext : DbContext
{
    public FootballLeageDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData; //%LOCALAPPDATA%
        var path = Environment.GetFolderPath(folder); //C:\Users\chbymn\AppData\Local\
        DbPath = Path.Combine(path, "FootballLeage_EfCore.db"); //C:\Users\chbymn\AppData\Local\FootballLeague_EfCore.db
    }

    public DbSet<Team> Teams { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<League> Leagues { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<TeamsAndLeaguesView> TeamsAndLeaguesView { get; set; }
    public string DbPath { get; private set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}")
                      .UseLazyLoadingProxies() //Abilito Lazy Loading (dopo aver aggiunto a EntityFrameworkCore.Data 'Microsoft.EntityFrameworkCore.Proxies'
                      .LogTo(Console.WriteLine, LogLevel.Information)
                      .EnableSensitiveDataLogging()
                      .EnableDetailedErrors();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TeamConfiguration());
        modelBuilder.ApplyConfiguration(new LeagueConfiguration());
        modelBuilder.ApplyConfiguration(new CoachConfiguration());

        //Se ho molti file di configurazione
        //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<TeamsAndLeaguesView>().HasNoKey().ToView("vw_TeamsAndLeagues");//Per aggiungere una View senza PK
        //modelBuilder.Entity<TableClass>().HasNoKey().ToTable("tableNameDB");

        modelBuilder.HasDbFunction(typeof(FootballLeageDbContext).GetMethod(nameof(GetEarliestTeamMatch), new[] { typeof(int) }))
                    .HasName("fn_GetEarliestMatch"); //Nome della function nel Database
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