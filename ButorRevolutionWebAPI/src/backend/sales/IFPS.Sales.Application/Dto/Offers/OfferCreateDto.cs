
namespace IFPS.Sales.Application.Dto
{
    public class OfferCreateDto
    {
        public RequiresCreateDto Requires { get; set; }
        public CabinetMaterialCreateDto TopCabinet { get; set; }
        public CabinetMaterialCreateDto BaseCabinet { get; set; }
        public CabinetMaterialCreateDto TallCabinet { get; set; }

        public OfferCreateDto()
        {
            Requires = new RequiresCreateDto();
            TopCabinet = new CabinetMaterialCreateDto();
            BaseCabinet = new CabinetMaterialCreateDto();
            TallCabinet = new CabinetMaterialCreateDto();
        }
    }
}
