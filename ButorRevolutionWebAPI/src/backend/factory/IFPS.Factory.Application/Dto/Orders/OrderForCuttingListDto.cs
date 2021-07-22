using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class OrderForCuttingListDto
    {
        public List<OrderDeatilsForCuttingDto> OrderDetails { get; set; }
        public List<ConcreteFurnitureUnitForCuttingListDto> ConcreteFurnitureUnits { get; set; }
        public List<ConcreteFurnitureComponentForCuttingListDto> ConcreteFurnitureComponents { get; set; }
        public List<BoardMaterialForCuttingListDto> Boards { get; set; }
        public WorkStationsForCuttingListDto Workstations { get; set; }

        public OrderForCuttingListDto()
        {
            OrderDetails = new List<OrderDeatilsForCuttingDto>();
            ConcreteFurnitureUnits = new List<ConcreteFurnitureUnitForCuttingListDto>();
            ConcreteFurnitureComponents = new List<ConcreteFurnitureComponentForCuttingListDto>();
            Boards = new List<BoardMaterialForCuttingListDto>();
            Workstations = new WorkStationsForCuttingListDto();
        }

        public void SetProperties(Order order, List<int> concreteFurnitureUnitIds, List<int> concreteFurnitureComponentIds, List<Drill> drillsFromModel)
        {
            OrderDetails.Add(new OrderDeatilsForCuttingDto(order));
            foreach (var cfu in order.ConcreteFurnitureUnits)
            {
                ConcreteFurnitureUnits.Add(new ConcreteFurnitureUnitForCuttingListDto(cfu));
            }
            foreach (var cfu in order.ConcreteFurnitureUnits)
            {
                foreach (var cfc in cfu.ConcreteFurnitureComponents)
                {
                    var drills = drillsFromModel.Where(ent => ent.FurnitureComponentId == cfc.FurnitureComponentId).ToList();
                    var holes = drills.Select(ent => ent.Holes).Count();
                    var rnd = new Random();
                    ConcreteFurnitureComponents.Add(new ConcreteFurnitureComponentForCuttingListDto(cfc)
                    {
                        //TODO fix this!
                        //CncHoles = holes,
                        //CncDistance = cfc.FurnitureComponent.CalculateDistance()
                        CncHoles = rnd.Next(10, 15),
                        CncDistance = rnd.Next(1000, 1100)
                    });
                    concreteFurnitureComponentIds.Add(cfc.Id);
                }
                concreteFurnitureUnitIds.Add(cfu.Id);
            }
        }
    }
}
