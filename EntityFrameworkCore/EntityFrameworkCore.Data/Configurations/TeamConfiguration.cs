using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.Data.Configurations
{
    internal class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            //Evita duplicati e ottimizza ricerche sul campo indicizzato
            builder.HasIndex(q => q.Name).IsUnique();

            //Composite Key Configuration
            //builder.HasIndex(q => new { q.CoachId, q.LeagueId }).IsUnique();

            //Concorrency Token for SQL-Server
            //builder.Property(q => q.Version).IsRowVersion();

            //Concorrency Token for SQLite
            builder.Property(q => q.Version).IsConcurrencyToken();

            //Posso settare proprietà della tabella
            builder.Property(q => q.Name).HasMaxLength(100).IsRequired();

            //Rendo la table Teams temporale (versionata, mi tiene traccia delle modifiche fatte ai record in una HistoryTable)
            builder.ToTable("Teams", b => b.IsTemporal());

            //Many-To-Many Team-Team
            builder.HasMany(m => m.HomeMatches)
                .WithOne(q => q.HomeTeam)
                .HasForeignKey(q => q.HomeTeamID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); //non posso eliminare Team se referenziato

            //Many-To-Many Team-Team
            builder.HasMany(m => m.AwayMatches)
                .WithOne(q => q.AwayTeam)
                .HasForeignKey(q => q.AwayTeamID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); //non posso eliminare Team se referenziato

            //Seeding Data
            builder.HasData(
                            new Team
                            {
                                Id = 1,
                                Name = "Boston Celtics",
                                CreatedDate = new DateTime(2025, 6, 27),
                                LeagueId = 1,
                                CoachId = 1
                            },
                            new Team
                            {
                                Id = 2,
                                Name = "NewYork Lakers",
                                CreatedDate = new DateTime(2025, 6, 28),
                                LeagueId = 2,
                                CoachId = 2
                            },
                            new Team
                            {
                                Id = 3,
                                Name = "Chicago' Bulls",
                                CreatedDate = new DateTime(2025, 6, 29),
                                LeagueId = 3,
                                CoachId = 3
                            }
                           );
        }
    }
}
