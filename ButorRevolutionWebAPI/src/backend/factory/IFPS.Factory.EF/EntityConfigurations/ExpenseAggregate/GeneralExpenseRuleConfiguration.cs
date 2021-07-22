using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityConfigurations.ExpenseAggregate
{
    public class GeneralExpensesTypeConfiguration : EntityTypeConfiguration<GeneralExpenseRule>
    {
        public override void ConfigureEntity(EntityTypeBuilder<GeneralExpenseRule> builder)
        {
            builder.OwnsOne(ent => ent.Amount);

            builder.HasOne(entity => entity.FrequencyType)
                .WithMany()
                .HasForeignKey(entity => entity.FrequencyTypeId);
        }
    }
}
