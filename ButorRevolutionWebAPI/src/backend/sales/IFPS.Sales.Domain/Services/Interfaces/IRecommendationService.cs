using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Interfaces
{
    public interface IRecommendationService
    {
        Task<List<string>> GetRecommendedFurnitureUnits(List<FurnitureUnit> unitsInBasket, List<string> allFurnitureUnitCodes, Dictionary<string, Dictionary<string, float>> ruleDict, int itemNum);
        Dictionary<string, Dictionary<string, float>> GenerateRuleSet(List<string> furnitureUnitCodes, List<bool[]> basketsAsBoolLists);
    }
}