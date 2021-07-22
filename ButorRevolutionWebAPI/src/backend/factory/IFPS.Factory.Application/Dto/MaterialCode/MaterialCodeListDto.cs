using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class MaterialCodeListDto
    {
        public Guid Id { get; set; }

        public string MaterialCode { get; set; }

        public MaterialCodeListDto(Guid id, string code)
        {
            Id = id;
            MaterialCode = code;
        }
    }
}
