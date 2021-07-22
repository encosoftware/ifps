using System;

namespace IFPS.Sales.Application.Dto
{
    public class BoardMaterialsForDropdownDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public BoardMaterialsForDropdownDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
