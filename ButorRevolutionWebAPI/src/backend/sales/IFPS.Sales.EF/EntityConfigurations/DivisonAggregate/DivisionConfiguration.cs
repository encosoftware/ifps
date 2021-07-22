
using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class DivisionConfiguration : EntityTypeConfiguration<Division>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Division> builder)
        {
            builder.Metadata.FindNavigation(nameof(Division.Claims))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata.FindNavigation(nameof(Division.Translations))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
