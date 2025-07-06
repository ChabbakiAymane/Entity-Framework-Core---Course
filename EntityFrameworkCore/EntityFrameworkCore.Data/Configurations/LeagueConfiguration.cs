using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.Data.Configurations
{
    internal class LeagueConfiguration : IEntityTypeConfiguration<League>
    {
        public void Configure(EntityTypeBuilder<League> builder)
        {
            //Ogni volta che query League, viene eseguito questo filtro
            builder.HasQueryFilter(x => x.isDeleted == false);

            builder.HasData(
                            new League { Id = 1, Name = "Serie A" },
                            new League { Id = 2, Name = "Serie B" },
                            new League { Id = 3, Name = "Serie C" }
                           );
        }
    }
}
