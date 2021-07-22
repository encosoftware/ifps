using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class OrderStateTranslationSeed : IEntitySeed<OrderStateTranslation>
    {
        public OrderStateTranslation[] Entities => new[]
        {
            new OrderStateTranslation(1, "WaitingForFirstPayment", LanguageTypeEnum.EN) {Id = 2},
            new OrderStateTranslation(1, "Várakozás első kifizetésre", LanguageTypeEnum.HU) {Id = 3},
            new OrderStateTranslation(2, "FirstPaymentConfirmed", LanguageTypeEnum.EN) {Id = 4},
            new OrderStateTranslation(2, "Első kifizetés sikeres", LanguageTypeEnum.HU) {Id = 5},
            new OrderStateTranslation(3, "UnderMaterialReservation", LanguageTypeEnum.EN) {Id = 6},
            new OrderStateTranslation(3, "Foglalás alatt", LanguageTypeEnum.HU) {Id = 7},
            new OrderStateTranslation(4, "AllMaterialReserved", LanguageTypeEnum.EN) {Id = 8},
            new OrderStateTranslation(4, "Minden elem lefoglalva", LanguageTypeEnum.HU) {Id = 9},
            new OrderStateTranslation(5, "WaitingForScheduling", LanguageTypeEnum.EN) {Id = 10},
            new OrderStateTranslation(5, "Várakozás ütemezésre", LanguageTypeEnum.HU) {Id = 11},
            new OrderStateTranslation(6, "Scheduled", LanguageTypeEnum.EN) {Id = 12},
            new OrderStateTranslation(6, "Ütemezett", LanguageTypeEnum.HU) {Id = 13},
            new OrderStateTranslation(7, "UnderProduction", LanguageTypeEnum.EN) {Id = 14},
            new OrderStateTranslation(7, "Gyártás alatt", LanguageTypeEnum.HU) {Id = 15},
            new OrderStateTranslation(8, "ProductionComplete", LanguageTypeEnum.EN) {Id = 16},
            new OrderStateTranslation(8, "Gyártás elkészült", LanguageTypeEnum.HU) {Id = 17},
            new OrderStateTranslation(9, "WaitingForSecondPayment", LanguageTypeEnum.EN) {Id = 18},
            new OrderStateTranslation(9, "Várakozás második kifizetésre", LanguageTypeEnum.HU) {Id = 19},
            new OrderStateTranslation(10, "SecondPaymentConfirmed", LanguageTypeEnum.EN) {Id = 20},
            new OrderStateTranslation(10, "Második kifizetés sikeres", LanguageTypeEnum.HU) {Id = 21},
        };
    }
}
