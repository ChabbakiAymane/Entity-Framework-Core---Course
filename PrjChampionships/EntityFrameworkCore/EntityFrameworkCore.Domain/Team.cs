namespace EntityFrameworkCore.Domain;

public class Team : BaseDomainModel
{
    public string? Name { get; set; }

    //Coach Navigation Propriety One-To-One
    public virtual Coach Coach { get; set; } //Team deve avere Coach, CHILD
    public int CoachId { get; set; }

    //League Navigation Propriety Many-To-One
    public virtual League? League { get; set; }
    public int? LeagueId { get; set; }

    //Match Navigation Propriety Many-To-Many
    public virtual List<Match> HomeMatches { get; set; } = new List<Match> { }; //Per evitare eccezioni NULL
    public virtual List<Match> AwayMatches { get; set; } = new List<Match> { }; //Inizializzo a Emplty
}
