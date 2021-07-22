using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Sales.Domain.Model
{
    public class EmailDataTranslation : Entity, IEntityTranslation<EmailData>
    {
        public string FileName { get; set; }
        public string ImageFileName { get; set; }

        public EmailData Core { get; private set; }
        public int CoreId { get; private set; }

        public LanguageTypeEnum Language { get; private set; }

        private EmailDataTranslation() { }

        public EmailDataTranslation(int coreId, string fileName, string imageFileName, LanguageTypeEnum language)
        {
            CoreId = coreId;
            FileName = fileName;
            ImageFileName = imageFileName;
            Language = language;
        }
    }
}
