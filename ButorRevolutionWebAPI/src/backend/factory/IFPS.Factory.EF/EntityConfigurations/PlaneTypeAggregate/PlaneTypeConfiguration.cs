using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    public class PlaneTypeConfiguration : EntityTypeConfiguration<PlaneType>
    {
        public override void ConfigureEntity(EntityTypeBuilder<PlaneType> builder)
        {

        }
    }
}
