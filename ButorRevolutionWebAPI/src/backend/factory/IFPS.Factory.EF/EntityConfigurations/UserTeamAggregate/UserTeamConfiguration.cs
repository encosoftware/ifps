using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class UserTeamConfiguration : EntityTypeConfiguration<UserTeam>
    {
        public override void ConfigureEntity(EntityTypeBuilder<UserTeam> builder)
        {
            builder.HasOne(ent => ent.Company)
                .WithMany(ent => ent.UserTeams)
                .HasForeignKey(ent => ent.CompanyId);

            builder.HasOne(ent => ent.User)
                .WithMany()
                .HasForeignKey(ent => ent.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ent => ent.TechnicalUser)
                .WithMany()
                .HasForeignKey(ent => ent.TechnicalUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
