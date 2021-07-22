using IFPS.Factory.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class CargoUpdateDto
    {
        public List<ProductsUpdateDto> Products { get; set; }

        public CargoUpdateDto()
        {
                     
        }
    }
}