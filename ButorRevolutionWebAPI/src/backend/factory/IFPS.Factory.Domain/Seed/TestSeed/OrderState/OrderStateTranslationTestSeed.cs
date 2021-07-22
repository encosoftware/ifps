using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class OrderStateTranslationTestSeed : IEntitySeed<OrderStateTranslation>
    {
        public OrderStateTranslation[] Entities => new[]
        {
            new OrderStateTranslation(10000,"None",LanguageTypeEnum.EN){Id = 10000},
            new OrderStateTranslation(10001,"Ready for delivery",LanguageTypeEnum.EN){Id = 10001},
            new OrderStateTranslation(10001,"Szállításra kész",LanguageTypeEnum.HU){Id = 10002},
            new OrderStateTranslation(10002,"Ready for production",LanguageTypeEnum.EN){Id = 10003},
            new OrderStateTranslation(10002,"Gyártásra kész",LanguageTypeEnum.HU){Id = 10004},
            new OrderStateTranslation(10003,"Under production",LanguageTypeEnum.EN){Id = 10005},
            new OrderStateTranslation(10003,"Gyártás alatt",LanguageTypeEnum.HU){Id = 10006},
            new OrderStateTranslation(10004,"Default",LanguageTypeEnum.EN){Id = 10007},
            new OrderStateTranslation(10004,"WaitingForFirstPayment",LanguageTypeEnum.EN){Id = 10008},
            new OrderStateTranslation(10005,"Várakozás első kifizetésre",LanguageTypeEnum.HU){Id = 10009},
            new OrderStateTranslation(10005,"FirstPaymentConfirmed",LanguageTypeEnum.EN){Id = 100010},
            new OrderStateTranslation(10006,"Első kifizetés sikeres",LanguageTypeEnum.HU){Id = 10011},
            new OrderStateTranslation(10006,"WaitingForProduction",LanguageTypeEnum.EN){Id = 10012},
            new OrderStateTranslation(10007,"Várakozás gyártásra",LanguageTypeEnum.HU){Id = 10013},
            new OrderStateTranslation(10007,"Production",LanguageTypeEnum.EN){Id = 10014},
            new OrderStateTranslation(10008,"Gyártás",LanguageTypeEnum.HU){Id = 10015},
            new OrderStateTranslation(10008,"ProductionComplete",LanguageTypeEnum.EN){Id = 10016},
            new OrderStateTranslation(10009,"Gyártás elkészült",LanguageTypeEnum.HU){Id = 10017},
            new OrderStateTranslation(10009,"WaitingForSecondPayment",LanguageTypeEnum.EN){Id = 10018},
            new OrderStateTranslation(10010,"Várakozás második kifizetésre",LanguageTypeEnum.HU){Id = 10019},
        };
    }
}
