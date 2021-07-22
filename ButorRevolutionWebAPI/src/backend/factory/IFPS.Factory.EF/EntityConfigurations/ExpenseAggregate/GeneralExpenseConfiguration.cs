using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Factory.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Factory.EF.EntityTypeConfiguration.ExpenseAggregate
{
    public class GeneralExpenseConfiguration : EntityTypeConfiguration<GeneralExpense>
    {
        public override void ConfigureEntity(EntityTypeBuilder<GeneralExpense> builder)
        {
            builder.HasOne(entity => entity.GeneralExpenseRule)
                .WithMany()
                .HasForeignKey(entity => entity.GeneralExpenseRuleId);

            builder.OwnsOne(ent => ent.Cost)
                .HasOne(ent => ent.Currency)
                .WithMany()
                .HasForeignKey(ent => ent.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
