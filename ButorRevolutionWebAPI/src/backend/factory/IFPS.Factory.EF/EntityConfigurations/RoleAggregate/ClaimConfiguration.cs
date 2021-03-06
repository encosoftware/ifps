using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class ClaimConfiguration : EntityTypeConfiguration<Claim>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Claim> builder)
        {
            builder.HasOne(ent => ent.Division)
                .WithMany(module => module.Claims)
                .HasForeignKey(ent => ent.DivisionId);

            builder.HasAlternateKey(ent => ent.Name);
        }
    }
}
