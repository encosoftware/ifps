using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using LinqKit;

namespace IFPS.Factory.Application.Services
{
    public class WorkStationsAppService : ApplicationService, IWorkStationsAppService
    {
        private readonly IWorkStationRepository workStationRepository;
        private readonly IWorkStationCameraRepository workStationCameraRepository;
        private readonly ICameraRepository cameraRepository;
        private readonly ICFCProductionStateRepository cFCProductionStateRepository;

        public WorkStationsAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IWorkStationRepository workStationRepository,
            IWorkStationCameraRepository workStationCameraRepository,
            ICameraRepository cameraRepository,
            ICFCProductionStateRepository cFCProductionStateRepository)
            : base(aggregate)
        {
            this.workStationRepository = workStationRepository;
            this.workStationCameraRepository = workStationCameraRepository;
            this.cameraRepository = cameraRepository;
            this.cFCProductionStateRepository = cFCProductionStateRepository;
        }

        public async Task AddCamerasAsync(int workStationId, WorkStationCameraCreateDto workStationCameraCreateDto)
        {
            var workStation = await workStationRepository.GetWorkStationsWithCameras(workStationId);
            if (workStationCameraCreateDto.Start != null)
            {
                var started = workStation.WorkStationCameras.SingleOrDefault(ent => ent.CFCProductionState.State == CFCProductionStateEnum.Started);
                var productionState = await cFCProductionStateRepository.SingleAsync(ent=> ent.Id == workStationCameraCreateDto.Start.CFCProductionStateId.Value);
                var workStationCamera = new WorkStationCamera(workStationCameraCreateDto.Start.CameraId.Value, workStationId, workStationCameraCreateDto.Start.CFCProductionStateId.Value) { CFCProductionState = productionState };
                if (started != null)
                {
                    if (started != workStationCamera)
                    {
                        workStation.RemoveWorkStationCamera(started);
                        await workStationCameraRepository.DeleteAsync(workStationCamera);
                        await workStationCameraRepository.InsertAsync(workStationCamera);
                        workStation.AddWorkStationCamera(workStationCamera);
                    }
                }
                else
                {
                    await workStationCameraRepository.InsertAsync(workStationCamera);
                    workStation.AddWorkStationCamera(workStationCamera);
                }
            }
            if (workStationCameraCreateDto.Finish.CameraId != null)
            {
                var finished = workStation.WorkStationCameras.SingleOrDefault(ent => ent.CFCProductionState.State == CFCProductionStateEnum.Finished);
                var workStationCamera = new WorkStationCamera(workStationCameraCreateDto.Finish.CameraId.Value, workStationId, workStationCameraCreateDto.Finish.CFCProductionStateId.Value);
                if (finished != null)
                {
                    if (finished != workStationCamera)
                    {
                        workStation.RemoveWorkStationCamera(finished);
                        await workStationCameraRepository.DeleteAsync(workStationCamera);
                        await workStationCameraRepository.InsertAsync(workStationCamera);
                        workStation.AddWorkStationCamera(workStationCamera);
                    }
                }
                else
                {
                    await workStationCameraRepository.InsertAsync(workStationCamera);
                    workStation.AddWorkStationCamera(workStationCamera);
                }
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<int> CreateWorkStationAsync(WorkStationCreateDto createDto)
        {
            var newWorkStation = createDto.CreateModelObject();
            await workStationRepository.InsertAsync(newWorkStation);
            await unitOfWork.SaveChangesAsync();
            return newWorkStation.Id;
        }

        public async Task DeleteWorkStationAsync(int workStationId)
        {
            var workStationCameras = await workStationCameraRepository.GetAllListAsync(ent=> ent.WorkStationId == workStationId);
            workStationCameras.ForEach(ent => ent.CameraId = null);
            await workStationRepository.DeleteAsync(workStationId);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<WorkStationDetailsDto> GetWorkStationAsync(int workStationId)
        {
            var workStation = await workStationRepository.SingleIncludingAsync(ent => ent.Id == workStationId, ent => ent.WorkStationCameras);
            return new WorkStationDetailsDto(workStation);
        }

        public async Task<PagedListDto<WorkStationListDto>> GetWorkStationsAsync(WorkStationFilterDto filterDto)
        {
            Expression<Func<WorkStation, bool>> filter = (WorkStation ent) => ent.WorkStationType != null;

            if (!string.IsNullOrEmpty(filterDto.Name))
            {
                filter = filter.And(ent => ent.Name.ToLower().Contains(filterDto.Name.ToLower().Trim()));
            }
            if (filterDto.OptimalCrew != 0)
            {
                filter = filter.And(ent => ent.OptimalCrew == filterDto.OptimalCrew);
            }
            if (filterDto.MachineId.HasValue)
            {
                filter = filter.And(ent => ent.MachineId == filterDto.MachineId.Value);
            }
            if (filterDto.WorkStationTypeId.HasValue)
            {
                filter = filter.And(ent => ent.WorkStationTypeId == filterDto.WorkStationTypeId.Value);
            }
            if (!string.IsNullOrEmpty(filterDto.Status.ToString()))
            {
                filter = filter.And(ent => ent.IsActive == filterDto.Status);
            }

            var orderingQuery = filterDto.Orderings.ToOrderingExpression<WorkStation>(
                WorkStationFilterDto.GetOrderingMapping(), nameof(WorkStation.Id));

            var workStations = await workStationRepository.GetPagedWorkStationsAsync(filter, orderingQuery, filterDto.PageIndex, filterDto.PageSize);
            return workStations.ToPagedList(WorkStationListDto.FromEntity);
        }

        public async Task SetAvailabilityOfWorkStationAsync(int workStationId)
        {
            var workStation = await workStationRepository.SingleAsync(ent => ent.Id == workStationId);
            if (workStation.IsActive == true)
            {
                workStation.IsActive = false;
            }
            else
            {
                workStation.IsActive = true;
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateWorkStationAsync(int workStationId, WorkStationUpdateDto updateDto)
        {
            var workStation = await workStationRepository.SingleAsync(ent => ent.Id == workStationId);
            updateDto.UpdateModelObject(workStation);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<WorkStationsPlansListDto> GetWorkStationsByWorkloadPageAsync()
        {
            var workStations = await workStationRepository.GetAllListIncludingAsync(ent => true, ent => ent.WorkStationType, ent => ent.Plans);
            var workStationsDto = new WorkStationsPlansListDto();
            foreach (var workStation in workStations)
            {
                workStationsDto.SetProperty(workStation);
            }
            return workStationsDto;
        }

        public async Task<WorkStationCameraDetailsDto> GetCamerasAsync(int workStationId)
        {
            var workStation = await workStationRepository.GetWorkStationsWithCameras(workStationId);
            return new WorkStationCameraDetailsDto(workStation);
        }
    }
}
