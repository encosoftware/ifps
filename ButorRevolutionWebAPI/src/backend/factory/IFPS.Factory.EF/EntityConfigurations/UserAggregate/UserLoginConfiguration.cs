using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
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
