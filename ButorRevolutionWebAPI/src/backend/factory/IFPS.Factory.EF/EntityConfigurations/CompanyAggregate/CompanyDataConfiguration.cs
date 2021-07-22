using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class CompanyDataConfiguration : EntityTypeConfiguration<CompanyData>
    {
        public override void ConfigureEntity(EntityTypeBuilder<CompanyData> builder)
        {
            builder.HasOne(ent => ent.Core)
                .WithMany(rev => rev.Versions)
                .HasForeignKey(ent => ent.CoreId);

            builder.OwnsOne(ent => ent.HeadOffice);

            builder.HasOne(ent => ent.ContactPerson)
                .WithMany()
                .HasForeignKey(ent => ent.ContactPersonId);
        }
    }
}
