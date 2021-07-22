using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class UserLoginConfiguration : EntityTypeConfiguration<UserLogin>
    {
        public override void ConfigureEntity(EntityTypeBuilder<UserLogin> builder)
        {
            builder.HasOne(ent => ent.User)
                .WithMany(user => user.Logins)
                .HasForeignKey(ent => ent.UserId);
        }
    }
}
