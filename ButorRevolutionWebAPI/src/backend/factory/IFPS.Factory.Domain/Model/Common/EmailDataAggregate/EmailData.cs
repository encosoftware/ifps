using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class EmailData : AggregateRoot, IMultiLingualEntity<EmailDataTranslation>
    {
        public EmailTypeEnum Type { get; private set; }

        /// <summary>
        /// Private list of translations
        /// </summary>
        private List<EmailDataTranslation> _translations;
        public ICollection<EmailDataTranslation> Translations => _translations.AsReadOnly();

        public EmailDataTranslation CurrentTranslation => Translations.GetCurrentTranslation();

        private EmailData()
        {
            _translations = new List<EmailDataTranslation>();
        }

        public EmailData(EmailTypeEnum type) : this()
        {
            Type = type;
        }

        public void AddTranslation(EmailDataTranslation translation)
        {
            Ensure.NotNull(translation);

            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding EmailDataTranslation: duplicate language: {translation.Language}");
            }

            _translations.Add(translation);
        }
    }
}
