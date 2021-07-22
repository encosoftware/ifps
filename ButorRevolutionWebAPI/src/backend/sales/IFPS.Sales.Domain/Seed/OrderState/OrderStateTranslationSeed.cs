using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class OrderStateTranslationSeed : IEntitySeed<OrderStateTranslation>
    {
        public OrderStateTranslation[] Entities =>  new[]
        {
            new OrderStateTranslation(1, "None",LanguageTypeEnum.EN){Id = 1},
            new OrderStateTranslation(2, "OrderCreated",LanguageTypeEnum.EN){Id = 2},
            new OrderStateTranslation(2, "Rendelés elkészült",LanguageTypeEnum.HU){Id = 3},
            new OrderStateTranslation(3, "WaitingForOffer",LanguageTypeEnum.EN){Id = 4},
            new OrderStateTranslation(3, "Várakozás árajánlatra",LanguageTypeEnum.HU){Id = 5},
            new OrderStateTranslation(4, "WaitingForOfferFeedback",LanguageTypeEnum.EN){Id = 6},
            new OrderStateTranslation(4, "Várakozás árajánlat elfogadására",LanguageTypeEnum.HU){Id = 7},
            new OrderStateTranslation(5, "OfferDeclined",LanguageTypeEnum.EN){Id = 8},
            new OrderStateTranslation(5, "Árajánlat visszautasítva",LanguageTypeEnum.HU){Id = 9},
            new OrderStateTranslation(6, "WaitingForContract",LanguageTypeEnum.EN){Id = 10},
            new OrderStateTranslation(6, "Várakozás szerződéskötésre",LanguageTypeEnum.HU){Id = 11},
            new OrderStateTranslation(7, "ContractSigned",LanguageTypeEnum.EN){Id = 12},
            new OrderStateTranslation(7, "Szerződés aláírva",LanguageTypeEnum.HU){Id = 13},
            new OrderStateTranslation(8, "Under production",LanguageTypeEnum.EN){Id = 14},
            new OrderStateTranslation(8, "Gyártás alatt",LanguageTypeEnum.HU){Id = 15},
            new OrderStateTranslation(9, "WaitingForShipping",LanguageTypeEnum.EN){Id = 16},
            new OrderStateTranslation(9, "Várakozás szállításra",LanguageTypeEnum.HU){Id = 17},
            new OrderStateTranslation(10, "Delivered",LanguageTypeEnum.EN){Id = 18},
            new OrderStateTranslation(10, "Kézbesítve",LanguageTypeEnum.HU){Id = 19},
            new OrderStateTranslation(11, "WaitingForInstallation",LanguageTypeEnum.EN){Id = 20},
            new OrderStateTranslation(11, "Várakozás összeszerelésre",LanguageTypeEnum.HU){Id = 21},
            new OrderStateTranslation(12, "Installed",LanguageTypeEnum.EN){Id = 22},
            new OrderStateTranslation(12, "Összeszerelt",LanguageTypeEnum.HU){Id = 23},
            new OrderStateTranslation(13, "WaitingForRepair",LanguageTypeEnum.EN){Id = 24},
            new OrderStateTranslation(13, "Várakozás javításra",LanguageTypeEnum.HU){Id = 25},
            new OrderStateTranslation(14, "Completed",LanguageTypeEnum.EN){Id = 26},
            new OrderStateTranslation(14, "Elkészült",LanguageTypeEnum.HU){Id = 27},
            new OrderStateTranslation(15, "UnderGuaranteeRepair",LanguageTypeEnum.EN){Id = 28},
            new OrderStateTranslation(15, "Garanicális javítás alatt",LanguageTypeEnum.HU){Id = 29},
            new OrderStateTranslation(16, "Default",LanguageTypeEnum.EN){Id = 30},
            new OrderStateTranslation(17, "WaitingForContractFeedback",LanguageTypeEnum.EN) {Id = 31},
            new OrderStateTranslation(17, "Várakozás szerződés elfogadására",LanguageTypeEnum.HU) {Id = 32 },
            new OrderStateTranslation(18, "Contract declined",LanguageTypeEnum.EN) {Id = 33 },
            new OrderStateTranslation(18, "Szerződés visszautasítva",LanguageTypeEnum.HU) {Id = 34 },
            new OrderStateTranslation(19, "Waiting for shipping's appointment",LanguageTypeEnum.EN) { Id = 35 },
            new OrderStateTranslation(19, "Várakozás kiszállítási időpontra",LanguageTypeEnum.HU) { Id = 36 },
            new OrderStateTranslation(20, "Waiting for on-site survey",LanguageTypeEnum.EN) { Id = 37 },
            new OrderStateTranslation(20, "Várakozás helyszíni felmérésre",LanguageTypeEnum.HU) { Id = 38 },
            new OrderStateTranslation(21, "On-site survey is done",LanguageTypeEnum.EN) { Id = 39 },
            new OrderStateTranslation(21, "Helyszíni felmérés elvégezve",LanguageTypeEnum.HU) { Id = 40 },
            new OrderStateTranslation(22, "Waiting for on-site survey's appointment",LanguageTypeEnum.EN) { Id = 41 },
            new OrderStateTranslation(22, "Várakozás időpontfoglalásra helyszíni felméréshez",LanguageTypeEnum.HU) { Id = 42 }
        };
    }
}
