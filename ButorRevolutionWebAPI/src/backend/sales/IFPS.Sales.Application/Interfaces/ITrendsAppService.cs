using IFPS.Sales.Application.Dto;
using IFPS.Sales.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface ITrendsAppService
    {
        Task<TrendListDto<FurnitureUnitPreviewDto>> ListFurnitureUnitOrdersTrend(int Take, DateTime? IntervalFrom, DateTime? IntervalTo);
        Task<TrendListDto<BoardMaterialPreviewDto>> ListBoardMaterialOrdersTrend(FurnitureComponentTypeEnum FurnitureComponentType, int Take, DateTime? IntervalFrom, DateTime? IntervalTo);
    }
}
