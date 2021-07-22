using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed.EventTypes
{
    public class EventTypeTranslationSeed : IEntitySeed<EventTypeTranslation>
    {
        public EventTypeTranslation[] Entities => new[]
        {
            new EventTypeTranslation(1, "Új időpont", "A felhasználó értesítést kap az új események létrehozásáról", LanguageTypeEnum.HU) { Id = 1},
            new EventTypeTranslation(1, "New appointment", "The user is notified about newly registered events", LanguageTypeEnum.EN) { Id = 2},

            new EventTypeTranslation(2, "Esemény emlékezetető", "A felhasználó értesítést kap a közelgő eseményekről", LanguageTypeEnum.HU) { Id = 3},
            new EventTypeTranslation(2, "Appointment reminder", "The user is notified about upcoming events", LanguageTypeEnum.EN) { Id = 4},

            new EventTypeTranslation(3, "Állapot változás", "A felhasználó értesítést kap a hozzá tartozó megrendelés állapotváltozásáról", LanguageTypeEnum.HU) { Id = 5},
            new EventTypeTranslation(3, "Change order state", "The user is notified about upcoming events", LanguageTypeEnum.EN) { Id = 6},

            new EventTypeTranslation(4, "Új dokumentumok", "A felhasználó értesítést kap, amennyiben számára új dokumentum lett feltöltve", LanguageTypeEnum.HU) { Id = 7},
            new EventTypeTranslation(4, "New files uploaded", "The user is notified about new uploaded files", LanguageTypeEnum.EN) { Id = 8},

            new EventTypeTranslation(5, "Új üzenet", "A felhasználó értesítést kap, amennyiben új üzenete érkezett", LanguageTypeEnum.HU) { Id = 9},
            new EventTypeTranslation(5, "New message", "The user is notified about received messages", LanguageTypeEnum.EN) { Id = 10},

            new EventTypeTranslation(6, "Értékelés", "A felhasználó értesítést kap, amennyiben közeleg a megrendelésének az értékelési határideje", LanguageTypeEnum.HU) { Id = 11},
            new EventTypeTranslation(6, "Order evaluation", "The user is notified about upcoming evaluation deadline", LanguageTypeEnum.EN) { Id = 12}
        };
    }
}
