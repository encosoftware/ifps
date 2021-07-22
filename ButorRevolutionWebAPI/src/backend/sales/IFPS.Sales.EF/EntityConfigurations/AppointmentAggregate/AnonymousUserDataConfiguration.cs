
using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class AnonymousUserDataConfiguration : EntityTypeConfiguration<AnonymousUserData>
    {
        public override void ConfigureEntity(EntityTypeBuilder<AnonymousUserData> builder)
        {            
        }
    }
}
