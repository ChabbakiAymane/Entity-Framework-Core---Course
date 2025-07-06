# Entity Framework Core - Dispensa Teorica

## Introduzione

Entity Framework Core (EF Core) è un Object-Relational Mapper (ORM) open-source sviluppato da Microsoft. Consente agli sviluppatori di lavorare con un database utilizzando oggetti .NET, eliminando gran parte del codice SQL necessario per le operazioni CRUD.

---

## Database Context

- Rappresenta un'astrazione del database in C#.
- Contiene i **modelli** (models) e le **proprietà DbSet**\*\*()\*\*, che rappresentano le tabelle.
- Si occupa della configurazione della connessione al database, solitamente nel metodo `OnConfiguring(...)` oppure tramite dependency injection.
- Permette di sovrascrivere i metodi `OnModelCreating(...)`, `OnConfiguring(...)`, e `SaveChanges(...)`.

## Connection String

- Serve a collegarsi al database.
- Può essere definita manualmente in `OnConfiguring(...)`, ma è consigliabile inserirla in `appsettings.json` (ASP.NET Core) o nel container di IoC per motivi di sicurezza e manutenibilità.

## Migrations

- Le **migration** sono classi che tracciano i cambiamenti apportati al modello del database.
- Permettono il versionamento e la gestione dello schema nel tempo.
- Ogni migration descrive la differenza tra lo stato attuale del modello e quello precedente.
- Vengono registrate nella tabella `__EFMigrationsHistory`.

## Data Models

- Classi C# che rappresentano le entità/tabelle del database.
- Le proprietà diventano colonne.
- Convenzioni:
  - `Id` o `[NomeClasse]Id` diventa automaticamente chiave primaria.
  - Le relazioni tra entità sono dedotte tramite navigational properties.

## Entity Framework Core e LINQ

- LINQ (Language Integrated Query) permette di scrivere query in C# che vengono tradotte in SQL.
  - **Query Syntax**:
    ```csharp
    var names = from name in ListName
                where name.Length > 10
                select name;
    ```
  - **Method Syntax**:
    ```csharp
    var names = ListName
                  .Where(name => name.Length > 10)
                  .Select(name => name);
    ```
- EF Core supporta diverse versioni:
  - EF 6 (2008-2013)
  - EF Core (2017 - oggi)

## Approcci di Sviluppo

- **Code First**:
  - Si scrive prima il codice C#, poi si genera il database da esso.
- **Database First**:
  - Si parte da un database esistente, poi si genera il codice (scaffolding).
  - Con `-Force` si possono rigenerare le classi.

## Programmazione Asincrona

- EF Core **non supporta operazioni parallele** sulla stessa istanza di `DbContext`.
- Ogni operazione asincrona (`async`) va attesa (`await`) prima di iniziarne un'altra.

## Query Efficienti

- Utilizzare **indici** con attenzione.
- Usare **projection** (es. `Select`) per ridurre i dati estratti.
- Limitare i risultati con `Skip()` e `Take()`.
- Sfruttare `AsNoTracking()` per letture più veloci.
- Usare query raw SQL con `FromSqlRaw()` dove necessario.
- Eseguire operazioni in **batch**.

## Tracking e Entity State

- EF Core tiene traccia dello stato delle entità:
  - **Detached**: non tracciata.
  - **Added**: nuova entità.
  - **Unchanged**: nessuna modifica.
  - **Modified**: modificata.
  - **Deleted**: marcata per eliminazione.
- `SaveChanges()` è transazionale: tutto o niente.

## Relazioni tra Entità

- **One-to-Many**:
  - Es. un autore con molti libri.
  - Il parent ha una `Collection`, il child ha una foreign key.
- **Many-to-Many**:
  - Richiede una tabella ponte.
  - Entrambe le entità hanno collections.
- **One-to-One**:
  - Relazione diretta, con navigational property e chiave esterna.

## Caricamento dei Dati Collegati (Loading Related Data)

- **Eager Loading**:
  - Carica tutto subito con `.Include()` e `.ThenInclude()`.
- **Explicit Loading**:
  - Carica i dati manualmente usando `DbContext.Entry(...).Reference(...).Load()`.
- **Lazy Loading**:
  - I dati vengono caricati on-demand accedendo alle proprietà `virtual`.
  - Richiede `Microsoft.EntityFrameworkCore.Proxies` e `UseLazyLoadingProxies()`.

## Delete Behaviors

- **Cascade**: elimina anche i figli.
- **Restrict**: blocca la cancellazione se ha figli.
- **NoAction**: comportamento di default.
- **SetNull**: imposta a null la chiave esterna.

## EF Core in ASP.NET

- Il `DbContext` è **scoped**: una istanza per richiesta.
- `ConnectionString` va in `appsettings.json` o `secrets.json`.
- Richiede configurazione nel container IoC con Dependency Injection.

## Temporal Tables

- Tabelle versionate nel tempo, con colonne `ValidFrom` e `ValidTo`.
- Permettono di vedere la cronologia delle modifiche.

## Concurrency Conflict

- **Optimistic Concurrency**:
  - Ogni record ha un **Concurrency Token**.
  - Se il token è cambiato tra lettura e scrittura, viene rilevato un conflitto.
- **Pessimistic Concurrency**:
  - Lock esplicito del record (es. tramite semafori).

## Global Query Filters

- Filtri applicati automaticamente a tutte le query LINQ per un dato tipo di entità.
- Usati ad esempio per:
  - Soft deletes
  - Multi-tenancy

---

## Setup dell'Ambiente

Consulta il `README.md` per la configurazione iniziale del progetto EF Core.

