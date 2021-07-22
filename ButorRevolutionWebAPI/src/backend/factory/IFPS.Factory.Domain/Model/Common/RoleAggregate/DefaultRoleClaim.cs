using ENCO.DDD.Domain.Events;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Domain.Model
{
    public class DefaultRoleClaim : IdentityRoleClaim<int>, IFullAudited, IEntity
    {
        private List<IDomainEvent> _domainEvents;

        /// <summary>
        /// Role entity
        /// </summary>
        public virtual Role Role { get; set; }

        /// <summary>
        /// Claim entity which belongs to role
        /// </summary>
        public virtual Claim Claim { get; set; }
        public int ClaimId { get; set; }

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

        public DefaultRoleClaim()
        {

        }

        public DefaultRoleClaim(int roleId, int claimId)
        {
            this.RoleId = roleId;
            this.ClaimId = claimId;
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
    }
}
