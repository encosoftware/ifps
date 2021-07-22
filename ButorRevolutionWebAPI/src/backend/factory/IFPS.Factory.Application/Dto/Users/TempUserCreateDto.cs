using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class TempUserCreateDto
    {
        public string EmailAddress { get; set; }

        public LanguageTypeEnum Language { get; set; }

        public TempUserCreateDto()
        {
            EmailAddress = "temp@user.com";
            Language = LanguageTypeEnum.EN;
        }

        public User CreateModelObject()
        {
            return new User(EmailAddress, Language) { };
        }
    }
}
