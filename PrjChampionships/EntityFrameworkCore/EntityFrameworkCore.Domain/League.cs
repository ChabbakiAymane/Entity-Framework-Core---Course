namespace EntityFrameworkCore.Domain;

public class League : BaseDomainModel
{
    public string Name { get; set; }

    //Team Navigation Proprieties Many-To-Many
    public virtual List<Team> Teams { get; set; } = new List<Team>();
}
