using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class UserTeamTypeConfiguration : EntityTypeConfiguration<UserTeamType>
    {
        public override void ConfigureEntity(EntityTypeBuilder<UserTeamType> builder)
        {
            builder.Metadata.FindNavigation(nameof(UserTeamType.Translations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
