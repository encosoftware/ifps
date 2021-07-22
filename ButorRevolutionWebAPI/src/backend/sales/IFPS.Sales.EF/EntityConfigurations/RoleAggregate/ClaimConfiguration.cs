using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
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
