using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations
{
    class ArcConfiguration : IEntityTypeConfiguration<Arc>
    {
        public void Configure(EntityTypeBuilder<Arc> builder)
        {
            builder.HasBaseType(typeof(Command));
        }
    }
}
