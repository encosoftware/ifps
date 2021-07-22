using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations.OrderAggregate
{
    public class PackageConfiguration : EntityTypeConfiguration<Package>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Package> builder)
        {
           
                
        }
    }
}
