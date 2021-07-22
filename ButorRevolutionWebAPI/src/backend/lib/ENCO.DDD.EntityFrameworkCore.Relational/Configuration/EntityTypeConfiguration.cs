using ENCO.DDD.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ENCO.DDD.EntityFrameworkCore.Relational.Configuration
{
    public class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntity
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            if (typeof(TEntity).GetInterface(nameof(ISoftDelete)) != null)
            {
                // build the 'x => !x.IsDeleted' expression
                var param = Expression.Parameter(typeof(TEntity), "x");
                var member = Expression.Property(param, nameof(ISoftDelete.IsDeleted));
                var body = Expression.Not(member);
                var expr = Expression.Lambda(body, param);

                builder.HasQueryFilter(expr);
            }

            builder.Ignore(ent => ent.DomainEvents);
            ConfigureEntity(builder);
        }

        public virtual void ConfigureEntity(EntityTypeBuilder<TEntity> builder) { }
    }
}
