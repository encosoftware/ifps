using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Sales.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Domain.Model
{
    public class Company : FullAuditedAggregateRoot, IVersionedEntity<CompanyData>
    {
        /// <summary>
        /// Company name, this property cannot change
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Company type
        /// </summary>
        public CompanyType CompanyType { get; set; }
        public int CompanyTypeId { get; set; }

        private List<CompanyDateRange> _openingHours;
        public IEnumerable<CompanyDateRange> OpeningHours => _openingHours.AsReadOnly();

        /// <summary>
        /// Historized versions of company data
        /// </summary>
        private List<CompanyData> _versions;
        public IEnumerable<CompanyData> Versions => _versions.AsReadOnly();

        public CompanyData CurrentVersion { get; set; }
        public int? CurrentVersionId { get; set; }

        /// <summary>
        /// Private list of company's teams
        /// </summary>
        private List<UserTeam> _userTeams;
        public IEnumerable<UserTeam> UserTeams => _userTeams.AsReadOnly();

        /// <summary>
        /// Default constructor is private
        /// </summary>
        private Company()
        {
            _versions = new List<CompanyData>();
            _userTeams = new List<UserTeam>();
            _openingHours = new List<CompanyDateRange>();
        }

        public Company(string name, int companyTypeId) : this()
        {
            Name = name;
            CompanyTypeId = companyTypeId;

        }

        public Company(string name, int companyTypeId, string taxNumber,
            string registerName, Address headOffice, int? contactPersonId) : this(name, companyTypeId)
        {
            _versions.Add(new CompanyData(taxNumber, registerName, headOffice, contactPersonId, Clock.Now));
        }


        /// <summary>
        /// Add new version of company data. The previous version will be closed automatically
        /// </summary>
        /// <param name="newVersion"></param>
        public void AddVersion(CompanyData newVersion)
        {
            Ensure.NotNull(newVersion);

            if (_versions.Any(ent => ent.ValidTo == null))
            {
                throw new IFPSDomainException("Exception at retrieving valid user version");
            }

            if (CurrentVersion != null)
            {
                CurrentVersion.SetValidTo(Clock.Now);
                _versions.Add(CurrentVersion);
                CurrentVersion = null;
            }

            newVersion.SetValidTo(null);
            CurrentVersion = newVersion;
        }

        /// <summary>
        /// Add new user team.
        /// </summary>
        /// <param name="userTeam"></param>
        public void AddUserTeam(UserTeam userTeam)
        {
            Ensure.NotNull(userTeam);
            _userTeams.Add(userTeam);
        }

        /// <summary>
        /// Add opening hours.
        /// </summary>
        /// <param name="companyDateRangeList"></param>
        public void AddOpeningHours(List<CompanyDateRange> companyDateRangeList)
        {
            Ensure.NotNull(companyDateRangeList);
            _openingHours.AddRange(companyDateRangeList);
        }

        public void RemoveOpeningHours(List<int> ids)
        {
            _openingHours.RemoveAll(x => ids.Contains(x.Id));
        }
    }
}
