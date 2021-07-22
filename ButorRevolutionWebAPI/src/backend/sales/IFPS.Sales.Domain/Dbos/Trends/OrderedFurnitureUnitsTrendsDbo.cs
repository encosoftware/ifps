using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace IFPS.Sales.Domain.Dbos.Trends
{
    public class OrderedFurnitureUnitsTrendsDbo
    {
        public Guid FurnitureUnitId { get; set; }
        public FurnitureUnit FurnitureUnit { get; set; }
        public int OrdersCount { get; set; }
       
    }
}
