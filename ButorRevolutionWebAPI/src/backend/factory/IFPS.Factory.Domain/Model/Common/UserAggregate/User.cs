using ENCO.DDD;
using ENCO.DDD.Domain.Events;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class User : IdentityUser<int>, IVersionedEntity<UserData>, IFullAudited, IAggregateRoot
    {
        private List<IDomainEvent> _domainEvents;

        public bool IsActive { get; set; }

        public int? CurrentVersionId { get; set; }
        public virtual UserData CurrentVersion { get; set; }

        /// <summary>
        /// Bool flag. True, if the account is technical. False, otherwise
        /// </summary>
        public bool IsTechnicalAccount { get; set; }

        public Company Company { get; set; }
        public int? CompanyId { get; set; }

        /// <summary>
        /// Default application, website and notification language
        /// </summary>
        public LanguageTypeEnum Language { get; set; }

        /// <summary>
        /// Picture
        /// </summary>
        public Image Image { get; set; }

        public Guid? ImageId { get; set; }

        /// <summary>
        /// Historized versions of company data
        /// </summary>
        private List<UserData> _versions;

        /// <summary>
        /// Public readonly collection of company historized versions
        /// </summary>
        public IEnumerable<UserData> Versions => _versions.AsReadOnly();

        private List<UserClaim> _claims;
        public IEnumerable<UserClaim> Claims => _claims.AsReadOnly();

        private List<UserRole> _roles;
        public IEnumerable<UserRole> Roles => _roles.AsReadOnly();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        private List<UserLogin> _logins;
        public IEnumerable<UserLogin> Logins => _logins.AsReadOnly();

        private List<UserToken> _tokens;
        public IEnumerable<UserToken> Tokens => _tokens.AsReadOnly();

        private List<Email> _emails;
        public IEnumerable<Email> Emails => _emails.AsReadOnly();

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

        protected User()
        {
            _versions = new List<UserData>();
            _claims = new List<UserClaim>();
            _roles = new List<UserRole>();
            _logins = new List<UserLogin>();
            _tokens = new List<UserToken>();
            _emails = new List<Email>();
        }

        public User(string emailAddress, LanguageTypeEnum preferredLanguage = LanguageTypeEnum.HU, bool isActive = true)
            : this()
        {
            this.Language = preferredLanguage;
            this.Email = emailAddress;
            this.IsActive = isActive;
            this.CreationTime = Clock.Now.Date;
        }

        public void AddVersion(UserData newVersion)
        {
            Ensure.NotNull(newVersion);

            if (_versions.Any(ent => ent.ValidTo == null))
            {
                throw new IFPSDomainException("Wrong UserData is archieved");
            }

            if (this.CurrentVersion != null)
            {
                CurrentVersion.ValidTo = Clock.Now;
                this._versions.Add(CurrentVersion);
                this.CurrentVersion = null;
            }

            newVersion.SetValidTo(null);
            //UserName = CurrentVersion?.Name; // ?
            //PhoneNumber = CurrentVersion?.Phone; // ?
            this.CurrentVersion = newVersion;
        }

        public void AddRoles(ICollection<int> rolesIds)
        {
            _roles.AddRange(
                rolesIds.Where(rId => !_roles.Any(r => r.RoleId == rId && !r.IsDeleted))
                    .Select(r => new UserRole(this.Id, r)
           ));
        }

        public void AddRoles(List<Role> roles)
        {
            _roles.AddRange(roles.Select(ent => new UserRole(Id, ent.Id) { Role = ent }).ToList());
        }

        public void AddEmail(Email email)
        {
            Ensure.NotNull(email);
            _emails.Add(email);
        }

        public void RemoveRoles(ICollection<int> rolesIds)
        {
            foreach (var role in _roles.Where(ur => rolesIds.Any(rId => rId == ur.RoleId && !ur.IsDeleted)))
            {
                role.IsDeleted = true;
                role.DeletionTime = Clock.Now;
            }
        }

        public void AddClaims(ICollection<int> claimsIds)
        {
            _claims.AddRange(
                claimsIds.Where(cId => !_claims.Any(uc => uc.ClaimId == cId && !uc.IsDeleted))
                    .Select(cId => new UserClaim(this.Id, cId)
            ));
        }

        public void RemoveClaims(ICollection<int> claimsIds)
        {
            foreach (var claim in _claims.Where(c => claimsIds.Any(cId => cId == c.ClaimId && !c.IsDeleted)))
            {
                claim.IsDeleted = true;
                claim.DeletionTime = Clock.Now;
            }
        }

        public void AddToken(UserToken userToken)
        {
            _tokens.Add(userToken);
        }

        public void RemoveTokens()
        {
            foreach (var token in _tokens.Where(c => !c.IsDeleted))
            {
                token.IsDeleted = true;
                token.DeletionTime = Clock.Now;
            }
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