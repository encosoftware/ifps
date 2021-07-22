using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class UserTokenConfiguration : EntityTypeConfiguration<UserToken>
    {
        public override void ConfigureEntity(EntityTypeBuilder<UserToken> builder)
        {
            builder.HasOne(ent => ent.User)
                .WithMany(user => user.Tokens)
                .HasForeignKey(ent => ent.UserId);
        }
    }
}
