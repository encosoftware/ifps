using IFPS.Sales.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cotur.DataMining.AssociationMining;
using System.Linq;
using IFPS.Sales.Domain.Model;
using Microsoft.Extensions.Configuration;

namespace IFPS.Sales.Domain.Services.Implementations
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IConfiguration configuration;

        public RecommendationService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<List<string>> GetRecommendedFurnitureUnits(List<FurnitureUnit> unitsInBasket, List<string> allFurnitureUnitCodes, Dictionary<string, Dictionary<string, float>> ruleDict, int itemNum)
        {
            var unitCodes = unitsInBasket.Select(fu => fu.Code).OrderBy(x => x).ToList();
            var maxLiftDict = new Dictionary<string, float>();

            if (unitCodes.Count > 1)
            {                
                foreach (var unitCode in allFurnitureUnitCodes)
                {
                    maxLiftDict.Add(unitCode, 0f);
                }

                var codeCombinations = GetAllCombinations(unitCodes);

                // logic: get max lift for each code and code combination and find the maximum lifts for the whole basket
                foreach (var codeCombination in codeCombinations)
                {
                    var liftDict = ruleDict[codeCombination];

                    foreach (var kv in liftDict)
                    {
                        if (kv.Value > maxLiftDict[kv.Key])
                        {
                            maxLiftDict[kv.Key] = kv.Value;
                        }
                    }
                }
            }
            else
            {
                maxLiftDict = ruleDict[unitCodes[0]];
            }            

            // sort by lift and return the best codes
            var recommendationsList = maxLiftDict.ToList();
            recommendationsList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            var recommendedItemCodes = recommendationsList.Take(itemNum).Select(x => x.Key).ToList();

            return Task.FromResult(recommendedItemCodes);
        }

        public Dictionary<string, Dictionary<string, float>> GenerateRuleSet(List<string> furnitureUnitCodes, List<bool[]> basketsAsBoolLists)
        {
            int maxCombinationLength = configuration.GetValue<int>("ApplicationSettings:CartAnalysis:CombinationLength");       
            var data = new DataFields(furnitureUnitCodes, basketsAsBoolLists);

            var aprioriAlgorithm = new Apriori(data, maxCombinationLength);
            float minSupport = 0f;

            aprioriAlgorithm.CalculateCNodes(minSupport, configuration.GetValue<string>("ApplicationSettings:CartAnalysis:Delimiter"));

            var ruleDict = aprioriAlgorithm.Rules
                .GroupBy(r => r.GetDictKey(data))
                .ToDictionary(x => x.Key,
                              x => x.ToDictionary(r => data.GetElementName(r.NodeB.ElementIDs[0]),
                                                  r => r.Lift ));        

            return ruleDict;
        }

        private List<string> GetAllCombinations(List<string> fuCodes)
        {
            // get all pair combinations of FU codes
            var combinations = fuCodes.ToList();

            for (int i = 0; i < fuCodes.Count; i++)
            {                
                for (int j = (i + 1); j < fuCodes.Count; j++)
                {
                    string tmpComb = fuCodes[i] + configuration.GetValue<string>("ApplicationSettings:CartAnalysis:Delimiter") + fuCodes[j];
                    combinations.Add(tmpComb);
                }
            }

            return combinations;
        }
    }
}
