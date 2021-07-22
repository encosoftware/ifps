using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
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
