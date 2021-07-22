using IFPS.Factory.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class FinanceStatisticsDto
    {
        public string MaterialCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int Quantity { get; set; }

        public static Expression<Func<Stock, FinanceStatisticsDto>> Projection
        {
            get
            {
                return ent => new FinanceStatisticsDto
                {
                    MaterialCode = ent.Package.Material.Code,
                    DateFrom = ent.ValidFrom,
                    DateTo = ent.ValidTo,
                    Quantity = ent.Quantity
                };
            }
        }
    }
}