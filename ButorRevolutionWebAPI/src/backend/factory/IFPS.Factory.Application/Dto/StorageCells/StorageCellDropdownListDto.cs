using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class StorageCellDropdownListDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public StorageCellDropdownListDto()
        {

        }

        public StorageCellDropdownListDto(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
