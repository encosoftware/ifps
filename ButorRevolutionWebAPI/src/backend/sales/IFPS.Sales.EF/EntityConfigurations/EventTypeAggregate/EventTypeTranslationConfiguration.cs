﻿
using ENCO.DDD.EntityFrameworkCore.Relational.Configuration;
using IFPS.Sales.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IFPS.Sales.EF.EntityConfigurations
{
    public class EventTypeTranslationConfiguration : EntityTypeConfiguration<EventTypeTranslation>
    {
        public override void ConfigureEntity(EntityTypeBuilder<EventTypeTranslation> builder)
        {
            builder.HasOne(ent => ent.Core)
                .WithMany(rev => rev.Translations)
                .HasForeignKey(ent => ent.CoreId);
        }
    }
}
