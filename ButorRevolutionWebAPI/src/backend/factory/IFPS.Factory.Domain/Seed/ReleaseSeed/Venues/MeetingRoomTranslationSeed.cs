using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class MeetingRoomTranslationSeed : IEntitySeed<MeetingRoomTranslation>
    {
        public MeetingRoomTranslation[] Entities => new[]
        {
            new MeetingRoomTranslation(1, "Kék tárgyaló", "B épület második emelet jobbra", LanguageTypeEnum.HU) { Id = 1 },
            new MeetingRoomTranslation(1, "Blue meeting room", "B entry second floor to the right", LanguageTypeEnum.EN) { Id = 2 },
            new MeetingRoomTranslation(2, "Nagy tárgyaló", "B épület második emelet balra", LanguageTypeEnum.HU) { Id = 3 },
            new MeetingRoomTranslation(2, "Big meeting room", "B entry second floor to the left", LanguageTypeEnum.EN) { Id = 4 },
        };
    }
}
