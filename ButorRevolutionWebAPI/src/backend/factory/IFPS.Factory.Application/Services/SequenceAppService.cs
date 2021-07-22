using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class SequenceAppService : ApplicationService, ISequenceAppService
    {
        private readonly ISequenceRepository sequenceRepository;
        private readonly IFurnitureComponentRepository furnitureComponentRepository;

        public SequenceAppService(IApplicationServiceDependencyAggregate aggregate,
            ISequenceRepository sequenceRepository,
            IFurnitureComponentRepository furnitureComponentRepository
            ) : base(aggregate)
        {
            this.sequenceRepository = sequenceRepository;
            this.furnitureComponentRepository = furnitureComponentRepository;
        }

        public async Task<int> CreateDrillBySequenceAsync(Guid componentId, DrillBySequenceCreateDto dto)
        {
            var component = await furnitureComponentRepository.SingleIncludingAsync(ent => ent.Id == componentId, x => x.Sequences);
            var sequence = dto.CreateModelObject();
            await sequenceRepository.InsertAsync(sequence);

            foreach (var hole in dto.Holes)
            {
                var newHole = hole.CreateModelObject(sequence.Id);
                sequence.AddHole(newHole);
            }
            
            component.AddSequence(sequence);

            await unitOfWork.SaveChangesAsync();
            return sequence.Id;
        }

        public async Task<int> CreateRectangleBySequenceAsync(Guid componentId, RectangleBySequenceCreateDto dto)
        {
            var component = await furnitureComponentRepository.SingleIncludingAsync(ent => ent.Id == componentId, x => x.Sequences);
            var sequence = dto.CreateModelObject();

            await sequenceRepository.InsertAsync(sequence);
            component.AddSequence(sequence);

            await unitOfWork.SaveChangesAsync();
            return sequence.Id;
        }

    }
}
