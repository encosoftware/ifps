using ENCO.DDD;
using ENCO.DDD.Domain.Events;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class Role : IdentityRole<int>, IFullAudited, IAggregateRoot, IMultiLingualEntity<RoleTranslation>
    {
        private List<IDomainEvent> _domainEvents;

        /// <summary>
        /// Category of the role
        /// </summary>
        public virtual Division Division { get; set; }
        public int DivisionId { get; set; }

        private List<DefaultRoleClaim> _defaultRoleClaims;
        private List<RoleTranslation> _translations;

        public ICollection<RoleTranslation> Translations
        {
            get
            {
                return _translations;
            }
            set
            {
                if (value == null)
                {
                    throw new IFPSDomainException($"Error setting Translations, value is null.");
                }
                _translations = value.ToList();
            }
        }

        public RoleTranslation CurrentTranslation => (RoleTranslation)Translations.GetCurrentTranslation();

        public virtual IEnumerable<DefaultRoleClaim> DefaultRoleClaims => _defaultRoleClaims.AsReadOnly();

        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// Creator of this entity.
        /// </summary>
        public virtual int? CreatorUserId { get; set; }

        /// <summary>
        /// Last modification date of this entity.
        /// </summary>
        public virtual DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// Last modifier user of this entity.
        /// </summary>
        public virtual int? LastModifierUserId { get; set; }

        /// <summary>
        /// Is this entity Deleted?
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        public virtual int? DeleterUserId { get; set; }

        /// <summary>
        /// Deletion time of this entity.
        /// </summary>
        public virtual DateTime? DeletionTime { get; set; }

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        private Role()
        {
            _translations = new List<RoleTranslation>();
            _defaultRoleClaims = new List<DefaultRoleClaim>();
        }

        public Role(string name, int divisionId) : this()
        {
            Name = name;
            DivisionId = divisionId;
            _translations = new List<RoleTranslation>();
        }

        public void AddDefaultRoleClaims(DefaultRoleClaim defaultRoleClaims)
        {
            Ensure.NotNull(defaultRoleClaims);
            _defaultRoleClaims.Add(defaultRoleClaims);
        }

        public void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents = _domainEvents ?? new List<IDomainEvent>();
            _domainEvents.Add(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        /// <summary>
        /// Checks if this entity is transient (it has not an Id).
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        public virtual bool IsTransient()
        {
            if (EqualityComparer<int>.Default.Equals(Id, default(int)))
            {
                return true;
            }

            //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
            if (Id.GetType() == typeof(int))
            {
                return Convert.ToInt32(Id) <= 0;
            }

            if (Id.GetType() == typeof(long))
            {
                return Convert.ToInt64(Id) <= 0;
            }

            return false;
        }

        public void AddTranslation(RoleTranslation translation)
        {
            Ensure.NotNull(translation);

            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding RoleTranslation: duplicate language: {translation.Language}");
            }

            _translations.Add(translation);
        }
    }
}
