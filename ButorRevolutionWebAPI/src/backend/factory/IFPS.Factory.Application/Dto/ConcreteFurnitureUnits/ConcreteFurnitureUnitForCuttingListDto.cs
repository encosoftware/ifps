using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class ConcreteFurnitureUnitForCuttingListDto
    {
        public int Id { get; set; }

        public Guid OrderGuid { get; set; }

        public string Name { get; set; }

        public List<int> ComponentIds { get; set; }

        public int Accessories { get; set; }

        public int CfuProductionStatus { get; set; }

        public ConcreteFurnitureUnitForCuttingListDto(ConcreteFurnitureUnit cfu)
        {
            Id = cfu.Id;
            OrderGuid = cfu.OrderId;
            Name = cfu.FurnitureUnit.Code;
            ComponentIds = new List<int>();
            foreach(var cfc in cfu.ConcreteFurnitureComponents)
            {
                ComponentIds.Add(cfc.Id);
            }
            int accessories = 0;
            foreach (var accessory in cfu.FurnitureUnit.Accessories)
            {
                accessories += accessory.AccessoryAmount;
            }
            Accessories = accessories;
            
            CfuProductionStatus = (int)cfu.Status;
        }
    }
}
