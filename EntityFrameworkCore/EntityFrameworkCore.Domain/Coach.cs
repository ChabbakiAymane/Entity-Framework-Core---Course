using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCore.Domain;

public class Coach : BaseDomainModel
{
    //Aggiungo Annotation
    [MaxLength(100)]
    [Required]
    public string Name { get; set; }

    //Team Navigation Propriety One-To-One
    public virtual Team? Team { get; set; } //Coach può non avere Team, PARENT
}