using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class CompanyConfiguration : EntityTypeConfiguration<Company>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Company> builder)
        {
            builder.HasOne(ent => ent.CompanyType)
                .WithMany()
                .HasForeignKey(ent => ent.CompanyTypeId);

            builder.HasOne(ent => ent.CurrentVersion)
                .WithMany()
                .HasForeignKey(ent => ent.CurrentVersionId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(ent => ent.Versions).WithOne(rev => rev.Core).HasForeignKey(rev => rev.CoreId);

            builder.Metadata.FindNavigation(nameof(Company.Versions)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Company.OpeningHours)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Company.UserTeams)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
