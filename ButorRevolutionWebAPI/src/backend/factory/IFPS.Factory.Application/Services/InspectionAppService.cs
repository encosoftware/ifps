using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using LinqKit;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class InspectionAppService : ApplicationService, IInspectionAppService
    {

        private readonly IInspectionRepository inspectionRepository;
        private readonly IStorageRepository storageRepository;

        public InspectionAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IInspectionRepository inspectionRepository,
            IStorageRepository storageRepository
            ) : base(aggregate)
        {
            this.inspectionRepository = inspectionRepository;
            this.storageRepository = storageRepository;
        }

        public async Task CloseInspectionReportAsync(int id)
        {
            var inspection = await inspectionRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.Report);
            inspection.Report.IsClosed = true;
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<int> CreateInspectionAsync(InspectionCreateDto inspectionCreateDto)
        {
            var inspection = inspectionCreateDto.CreateModelObject();
            inspection.UpdateUserInspections(inspectionCreateDto.DelegationIds);
            await inspectionRepository.InsertAsync(inspection);
            inspection.Report = new Report(inspectionCreateDto.ReportName);
            var storage = await storageRepository.GetStorageWithIncludesAsync(inspection.InspectedStorageId);
            foreach (var cell in storage.StorageCells)
            {
                cell.Stocks
                    .Select(ent => new StockReport(ent.Id, inspection.Report.Id))
                    .ToList()
                    .ForEach(ent => inspection.Report.AddStockReport(ent));
            }
            await unitOfWork.SaveChangesAsync();
            return inspection.Id;
        }

        public async Task DeleteInspectionAsync(int id)
        {
            await inspectionRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<InspectionDetailsDto> GetInspectionAsync(int id)
        {
            var inspection = await inspectionRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.Report, ent => ent.UserInspections);
            return new InspectionDetailsDto(inspection);
        }

        public async Task<ReportDetailsDto> GetInspectionReportAsync(int id)
        {
            var inspection = await inspectionRepository.GetInspectionReportWithIncludesAsync(id);
            return new ReportDetailsDto(inspection.Report) { Inspection = new InspectionListDto(inspection)   };
        }

        public async Task<PagedListDto<InspectionListDto>> GetInspectionsAsync(InspectionFilterDto inspectionFilterDto)
        {
            Expression<Func<Inspection, bool>> filter = (Inspection ent) => true;

            if (DateTime.MinValue != inspectionFilterDto.InspectedOn)
            {
                filter = filter.And(e => e.InspectedOn == inspectionFilterDto.InspectedOn);
            }

            if (inspectionFilterDto.StorageId != 0)
            {
                filter = filter.And(e => e.InspectedStorageId == inspectionFilterDto.StorageId);
            }

            if (!string.IsNullOrWhiteSpace(inspectionFilterDto.ReportName))
            {
                filter = filter.And(ent => ent.Report.Name.ToLower().Contains(inspectionFilterDto.ReportName.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(inspectionFilterDto.Delegation))
            {
                filter = filter.And(ent => ent.UserInspections.Any(i => i.User.CurrentVersion.Name.ToLower().Contains(inspectionFilterDto.Delegation)));
            }

            var orderingQuery = inspectionFilterDto.Orderings.ToOrderingExpression<Inspection>(
                InspectionFilterDto.GetOrderingMapping(), nameof(Inspection.Id));

            var inspections = await inspectionRepository.GetPagedListAsync(
                filter, InspectionListDto.Projection, orderingQuery, inspectionFilterDto.PageIndex, inspectionFilterDto.PageSize);

            return inspections.ToPagedList();
        }

        public async Task UpdateInspectionAsync(int id, InspectionUpdateDto inspectionUpdateDto)
        {
            var inspection = await inspectionRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.Report, ent => ent.UserInspections);
            if(inspection.InspectedStorageId != inspectionUpdateDto.StorageId)
            {
                inspection.Report.IsDeleted = true;
                inspection.Report = new Report(inspectionUpdateDto.ReportName);
                var storage = await storageRepository.GetStorageWithIncludesAsync(inspection.InspectedStorageId);
                foreach (var cell in storage.StorageCells)
                {
                    cell.Stocks
                        .Select(ent => new StockReport(ent.Id, inspection.Report.Id))
                        .ToList()
                        .ForEach(ent => inspection.Report.AddStockReport(ent));
                }
            }
            inspection = inspectionUpdateDto.UpdateModelObject(inspection);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateInspectionReportAsync(int id, ReportUpdateDto reportUpdateDto)
        {
            var inspection = await inspectionRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.Report.StockReports);
            foreach (var stockReport in inspection.Report.StockReports)
            {
                stockReport.MissingAmount = reportUpdateDto.StockReports.Single(ent=> ent.Id == stockReport.Id).MissingAmount;
            }
            await unitOfWork.SaveChangesAsync();
        }
    }
}
