using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class InspectionListDto
    {
        public int Id { get; set; }
        public DateTime InspectedOn { get; set; }
        public string StorageName { get; set; }
        public int StorageId { get; set; }
        public string ReportName { get; set; }
        public bool ReportIsClosed { get; set; }
        public List<string> DelegationNames { get; set; }

        public InspectionListDto() { }

        public InspectionListDto(Inspection inspection)
        {
            Id = inspection.Id;
            InspectedOn = inspection.InspectedOn;
            StorageName = inspection.InspectedStorage.Name;
            StorageId = inspection.InspectedStorageId;
            ReportName = inspection.Report.Name;
            ReportIsClosed = inspection.Report.IsClosed;
            DelegationNames = inspection.UserInspections.Select(ent => ent.User.CurrentVersion.Name).ToList();
        }

        public static Expression<Func<Inspection, InspectionListDto>> Projection
        {
            get
            {
                return entity => new InspectionListDto
                {
                    Id = entity.Id,
                    InspectedOn = entity.InspectedOn,
                    StorageName = entity.InspectedStorage.Name,
                    StorageId = entity.InspectedStorageId,
                    ReportName = entity.Report.Name,
                    ReportIsClosed = entity.Report.IsClosed,
                    DelegationNames = entity.UserInspections.Select(ent => ent.User.CurrentVersion.Name).ToList()
                };
            }
        }
    }
}
