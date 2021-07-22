using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class OrderState : AggregateRoot, IMultiLingualEntity<OrderStateTranslation>
    {
        public OrderStateEnum State { get; set; }

        private List<OrderStateTranslation> _translations = new List<OrderStateTranslation>();
        public ICollection<OrderStateTranslation> Translations => _translations.AsReadOnly();

        public OrderStateTranslation CurrentTranslation => (OrderStateTranslation)Translations.GetCurrentTranslation();

        
        public OrderState(OrderStateEnum state)
        {
            State = state;
        }

        public void AddTranslation(OrderStateTranslation translation)
        {
            if (translation == null)
            {
                throw new ArgumentNullException("Translation can't be null");
            }

            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding OrderStateTranslation: duplicate language: {translation.Language}");
            }

            _translations.Add(translation);
        }
    }
}
