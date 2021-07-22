using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class BookedByDropdownListDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public BookedByDropdownListDto(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
