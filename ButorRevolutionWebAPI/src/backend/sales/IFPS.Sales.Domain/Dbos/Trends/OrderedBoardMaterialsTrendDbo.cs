using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.Domain.Dbos.Trends
{
    public class OrderedBoardMaterialsTrendDbo
    {
        public Guid BoardMaterialId { get; set; }
        public BoardMaterial BoardMaterial { get; set; }
        public int OrdersCount { get; set; }
    }
}
