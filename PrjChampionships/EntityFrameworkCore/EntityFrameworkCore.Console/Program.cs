using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

//Istance of Context
using var context = new FootballLeageDbContext();

//Eseguo Migration al caricamento dell'applicazione
context.Database.EnsureCreated(); //Controlla se il Context del DB esiste
//await context.Database.MigrateAsync(); //Esegue la Migration (crea DB se non esiste)

#region CALL COMMENTATE
//Execute User-Defined Query
//await UserDefinedQuery();

//Query Scalar or Non-Entity Type
//await QueryingScalar_NonEntityType();

//Non-querying statement
//await NonQueryingStatement();

//Executing Stored Procesures
//await QueryingExecutingStoredProcedures();

//Raw SQL - Mixing with LINQ
//await QueryingMixingLINQ();

//Raw SQL - FromSqlInterpolated()
//await QueryingFromSqlInterpolated();

//Raw SQL - FromSql()
//await QueryingFromSql();

//Raw SQL - FromSqlRaw()
//await QueryingFromSqlRaw();

//Raw SQL - Querying a Keyless Entity
//await QueryingKeylessEntityOrView();

//Projects and Anonymous Types
//await ProjectsAndAnonymous();

//Lazy Loading Data with Filter
//await ExplicitLoadingDataFiltering();

//Lazy Loading Data
//await LazyLoading();

//Explicit Loading Data
//await ExplicitLoading();

//Eager Loading Data
//await EagerLoading();

//Insert Parent with Children
//await AddParentWithChildMatch();

//Insert Parent/Child
//await AddParentChildMatch();

//Insert Record with FK
//await AddSimpleMatchWithFK();

//Select First Team
//await GetOneTeam();

//Select all Teams (Methods Syntax)
//await GetAllTeams();

//Select Filtered/Searched Team (Method Syntax)
//await GetFiltedNameTeam();
//await GetSearchedNameTeams();

//Select all record that meet a condition
//await GetFiltedDateTeams(new DateTime(2025, 6, 20));

//Select all Team (Query Syntax)
//await GetAllTeamsQuerySyntax();

//Select Searched Team (Query Syintax)
//await GetAllSearchedNameTeamsQuerySyntax();

//COUNT
//await CountAll();
//await CountWithCriterion();

//MAX
//await MAXTeam();

//MIN
//await MINTeam();

//AVG
//await AverageTeam();

//SUM
//await SUMTeam();

//GroupBy & Aggregating
//await GroupByTeamYear();

//Ordering
//await OrderingNameTeam();
//await OrderingMaxByTeam();

//Execute Update
//await ExecuteUpdate();

//Execute Delete
//await ExecuteDelete();

//Simple Delete operations: DELETE FROM Coaches WHERE Id = 1 
//await SimpleDelete();

//Update operation with no tracking
//await UpdateCoachAsNoTracking();

//Update operation with tracking
//await UpdateCoachAsTracking();

//Inserting multiple Data
//await BatchInsert();

//Inserting multiple Data & ChangeTracker via foreach
//await LoopInsert();

//Inserting Data
//await SimpleInsert();

//Skip and Take - Paging
//await Skip_Take();

//IQueryable vs List Types
//await IQueryableVsList();

// No traking - EF Core tracks (in memory) Objects that are returned by queries (This is less useful in disconnected applications like APIs and WebApps),
//await NoTracking();

//Select and Projections
//await Select_Projections();
#endregion

#region FUNZIONI
async Task UserDefinedQuery()
{
    int Id = 1;
    DateTime earliestMatch = context.GetEarliestTeamMatch(Id);
}
async Task QueryingScalar_NonEntityType()
{
    List<int> leagueIDs = await context.Database.SqlQuery<int>($"SELECT Id From Leagues").ToListAsync();
    foreach (int id in leagueIDs)
    {
        Console.WriteLine($"ID: {id}");
    }
}

async Task NonQueryingStatement()
{
    string someNewTeameName = "New TeamName Here";
    int successNewName = await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE Teams SET Name = {someNewTeameName}"); //Se success != 0, OK!

    var teamToDeleteId = 1;
    int successDeleteTeam = await context.Database.ExecuteSqlInterpolatedAsync($"EXEC dbo.DeleteTeam {teamToDeleteId}");
}

async Task QueryingExecutingStoredProcedures()
{
    int leagueID = 1;
    List<League> league = await context.Leagues.FromSqlInterpolated($"EXEC dbo.StoredProcedureToGetLeagueNameHere {leagueID}").ToListAsync();
}

async Task QueryingMixingLINQ()
{
    //FromSql()
    List<Team> teams = await context.Teams.FromSql($"SELECT * FROM Teams")
                                          .Where(q => q.Id == 1)
                                          .OrderBy(q => q.Id)
                                          .Include("League")
                                          .ToListAsync();
    foreach (var team in teams)
    {
        Console.WriteLine($"{team.Name}");
    }
}

async Task QueryingFromSqlInterpolated()
{
    Console.WriteLine("Enter Team name:");
    string? teamName = Console.ReadLine();
    SqliteParameter teamNameParam = new SqliteParameter("teamName", teamName);
    //List<Team> team = await context.Teams.FromSqlRaw($"SELECT * FROM Teams WHERE name = '{teamName}'").ToListAsync(); //bad practice, devo controllare input
    List<Team> teams = await context.Teams.FromSqlInterpolated($"SELECT * FROM Teams WHERE name = {teamName}").ToListAsync();
    foreach (var team in teams)
    {
        Console.WriteLine($"{team.Name}");
    }
}

async Task QueryingFromSql()
{
    Console.WriteLine("Enter Team name:");
    string? teamName = Console.ReadLine();
    SqliteParameter teamNameParam = new SqliteParameter("teamName", teamName);
    //List<Team> team = await context.Teams.FromSqlRaw($"SELECT * FROM Teams WHERE name = '{teamName}'").ToListAsync(); //bad practice, devo controllare input
    List<Team> teams = await context.Teams.FromSql($"SELECT * FROM Teams WHERE name = {teamName}").ToListAsync();
    foreach (var team in teams)
    {
        Console.WriteLine($"{team.Name}");
    }
}

async Task QueryingFromSqlRaw()
{
    Console.WriteLine("Enter Team name:");
    string? teamName = Console.ReadLine();
    SqliteParameter teamNameParam = new SqliteParameter("teamName", teamName);
    //List<Team> team = await context.Teams.FromSqlRaw($"SELECT * FROM Teams WHERE name = '{teamName}'").ToListAsync(); //bad practice, devo controllare input
    List<Team> teams = await context.Teams.FromSqlRaw($"SELECT * FROM Teams WHERE name = @teamName", teamNameParam).ToListAsync();
    foreach (var team in teams)
    {
        Console.WriteLine($"{team.Name}");
    }
}

async Task QueryingKeylessEntityOrView()
{
    List<TeamsAndLeaguesView> teamsView = await context.TeamsAndLeaguesView.ToListAsync();
    foreach (var team in teamsView)
    {
        Console.WriteLine($"{team.Name} - {team.LeagueName}");
    }
}

async Task ProjectsAndAnonymous()
{
    //Creo classe TeamDetail che andrà a contenere i dati che mi servono da Team e Coach
    List<TeamDetail> teamsDetail = await context.Teams.Select(q => new TeamDetail
    {
        TeamId = q.Id,
        TeamName = q.Name,
        CoachName = q.Coach.Name,
        TotalHomeGoals = q.HomeMatches.Sum(score => score.HomeTeamScore),
        TotalAwayGoals = q.AwayMatches.Sum(score => score.AwayTeamScore)
    }).ToListAsync();

    foreach (TeamDetail teamD in teamsDetail)
    {
        Console.WriteLine($"{teamD.TeamName} - {teamD.CoachName} | HG: {teamD.TotalHomeGoals} | AG: {teamD.TotalAwayGoals}");
    }
}

async Task ExplicitLoadingDataFiltering()
{
    //Get all teams and only homeMatches where they have scored
    await InsertMoreMatches();
    List<Team> teams = await context.Teams
                                          //Non va bene, se Team non ha goal, null-exception
                                          //Non restituisce Teams che ha fatto goal giocando in casa, restituisce Teams che hanno fatto goal
                                          //.Where(q => q.HomeMatches.First().HomeTeamScore > 0) 
                                          .Include("Coach")
                                          .Include(q => q.HomeMatches.Where(q => q.HomeTeamScore > 0))
                                          .ToListAsync();
    foreach (Team team in teams)
    {
        Console.WriteLine($"{team.Name} - {team.Coach.Name}");
        foreach (Match match in team.HomeMatches)
        {
            Console.WriteLine($"Score: {match.HomeTeamScore}");
        }
        Console.WriteLine("---------------------");
    }
}

async Task InsertMoreMatches()
{
    Match match1 = new Match
    {
        AwayTeamID = 2,
        HomeTeamID = 3,
        HomeTeamScore = 1,
        AwayTeamScore = 0,
        MatchDate = new DateTime(2023, 01, 1),
        TicketPrice = 20
    };
    Match match2 = new Match
    {
        AwayTeamID = 2,
        HomeTeamID = 1,
        HomeTeamScore = 1,
        AwayTeamScore = 0,
        MatchDate = new DateTime(2023, 1, 1),
        TicketPrice = 20
    };
    Match match3 = new Match
    {
        AwayTeamID = 1,
        HomeTeamID = 3,
        HomeTeamScore = 1,
        AwayTeamScore = 0,
        MatchDate = new DateTime(2023, 1, 1),
        TicketPrice = 20
    };
    Match match4 = new Match
    {
        AwayTeamID = 4,
        HomeTeamID = 3,
        HomeTeamScore = 0,
        AwayTeamScore = 1,
        MatchDate = new DateTime(2023, 1, 1),
        TicketPrice = 20
    };

    await context.AddRangeAsync(match1, match2, match3, match4);
    await context.SaveChangesAsync();
}

async Task LazyLoading()
{
    int IdLeague = 1;
    League? league = await context.FindAsync<League>(IdLeague);
    foreach (Team team in league.Teams)
    {
        Console.WriteLine($"{team.Name}");
    }

    foreach (League leagueLazy in context.Leagues)
    {
        foreach (Team teamLazy in leagueLazy.Teams)
        {
            //1. Lazy per Leagues - 2. Lazy per Team - 3. Lazy per Coach (4 query quando me ne bastava 1)
            Console.WriteLine($"{teamLazy.Name} - {teamLazy.Coach.Name}");
        }
    }
}

async Task ExplicitLoading()
{
    int IdLeague = 1;
    League? league = await context.FindAsync<League>(IdLeague);

    if (!league.Teams.Any())
    {
        Console.WriteLine("Teams have not been loaded!");
    }

    await context.Entry(league).Collection(q => q.Teams).LoadAsync();
    if (league.Teams.Any())
    {
        Console.WriteLine("Teams have been loaded!");
        foreach (Team team in league.Teams)
        {
            Console.WriteLine($"{team.Name}");
        }
    }
}

async Task EagerLoading()
{
    List<League> leagues = await context.Leagues.ToListAsync();

    foreach (League league in leagues)
    {
        Console.WriteLine(league.Name);
        foreach (Team team in league.Teams)
        {
            Console.WriteLine(team.Name); //Non stampa i nomi dei Team perchè non è inclusa Team nella query
        }
    }

    //List<League> leaguesWithTeams = await context.Leagues.Include("Teams").ThenInclude("Coach").ToListAsync(); //Uso nome dato alla Property Navigation nell'Include() e ThenInclude()
    List<League> leaguesWithTeams = await context.Leagues.Include(q => q.Teams).ThenInclude(q => q.Coach).ToListAsync(); //Include()/ThenInclude() accetta Lambda expression 

    foreach (League league in leagues)
    {
        Console.WriteLine($"League - {league.Name}");
        foreach (Team team in league.Teams)
        {
            Console.WriteLine($"{team.Name} - {team.Coach.Name}");
        }
    }
}

async Task AddParentWithChildMatch()
{
    League newLeague = new League
    {
        Name = "New League",
        Teams = new List<Team>
        {
            new Team
            {
                Name = "Juventus",
                Coach = new Coach
                {
                    Name = "Juve Coach"
                }
            },
            new Team
            {
                Name = "AC Milan",
                Coach = new Coach
                {
                    Name = "Milan Coach"
                }
            },
            new Team
            {
                Name = "AS Roma",
                Coach = new Coach
                {
                    Name = "Roma Coach"
                }
            }
        }
    };
    await context.AddAsync(newLeague);
    await context.SaveChangesAsync();
}

async Task AddParentChildMatch()
{
    //Coach newCoach = new Coach
    //{
    //    Name = "Johnson",
    //};

    Team newTeam = new Team
    {
        Name = "New Team",
        //Coach = newCoach,
        Coach = new Coach
        {
            Name = "Johnson",
        }
    };

    //Automaticamente crea Coach, ottiene ID e lo usa per crearea Team
    await context.AddAsync(newTeam);
    await context.SaveChangesAsync();
}

async Task AddSimpleMatchWithFK()
{
    Match NewMatch = new Match
    {
        AwayTeamID = 1,
        HomeTeamID = 2,
        HomeTeamScore = 0,
        AwayTeamScore = 0,
        MatchDate = new DateTime(2023, 10, 1),
        TicketPrice = 20
    };

    await context.AddAsync(NewMatch);
    await context.SaveChangesAsync();
}

async Task ExecuteUpdate()
{
    await context.Coaches.Where(c => c.CreatedDate > new DateTime(2024, 12, 1))
                         .ExecuteUpdateAsync(set => set
                                                       .SetProperty(prop => prop.Name, "Updated Name")
                                                       .SetProperty(prop => prop.CreatedDate, DateTime.Now)
                                            );
}

async Task ExecuteDelete()
{
    // List<Coach> coaches = await context.Coaches.Where(c => c.CreatedDate > new Data(2024, 12, 1)).ToListAsync();
    // context.RemoveRange(coaches);
    // await context.SaveChangesAsync();
    await context.Coaches.Where(c => c.CreatedDate > new DateTime(2024, 12, 1)).ExecuteDeleteAsync();
}

async Task SimpleDelete()
{
    int IdRemove = 1;
    Coach? coachToRemove = await context.Coaches.FindAsync(IdRemove);
    context.Remove(coachToRemove);
    //Setto Entity ID=1 come 'removed' così verrà eliminata successivamente dal DB
    //context.Entry(coachToRemove).State = EntityState.Removed;
    List<Coach> coachesToRemove = await context.Coaches.Where(c => c.CreatedDate > new DateTime(2024, 12, 1)).ToListAsync();
    context.RemoveRange(coachesToRemove);
    foreach (Coach coachToFlag in coachesToRemove)
    {
        context.Entry(coachToFlag).State = EntityState.Deleted;
    }
    await context.SaveChangesAsync();
}

async Task UpdateCoachAsNoTracking()
{
    int IDcoach = 9;
    //Find() funziona solo con AsTracking()
    Coach? coach9 = context.Coaches.AsNoTracking().FirstOrDefault(coach => coach.Id == IDcoach);
    coach9.CreatedDate = DateTime.Now;
    //Devo specificare che ho aggiornato un entity, altrimenti non viene vista la modifica
    Console.WriteLine(context.ChangeTracker.DebugView.LongView);
    //await context.Update(coach9);
    //Posso cambiare lo stato dell'Entity a mano
    context.Entry(coach9).State = EntityState.Modified;
    Console.WriteLine(context.ChangeTracker.DebugView.LongView);
    //Ora vedo la modifica fatta all'entity
    await context.SaveChangesAsync();
    Console.WriteLine(context.ChangeTracker.DebugView.LongView);
}

async Task UpdateCoachAsTracking()
{
    int IDcoach = 9;
    //Find() prima verifica se entity che cerco è in memoria (tracked), se non la trova (no tracked), cerca nel DB
    Coach? coach = await context.Coaches.FindAsync(IDcoach);
    coach.CreatedDate = DateTime.Now; //Aggiorno l'Entity
    await context.SaveChangesAsync(); //Automaticamente aggiorna il record sul DB
}

async Task BatchInsert()
{
    List<Coach> coaches = new List<Coach> { new Coach{ Name = "Pippo", CreatedDate = new DateTime(2025, 6, 30) },
                                            new Coach{ Name = "Pluto", CreatedDate = new DateTime(2025, 6, 29) },
                                            new Coach{ Name = "Mario", CreatedDate = new DateTime(2025, 6, 28) },
                                            new Coach{ Name = "Luigi", CreatedDate = new DateTime(2025, 6, 27) }
                                          };
    await context.AddRangeAsync(coaches);
    await context.SaveChangesAsync();
}

async Task LoopInsert()
{
    List<Coach> coaches = new List<Coach> { new Coach{ Name = "Pippo", CreatedDate = new DateTime(2025, 6, 30) },
                                            new Coach{ Name = "Pluto", CreatedDate = new DateTime(2025, 6, 29) },
                                            new Coach{ Name = "Mario", CreatedDate = new DateTime(2025, 6, 28) },
                                            new Coach{ Name = "Luigi", CreatedDate = new DateTime(2025, 6, 27) }
                                           };

    foreach (Coach coach in coaches)
    {
        //Aggiungo al Tracking le Entities
        await context.Coaches.AddAsync(coach);
        //Ogni singolo elemento viene salvato, se uno falisce viene generata eccezione (gestita)
        //await context.SaveChangesAsync();
    }

    Console.WriteLine(context.ChangeTracker.DebugView.LongView);
    //Se SaveChangesAsync() di 49/50 Entity falisce, context subisce un rollback (si perde tutto)
    await context.SaveChangesAsync();
    Console.WriteLine(context.ChangeTracker.DebugView.LongView);
    //Una volta aggiunta Entity, viene restituita indietro come Unchanged e posso usarla (anche ID)
    foreach (Coach coach in coaches)
    {
        Console.WriteLine($"[{coach.Id}] - {coach.Name}");
    }
}

async Task SimpleInsert()
{
    Coach newCoach = new Coach()
    {
        Name = "Karter",
        CreatedDate = new DateTime(2025, 6, 30)
    };
    //Aggiunge entity al tracking (stile git)
    await context.Coaches.AddAsync(newCoach);
    //Restituisce ID dell'entity aggiunta 
    await context.SaveChangesAsync();
}

async Task IQueryableVsList()
{
    Console.WriteLine("Enter '1' for Team with Id=1 or '2' for teams that contains 'N'");
    int option = Convert.ToInt32(Console.ReadLine());
    List<Team> teamAsList = new List<Team>();

    // After executing ToListAsync(), the record are loaded in into memory. Any operations is then done in memory.
    teamAsList = await context.Teams.ToListAsync();
    if (option == 1)
        teamAsList = teamAsList.Where(t => t.Id == 1).ToList();
    else if (option == 2)
        teamAsList = teamAsList.Where(t => t.Name.Contains("N")).ToList();

    foreach (Team team in teamAsList)
    {
        Console.WriteLine(team.Name);
    }

    // Records stay as IQueryable until the ToListAsync() is executed, then the final query is performed.
    IQueryable<Team> teamsAsQueryable = context.Teams.AsQueryable();
    if (option == 1)
        teamsAsQueryable = teamsAsQueryable.Where(t => t.Id == 1);
    else if (option == 2)
        teamsAsQueryable = teamsAsQueryable.Where(t => t.Name.Contains("N"));

    //Actual Query execution
    teamAsList = await teamsAsQueryable.ToListAsync();
    foreach (Team team in teamAsList)
    {
        Console.WriteLine(team.Name);
    }
}

async Task NoTracking()
{
    //Tramite AsNoTracking() spengo il "tracciamento" dei dati, non mantiene in memoria lo stato dei dati
    List<Team> teams = await context.Teams.AsNoTracking().ToListAsync();
    foreach (Team team in teams)
    {
        Console.WriteLine(team.Name);
    }
    // posso farlo globalmente andando su OnConfiguring(){ optionsBuilder.UseSqlite(...).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).LogTo(...)...  }
}

async Task Select_Projections()
{
    //Restituisce una Lista di Team
    List<Team> teamsTeam = await context.Teams.ToListAsync();
    foreach (Team team in teamsTeam)
    {
        Console.WriteLine(team.Name);
    }
    //Selezionando solo il nome, restituisce una Lista di String (nomi dei team)
    List<string?> teamNames = await context.Teams.Select(t => t.Name).ToListAsync();
    foreach (string name in teamNames)
    {
        Console.WriteLine(name);
    }
    //Restituisce un tipo di dato anonimo che contiene i due campi proiettati dal Database
    var teamNamesDate = await context.Teams.Select(team => new { team.Name, team.CreatedDate }).ToListAsync();
    foreach (var team in teamNamesDate)
    {
        Console.WriteLine($"{team.Name} - [Date]: {team.CreatedDate}");
    }
    //Costruisco Classe TeamInfo (messa sotto) dove andare a salvare il tipo di dato anonomi restituito
    var teamInfoNamesDate = await context.Teams.Select(team => new { Name = team.Name, CreatedDate = team.CreatedDate }).ToListAsync();
    foreach (var team in teamInfoNamesDate)
    {
        Console.WriteLine($"{team.Name} - [Date]: {team.CreatedDate}");
    }
}

async Task Skip_Take()
{
    int recordCount = 3;
    int page = 0;
    bool continueLoop = true;

    while (continueLoop)
    {
        //Tramite Skip(page * recordCount) gli dico di saltare gli elementi già letti
        List<Team> teams = await context.Teams.Skip(page * recordCount).Take(recordCount).ToListAsync();
        foreach (Team team in teams)
        {
            Console.WriteLine(team.Name);
        }
        Console.WriteLine("Enter [TRUE] for the next set of records, [FALSE] to exit.");
        continueLoop = Convert.ToBoolean(Console.ReadLine());
        if (!continueLoop) break;
    }
}

async Task OrderingMaxByTeam()
{
    Team? maxSingleTeam = context.Teams.MaxBy(t => t.Id); //Restituisce un solo elemento (il max) non è asynch
    Team? minSingleTeam = context.Teams.MinBy(t => t.Id); //Restituisce un solo elemento (il min) non è asynch
    //Team maxByDescendingOrder = await context.Teams.OrderByDescending(t => t.TeamId).FirstOrDefaultAsync();
    //Team minByAscendingOrder = await context.Teams.OrderBy(t => t.TeamId).FirstOrDefaultAsync();
    Console.WriteLine($"MAX: {maxSingleTeam}");
    Console.WriteLine($"MIN: {minSingleTeam}");
}

async Task OrderingNameTeam()
{
    List<Team> ascOrderedTeams = await context.Teams.OrderBy(t => t.Name).ToListAsync();
    Console.WriteLine("Ascending Order:");
    foreach (Team item in ascOrderedTeams)
    {
        Console.WriteLine(item.Name);
    }

    List<Team> desOrderedTeams = await context.Teams.OrderByDescending(t => t.Name).ToListAsync();
    Console.WriteLine("Descending Order:");
    foreach (Team item in desOrderedTeams)
    {
        Console.WriteLine(item.Name);
    }
}

async Task GroupByTeamYear()
{
    //Istanziando con new, mi restituisce un oggetto anonimo, devo usare dynamic
    //List<IGrouping<dynamic, Team>> groupedTeams = await context.Teams.GroupBy(t => new { t.CreatedDate.Year }).ToListAsync();

    List<IGrouping<int, Team>> groupedTeams = await context.Teams
                                                           //.Where() //WHERE clause
                                                           .GroupBy(t => t.CreatedDate.Year)
                                                           //.Where() //HAVING clause
                                                           .ToListAsync();
    foreach (IGrouping<int, Team> group in groupedTeams)
    {
        Console.WriteLine($"Anno: {group.Key}");
        Console.WriteLine($"SUM: {group.Sum(q => q.Id)}");
        foreach (Team team in group)
        {
            Console.WriteLine($" - {team.Name}");
        }
    }
}

async Task MAXTeam()
{
    int maxTeams = await context.Teams.MaxAsync(q => q.Id);
    Console.WriteLine($"MAX: {maxTeams}");
}

async Task MINTeam()
{
    int minTeams = await context.Teams.MinAsync(q => q.Id);
    Console.WriteLine($"MIN: {minTeams}");
}

async Task AverageTeam()
{
    double avgTeams = await context.Teams.AverageAsync(q => q.Id);
    Console.WriteLine($"AVG: {avgTeams}");
}

async Task SUMTeam()
{
    int sumTeams = await context.Teams.SumAsync(q => q.Id);
    Console.WriteLine($"SUM: {sumTeams}");
}

async Task CountWithCriterion()
{
    Console.Write("[Enter Criteria to search]: ");
    string? searchedTeam = Console.ReadLine();

    var numberOfTeamsWithCondition = await context.Teams.CountAsync(t => EF.Functions.Like(t.Name, $"%{searchedTeam}%"));
    Console.WriteLine($"Number of Teams with Criterion [{searchedTeam}]: {numberOfTeamsWithCondition}");
}

async Task CountAll()
{
    var numberOfTeams = await context.Teams.CountAsync();
    Console.WriteLine($"Number of Teams: {numberOfTeams}");
}

async Task GetAllSearchedNameTeamsQuerySyntax()
{
    Console.Write("[Enter Team to search]: ");
    string? searchedTeam = Console.ReadLine();

    //IQueryable<Team> teamsIQueryble = from team in context.Teams select team;
    List<Team> teamsList = await (from team in context.Teams
                                  where EF.Functions.Like(team.Name, $"%{searchedTeam}%")
                                  select team
                                 ).ToListAsync();

    foreach (var team in teamsList)
    {
        Console.WriteLine(team.Name);
    }
}

async Task GetAllTeamsQuerySyntax()
{
    //IQueryable<Team> teamsIQueryble = from team in context.Teams select team;
    List<Team> teamsList = await (from team in context.Teams
                                  select team
                                 ).ToListAsync();

    foreach (var team in teamsList)
    {
        Console.WriteLine(team.Name);
    }
}

async Task GetSearchedNameTeams()
{
    Console.Write("[Enter Team to search]: ");
    string? searchedTeam = Console.ReadLine();
    if (searchedTeam != null)
    {
        //SELECT * FROM Teams WHERE Name LIKE '%...%'
        List<Team> partialMatches = await context.Teams.Where(team => EF.Functions.Like(team.Name, $"%{searchedTeam}%")).ToListAsync();
        //List<Team> partialMatches = await context.Teams.Where(t => t.Name.Contains(searchedTeam)).ToListAsync();
        foreach (Team team in partialMatches)
        {
            Console.WriteLine($"Found: [{team.Name}]");
        }
    }
    else
    {
        Console.WriteLine("Error!");
    }
}

async Task GetFiltedDateTeams(DateTime dateToFilter)
{
    List<Team> teamsFiltered = await context.Teams.Where(filteredTeam => filteredTeam.CreatedDate >= dateToFilter).ToListAsync(); //Query eseguita solo al ToList()/ToListAsync()
    //IQueryable<Team> teamsFiltered2 = context.Teams.Where(filteredTeam => filteredTeam.CreatedDate > dateToFilter); //Invece che List posso usare IQueryable
    Console.WriteLine($"Registered Teams [{dateToFilter.ToShortDateString()}]: ");
    foreach (Team t in teamsFiltered)
    {
        Console.WriteLine(t.Name);
    }
    Console.WriteLine();
}

async Task GetFiltedNameTeam()
{
    Console.Write("[Enter Desired Team]: ");
    string? desiredTeam = Console.ReadLine();
    if (desiredTeam != null)
    {
        List<Team> teamsFiltered = await context.Teams.Where(filteredTeam => filteredTeam.Name == desiredTeam).ToListAsync();
        foreach (Team team in teamsFiltered)
        {
            Console.WriteLine($"Desired Team: [{team.Name}]");
        }
        Console.WriteLine();
    }
    else
    {
        Console.WriteLine("Error!");
    }
}

async Task GetAllTeams()
{
    //SELECT * FROM Team
    List<Team> teamsRecord = await context.Teams.ToListAsync();
    Console.WriteLine("\n\t------------------\n\t SQUADRE AMMESSE");
    foreach (var record in teamsRecord)
    {
        Console.WriteLine($"\t------------------\n\t{record.Id}[{record.Name}]");
    }
    Console.WriteLine("\t------------------");
}

async Task GetOneTeam()
{
    //Selecting a single record - First one in the list
    Team teamFirst = await context.Teams.FirstAsync();
    if (teamFirst != null)
    {
        Console.WriteLine("\t--------------------");
        Console.WriteLine($"\t{teamFirst.Id}: [{teamFirst.Name}]");
        Console.WriteLine("\t--------------------");
    }
}

async Task Esempi()
{
    // Il context esiste solo all'interno dello scope, come sopra insomma
    using (var context = new FootballLeageDbContext())
    {
        await GetAllTeams();
        await GetOneTeam();
        await GetFiltedNameTeam();
        await GetFiltedDateTeams(new DateTime(2025, 6, 20));
        await GetSearchedNameTeams();
        await GetAllTeamsQuerySyntax();
        await GetAllSearchedNameTeamsQuerySyntax();
        await CountAll();
        await CountWithCriterion();
        await MAXTeam();
        await MINTeam();
        await AverageTeam();
        await SUMTeam();
        await GroupByTeamYear();
        await OrderingNameTeam();
        await OrderingMaxByTeam();
        await Skip_Take();
        await Select_Projections();
        await NoTracking();
        await IQueryableVsList();
        await SimpleInsert();
        await LoopInsert();
        await BatchInsert();
        await UpdateCoachAsTracking();
        await UpdateCoachAsNoTracking();
        await SimpleDelete();
        await ExecuteDelete();
        await ExecuteUpdate();
        await AddSimpleMatchWithFK();
        await AddParentChildMatch();
        await AddParentWithChildMatch();
        await EagerLoading();
        await ExplicitLoading();
        await LazyLoading();
        await ExplicitLoadingDataFiltering();
        await ProjectsAndAnonymous();
        await QueryingKeylessEntityOrView();
        await QueryingFromSqlRaw();
        await QueryingFromSql();
        await QueryingFromSqlInterpolated();
        await QueryingMixingLINQ();
        await QueryingExecutingStoredProcedures();
        await NonQueryingStatement();
        await QueryingScalar_NonEntityType();
        await UserDefinedQuery();
        await Esempi();

        // Codice...
    }

    //Select a single Record - First one in the list
    Team teamFirst = await context.Teams.FirstAsync(); //FirstOrDefaultAsync()
    Console.WriteLine("\t------------------\n\t [" + teamFirst.Name + "]\n\t------------------");

    Team teamFirstNull = await context.Teams.FirstOrDefaultAsync();
    if (teamFirstNull != null)
        Console.WriteLine("\t------------------\n\t [" + teamFirst.Name + "]\n\t------------------");

    //Select a single Record - First one in the list that meets a conditions
    Team teamSelectID = await context.Teams.FirstAsync(single => single.Id == 2);
    Console.WriteLine("\t------------------\n\t [" + teamSelectID.Name + "]\n\t------------------");

    // Select a single Record in table with only 1 element
    Team singleTeam = await context.Teams.SingleAsync(team => team.Id == 3);
    Console.WriteLine("\t------------------\n\t [" + singleTeam.Name + "]\n\t------------------");

    //Selecting based on ID
    Team? teamBasedId = await context.Teams.FindAsync(2);
    if (teamBasedId != null)
        Console.WriteLine(teamBasedId.Name);
}
#endregion

//Classe simile a DTO dove salvare i dati restituiti dal Database
class TeamInfo
{
    public int TeamId { get; set; }
    public int Name { get; set; }
    public int CreatedDate { get; set; }
}

//Classe DTO per salvare i dettagli a cui sono interessato
class TeamDetail
{
    public int TeamId { get; set; }
    public string TeamName { get; set; }
    public string CoachName { get; set; }

    public int TotalHomeGoals { get; set; }
    public int TotalAwayGoals { get; set; }
}