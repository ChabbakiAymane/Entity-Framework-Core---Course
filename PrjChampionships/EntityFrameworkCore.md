# **ENTITY FRAMEWORK CORE COURSE**

## **THEORY**
- **Database Context**:
  - Astrazione della steuttura del Database in codice C#.
  - Contiene models e proprietà DbSet() che rappresentano le tabelle del Database (stesso nome delle tabelle).
  - Inizializza il collegamento con il Database (avviato durante run-time).
  - Contiene l'override dei metodi di OnCreating(...), OnConfiguring(...), OnSaveChange(...).

- **Connection Strings**:
  - usata per collegarsi al Database, può essere configurata manualemente nel OnConfiguring(...) (non consigliato per questioni di sicurezza) o in Program.cs e/o IoC Setup.

- **Migrations**:
  - sono Classi che vengono usate perchè i cambiamenti del Database sono difficili da tracciare, non si ha Versioning Control o per Backup manuali o Restore.
  - Sono file che descrivono i cambiamenti tra il currentDB e uno snapshot di un vecchio stato del Database.
  - Tramite i comandi di migrations si può generare un file dove si tiene traccia di tutte queste differenze.
  - Tutte le migrations vengono anche salvate all'interno di una historyTable all'interno del Database.

- **Entity Framework Core**
  - *ORM*, Open-Source library usata per interagire con il Database, incaplusa logicamente il Contex del Database e rende più semplice e ottimizzato l'accesso e l'uso del Database.
  - **LINQ**:
    - Language Integrated Query, permette di scrivere query (list, objects) in C# che verranno successivamente tradotte a run-time in SQL.
    - **Query Syntax**:
      ```C#
      var names = from name in ListName where name.lenght > 10 select name;
      ```
    - **Method Syntax**:
      ```C#
      var names = ListName.Select(name => ListNames.Name).Where(name => name.lenght >= 10);
      ```
  - **EF**:
    - *EF-EF6*: 2008-2013
    - *Core*: 2017
    - *Core 5/6*: 2020-2024
    - *Core 7*: 2022-2024
    - *Core 8/9*: 2024 - oggi
  - **Vantaggi di *EFC***:
    - Permette l'uso di una sintassi C# nativa (LINQ è un dialetto di C#).
    - Veloce e performante.
    - Permette **Migrations** e Tracking del Database:
      - permettendo così di tenere traccia di tutte le modifiche fatte agli schemi del Database.
    - Permette di usare diversi e più Database contemporaneamente sulla stessa instanza.
    - Ottimizzato per Testing.
  - **Data Models**:
    - Classi usate come modelli che rappresentano una tabella sul Database (proprietà diventano colonne, nome e tipo compresi).
    - Esistono delle **convenzioni** (ID diventa automaticamente la PrimaryKey della tabella).

- **Code vs Database First Development**:
  - **Code First Development**:
    - viene prima scritto il codice sorgente, successivamente viene generato il Database in base al codice.
  - **Database First Development**:
    - viene generata prima la struttura del Database (o è pre-esistente) e successivemente il codice sorgente.
    - **Scaffolding** (Reverse Engineering).
      - Database genera **Code Models** della struttura/schemi del Database (viene fatto ogni volta che il Database cambia struttura).
      - Tramite l'opzione ```-Force``` lo costringo a droppare tutto e ricreare le Classi.
  
- **Asynchronous Programming**:
  - **EF Core** non supporta *multiple parallel operations* sulla stessa istanza del **DbContext** (sulla stessa connessione).
  - Devo aspettare che l'operazione finisca prima di iniziarne un'altra (devo usare ```await``` per ogni ```async```).

- **Efficient Querying:**
  - Usare **indexes** saggiamente (molti Index velocizzano la lettura, ma rallentano la scrittura).
  - Usare **Projections**.
  - Limitare il risultato delle query (**skip** and **take**).
  - Usare **async** methods.
  - Usare **raw SQL queries** (FromSQLRaw() method).
  - Usare **no-tracking**.
  - Eseguire operazioni in **batch**.
## **SETTING UP**

## **UTILITY**

```csharp

```
