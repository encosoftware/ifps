
using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasBaseType(typeof(Document));
        }
    }
}
