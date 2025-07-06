using EntityFrameworkCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using System.Text.Json.Serialization;
using static System.Environment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; //Meglio usare DTO
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Salvo il nome del DB in appsetting.json e lo recupero per creare la ConnectionString
string sqliteDatabaseName = builder.Configuration.GetConnectionString("SqliteDatabaseConnectionString");

SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData; //%LOCALAPPDATA%
string path = Environment.GetFolderPath(folder); //C:\Users\chbymn\AppData\Local\
string dbPath = Path.Combine(path, sqliteDatabaseName); //C:\Users\chbymn\AppData\Local\FootballLeague_EfCore.db

string connectionString = $"Data Source={dbPath}";

builder.Services.AddDbContext<FootballLeagueDbContext>(options =>
{
    //Connection Retry/Timeout Policies
    options.UseSqlite(connectionString, sqliteOptions =>
    {
        sqliteOptions.CommandTimeout(30);
    });

    options.UseSqlite(connectionString)
           //.UseLazyLoadingProxies() //Abilito Lazy Loading (dopo aver aggiunto a EntityFrameworkCore.Data 'Microsoft.EntityFrameworkCore.Proxies'
           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
           .LogTo(Console.WriteLine, LogLevel.Information);

    if (!builder.Environment.IsProduction())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
