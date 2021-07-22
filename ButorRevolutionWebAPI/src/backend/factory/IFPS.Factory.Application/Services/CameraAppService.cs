using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class CameraAppService : ApplicationService, ICameraAppService
    {
        private readonly IWorkStationCameraRepository workStationCameraRepository;
        private readonly ICameraRepository cameraRepository;

        public CameraAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IWorkStationCameraRepository workStationCameraRepository,
            ICameraRepository cameraRepository
            ) : base(aggregate)
        {
            this.workStationCameraRepository = workStationCameraRepository;
            this.cameraRepository = cameraRepository;
        }

        public async Task<int> CreateCameraAsync(CameraCreateDto cameraCreateDto)
        {
            var camera = cameraCreateDto.CreateModelObject();
            await cameraRepository.InsertAsync(camera);
            await unitOfWork.SaveChangesAsync();

            return camera.Id;
        }

        public async Task DeleteCameraAsync(int id)
        {
            await cameraRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<CameraDetailsDto> GetCameraAsync(int id)
        {
            var camera = await cameraRepository.SingleAsync(ent => ent.Id == id);
            return new CameraDetailsDto(camera);
        }

        public async Task<List<CameraNameListDto>> GetCameraNameListAsync(int workStationId)
        {
            var usedCameraIds = await workStationCameraRepository.GetAllListAsync(ent => ent.WorkStationId != workStationId, ent => ent.CameraId);
            var cameras = await cameraRepository.GetAllListAsync(ent => !usedCameraIds.Contains(ent.Id));
            return cameras.Select(ent => new CameraNameListDto(ent)).ToList();
        }

        public async Task<PagedListDto<CameraListDto>> GetCamerasAsync(CameraFilterDto cameraFilterDto)
        {
            Expression<Func<Camera, bool>> filter = (Camera ent) => true;

            if (!string.IsNullOrWhiteSpace(cameraFilterDto.Name))
            {
                filter = filter.And(e => e.Name.ToLower().Contains(cameraFilterDto.Name.ToLower().Trim()));
            }

            if (!string.IsNullOrWhiteSpace(cameraFilterDto.Type))
            {
                filter = filter.And(e => e.Type.ToLower().Contains(cameraFilterDto.Type.ToLower().Trim()));
            }

            if (!string.IsNullOrWhiteSpace(cameraFilterDto.IPAddress))
            {
                filter = filter.And(e => e.IPAddress.ToLower().Contains(cameraFilterDto.IPAddress.ToLower().Trim()));
            }

            var orderingQuery = cameraFilterDto.Orderings.ToOrderingExpression<Camera>(
                CameraFilterDto.GetOrderingMapping(), nameof(Camera.Id));

            var cameras = await cameraRepository.GetPagedListAsync(
                filter, CameraListDto.Projection, orderingQuery, cameraFilterDto.PageIndex, cameraFilterDto.PageSize);

            return cameras.ToPagedList();
        }

        public async Task UpdateCameraAsync(int id, CameraUpdateDto cameraUpdateDto)
        {
            var camera = await cameraRepository.SingleAsync(ent => ent.Id == id);
            camera = cameraUpdateDto.UpdateModelObject(camera);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
