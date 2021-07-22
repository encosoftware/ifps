using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class UserRoleConfiguration : EntityTypeConfiguration<UserRole>
    {
        public override void ConfigureEntity(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasOne(ent => ent.User)
                .WithMany(user => user.Roles)
                .HasForeignKey(ent => ent.UserId);

            builder.HasOne(ent => ent.Role)
                .WithMany()
                .HasForeignKey(ent => ent.RoleId);
        }
    }
}
