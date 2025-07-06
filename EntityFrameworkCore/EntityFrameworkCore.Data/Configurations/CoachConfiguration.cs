using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.Data.Configurations
{
    internal class CoachConfiguration : IEntityTypeConfiguration<Coach>
    {
        public void Configure(EntityTypeBuilder<Coach> builder)
        {
            builder.HasData(
                            new Coach { Id = 1, Name = "Karter", CreatedDate = new DateTime(2025, 6, 27) },
                            new Coach { Id = 2, Name = "Michael", CreatedDate = new DateTime(2025, 6, 28) },
                            new Coach { Id = 3, Name = "Kobe", CreatedDate = new DateTime(2025, 6, 29) }
                           );
        }
    }
}
