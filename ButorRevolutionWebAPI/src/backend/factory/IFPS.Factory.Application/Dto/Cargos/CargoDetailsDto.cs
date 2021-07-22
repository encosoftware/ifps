using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class CargoDetailsDto
    {
        public CargoDetailsByProductsDto CargoDetailsForProducts { get; set; }
        public List<ProductListDto> ProductList { get; set; }

        public CargoDetailsDto(Cargo cargo)
        {
            CargoDetailsForProducts = new CargoDetailsByProductsDto(cargo);
            ProductList = cargo.OrderedPackages.Select(ent => new ProductListDto(ent)).ToList();
        }
    }
}