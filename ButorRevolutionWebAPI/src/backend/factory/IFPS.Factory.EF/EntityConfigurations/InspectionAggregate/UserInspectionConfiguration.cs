using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class UserInspectionConfiguration : EntityTypeConfiguration<UserInspection>
    {
        public override void ConfigureEntity(EntityTypeBuilder<UserInspection> builder)
        {
            builder.HasOne(ent => ent.User)
                .WithMany()
                .HasForeignKey(ent => ent.UserId);

            builder.HasOne(ent => ent.Inspection)
                .WithMany(ent => ent.UserInspections)
                .HasForeignKey(ent => ent.InspectionId);
        }
    }
}
