namespace EntityFrameworkCore.Domain;

public class League : BaseDomainModel
{
    public string Name { get; set; }

    //Soft-deleting, il campo viene flaggato come "delete" ma non viene effettivamente eliminato
    public bool isDeleted { get; set; } 

    //Team Navigation Proprieties Many-To-Many
    public virtual List<Team> Teams { get; set; } = new List<Team>();
}
