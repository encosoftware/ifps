using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public override void ConfigureEntity(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(ent => ent.Company)
                .WithMany()
                .HasForeignKey(ent => ent.CompanyId);

            builder.HasOne(ent => ent.CurrentVersion)
                .WithMany()
                .HasForeignKey(ent => ent.CurrentVersionId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(ent => ent.Emails)
                .WithOne(ent => ent.User)
                .HasForeignKey(ent => ent.UserId);

            builder.HasMany(ent => ent.Versions).WithOne(rev => rev.Core).HasForeignKey(rev => rev.CoreId);

            builder.HasOne(ent => ent.Image).WithMany().HasForeignKey(ent => ent.ImageId);


            builder.Metadata.FindNavigation(nameof(User.Roles)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(User.Claims)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(User.Versions)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(User.Tokens)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(User.Logins)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(User.Emails)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
