namespace EntityFrameworkCore.Domain;

public class Match : BaseDomainModel
{
    //Team Navigation Proprieties Many-To-One
    public virtual Team HomeTeam { get; set; }
    public int HomeTeamID { get; set; }

    //Team Navigation Proprieties Many-To-One
    public virtual Team AwayTeam { get; set; }
    public int AwayTeamID { get; set; }

    public int HomeTeamScore { get; set; }
    public int AwayTeamScore { get; set; }
    public decimal TicketPrice { get; set; }
    public DateTime MatchDate { get; set; }
}