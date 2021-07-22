using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class UserDataConfiguration : EntityTypeConfiguration<UserData>
    {
        public override void ConfigureEntity(EntityTypeBuilder<UserData> builder)
        {
            builder.HasOne(ent => ent.Core)
                .WithMany(rev => rev.Versions)
                .HasForeignKey(ent => ent.CoreId);

            builder.OwnsOne(ud => ud.ContactAddress);
            builder.Property(ent => ent.RowVersion).IsRowVersion();
        }
    }
}
