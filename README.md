# 📚 Entity Framework Core - Course  

We will learn how to write queries, update databases incrementally, roll back changes, and explore the myriad capabilities that Entity Framework Core affords us. This course is compatible with .NET 6 / .NET 7 / .NET 8.
When you finish this course, you’ll have the skills and knowledge of Entity Framework Core needed to easily interact with data and write queries for .NET Core applications.
By the end of watching this course, you'll be able to:
- Construct a data model using code-first and database-first workflows.
- Understand Entity Framework Commands for all operating systems.
- Use migrations to manage database changes.
- Apply Database validations and constraints.
- Perform CRUD operations using LINQ.
- Apply best practices with Entity Framework.
- Extending Data Contexts
- Understand how Change Tracking works.   
- Manage Database Structure using Fluent API
- Handle One-To-One, One-To-Many and Many-To-Many Relationships
- Entity Framework Core 6 New Features

[🔗 Corso Udemy](https://seacspa.udemy.com/course/entity-framework-core-a-full-tour/)

## ✅ Prerequisiti

Assicurati di avere installato:

- [.NET SDK](https://dotnet.microsoft.com/en-us/download)
- .NET 9.0 SDK
- Tool EF Core:
  ```bash
  dotnet tool install --global dotnet-ef
  dotnet tool update --global dotnet-ef
  ```

---

## 🧪 Progetto d'esempio

### 🗂️ Struttura della Solution `EntityFrameworkCore`

- **Progetti:**
  - `EntityFrameworkCore.Domain`: contiene le classi che rappresentano le tabelle del database.
  - `EntityFrameworkCore.Data`: contiene i dati e il **DbContext** del progetto.

### ➕ Aggiunta pacchetti NuGet

> Assicurati di eseguire questi comandi all'interno delle cartelle corrette dei progetti.

- Base EF Core:
  ```bash
  dotnet add package Microsoft.EntityFrameworkCore
  ```

- Provider database:
  - SQL Server:
    ```bash
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    ```
  - SQLite:
    ```bash
    dotnet add package Microsoft.EntityFrameworkCore.Sqlite
    ```

- Tools (necessari per la console di gestione delle migration):
  ```bash
  dotnet add package Microsoft.EntityFrameworkCore.Tools
  dotnet add package Microsoft.EntityFrameworkCore.Design
  ```

---

## 🧱 Migrazioni

### 🔨 Creazione e aggiornamento delle migrazioni

> Esegui questi comandi dalla cartella del progetto `EntityFrameworkCore.Console`:

```bash
dotnet ef migrations add InitialMigration --startup-project ./ --project ../EntityFrameworkCore.Data/
dotnet ef database update --startup-project ./ --project ../EntityFrameworkCore.Data/
```

### 🖥️ Con Visual Studio / Package Manager Console

```powershell
Add-Migration InitialMigration
Update-Database
```

---

## 🧪 Scaffold (Reverse Engineering)

Genera le entità e il contesto a partire da un database esistente.

### Esempio (SQL Server):

```powershell
Scaffold-DbContext 'Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FootballLeague_EfCore;Encrypt=False' Microsoft.EntityFrameworkCore.SqlServer -ContextDir ScaffoldContext -OutputDir ScaffoldModels -Force
```

### Esempio (riga di comando):

```bash
dotnet ef dbcontext scaffold "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FootballLeague_EfCore" Microsoft.EntityFrameworkCore.SqlServer --context-dir ScaffoldDbContext --output-dir ScaffoldModels --startup-project ./ --project ../EntityFrameworkCore.Data/
```

---

## 🌱 Seeding & Migrazioni Step-by-Step

```powershell
Add-Migration InitialMigration -Context FootballLeagueDbContext
Update-Database -Context FootballLeagueDbContext

# Dopo inserimento dati (seeding)
Add-Migration SeededTeams -Context FootballLeagueDbContext
Update-Database -Context FootballLeagueDbContext
```

### 🔍 Elenco migrazioni

```powershell
Get-Migration -Context FootballLeagueDbContext
dotnet ef migrations list -Context FootballLeagueDbContext
```

---

## 📝 Script SQL Migrazione

### Da Package Manager

```powershell
Script-Migration -Context FootballLeagueDbContext
Script-Migration -Context FootballLeagueDbContext -Idempotent
Script-Migration InitialMigration AddedMoreEntities -Context FootballLeagueDbContext -Idempotent
```

### Da Console

```bash
dotnet ef migrations script AddedMoreEntities -Context FootballLeagueDbContext --idempotent
dotnet ef migrations script InitialMigration AddedMoreEntities -Context FootballLeagueDbContext --idempotent
```

---

## ⏪ Rollback Migrazione

```powershell
Update-Database -Context FootballLeagueDbContext -Migration 20250630091936_AddedMoreEntities
# Per ripristinare lo stato attuale:
Update-Database -Context FootballLeagueDbContext
```

```bash
dotnet ef database update 20250630091936_AddedMoreEntities
```

---

### 🧰 EF CORE POWER TOOLS:
- Estensioni → **EF Core Power Tools** (installare da Visual Studio Marketplace)
- In Visual Studio Installer:
  - Modifica > Componenti singoli > Code Tools > DGML editor

---

## 🛠️ EntityFrameworkCore.API (Web API con Scaffold)

### Visual Studio 2022:
- Solution → Add → ASP.NET Core Web API
- In `Program.cs` e `FootballLeagueDbContext.cs`, impostare la `ConnectionString` e aggiungere il `DbContext`
- In `Controllers`:
  - Add → Controller → Common/API
  - `ModelClass` = Team
  - `DbContextClass` = FootballLeagueDbContext (da `EntityFrameworkCore.Data`)
  - `ControllerName` = TeamsController

### Visual Studio Code:
- Creare progetto Web API:
```bash
dotnet new webapi -o EntityFrameworkCore.Api
dotnet sln add EntityFrameworkCore.Api/EntityFrameworkCore.Api.csproj
```
- Aggiungere pacchetti necessari:
```bash
cd EntityFrameworkCore/EntityFrameworkCore.Api
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```
- Installare e aggiornare generatori:
```bash
dotnet tool install -g dotnet-aspnet-codegenerator
dotnet tool update -g dotnet-aspnet-codegenerator
```
- Generare controller:
```bash
dotnet aspnet-codegenerator controller -name TeamsController -async -api -m Team -dc FootballLeagueDbContext -outDir Controllers
```

## 🛠️ Extra Tools

- [📥 DB Browser for SQLite](https://sqlitebrowser.org/dl/)
