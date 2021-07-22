using System;
using System.Linq;
using System.Threading.Tasks;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;

namespace IFPS.Factory.Application.Services
{
    public class ProductionProcessAppService : ApplicationService, IProductionProcessAppService
    {
        private readonly IProductionProcessRepository productionProcessRepository;
        private readonly IConcreteFurnitureUnitRepository concreteFurnitureUnitRepository;
        private readonly IOrderRepository orderRepository;

        public ProductionProcessAppService(IApplicationServiceDependencyAggregate aggregate, 
            IProductionProcessRepository productionProcessRepository,
            IConcreteFurnitureUnitRepository concreteFurnitureUnitRepository,
            IOrderRepository orderRepository) : base(aggregate)
        {
            this.productionProcessRepository = productionProcessRepository;
            this.concreteFurnitureUnitRepository = concreteFurnitureUnitRepository;
            this.orderRepository = orderRepository;
        }

        public async Task SetProcessStatusAsync(int id, int userId)
        {
            var productionProcess = await productionProcessRepository.SingleIncludingAsync(ent => ent.Id == id, ent => ent.Plan.ConcreteFurnitureComponent, 
                ent => ent.Plan.ConcreteFurnitureUnit, ent => ent.Plan.WorkStation.WorkStationType);

            if (!productionProcess.StartTime.HasValue)
            {
                productionProcess.StartTime = DateTime.Now;

                if (productionProcess.Plan.WorkStation.WorkStationType.StationType == WorkStationTypeEnum.Layout) 
                {
                    await SetUnderProductionStatus(productionProcess.OrderId, userId);
                }
            }
            else
            {
                productionProcess.EndTime = DateTime.Now;
                await SetPlanStatus(productionProcess, userId);
            }

            await unitOfWork.SaveChangesAsync();
        }

        private async Task SetPlanStatus(ProductionProcess productionProcess, int userId)
        {
            if (productionProcess.EndTime.HasValue)
            {
                var type = productionProcess.Plan.WorkStation.WorkStationType.StationType;

                switch (type)
                {
                    case WorkStationTypeEnum.Layout:
                        (productionProcess.Plan as LayoutPlan).Status = LayoutPlanStatusEnum.Layout;
                        break;
                    case WorkStationTypeEnum.Cnc:
                        productionProcess.Plan.ConcreteFurnitureComponent.Status = CfcStatusEnum.Cnc;
                        break;
                    case WorkStationTypeEnum.Edging:
                        productionProcess.Plan.ConcreteFurnitureComponent.Status = CfcStatusEnum.Edging;
                        break;
                    case WorkStationTypeEnum.Assembly:
                        productionProcess.Plan.ConcreteFurnitureUnit.Status = CfuStatusEnum.Assembly;
                        break;
                    case WorkStationTypeEnum.Sorting:
                        productionProcess.Plan.ConcreteFurnitureUnit.Status = CfuStatusEnum.Sorting;
                        break;
                    case WorkStationTypeEnum.Packing:
                        productionProcess.Plan.ConcreteFurnitureUnit.Status = CfuStatusEnum.Packing;
                        await SetOrderStatus(productionProcess.OrderId, userId);
                        break;
                }
            }
        }

        private async Task SetOrderStatus(Guid orderId, int userId)
        {
            var cfus = await concreteFurnitureUnitRepository.GetAllListIncludingAsync(ent => ent.OrderId == orderId);
            var cfusWithPackingStatus = cfus.Where(ent => ent.Status == CfuStatusEnum.Packing).ToList();

            if (cfus.Count() == cfusWithPackingStatus.Count())
            {
                var order = await orderRepository.SingleAsync(ent => ent.Id == orderId);
                order.SetProductionCompleteState(userId);
            }
        }

        private async Task SetUnderProductionStatus(Guid orderId, int userId)
        {
            var order = await orderRepository.SingleAsync(ent => ent.Id == orderId);
            order.SetUnderProductionState(userId);
        }
    }
}
