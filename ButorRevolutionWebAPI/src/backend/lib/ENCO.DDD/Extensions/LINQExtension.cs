using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;
using ENCO.DDD.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ENCO.DDD.Extensions
{
    public static class LINQExtension
    {
        public static T GetCurrentTranslation<T>(this IEnumerable<T> source) where T : class, IEntityTranslation
        {
            var language = (LanguageTypeEnum)Enum.Parse(typeof(LanguageTypeEnum), CultureInfo.CurrentCulture.Name.Split('-').First().ToUpper());

            return source.Any(p => p.Language == language) ? source.FirstOrDefault(p => p.Language == language) : source.FirstOrDefault();
        }
    }
}
