using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class CargoStatusTypeConfiguration : EntityTypeConfiguration<CargoStatusType>
    {
        public override void ConfigureEntity(EntityTypeBuilder<CargoStatusType> builder)
        {
            builder.HasMany(ent => ent.Translations)
                .WithOne(ent => ent.Core)
                .HasForeignKey(ent => ent.CoreId);

            builder.Metadata.FindNavigation(nameof(CargoStatusType.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}