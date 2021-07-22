namespace IFPS.Sales.Application.Dto
{
    public class AddressFilterDto
    {
        public string Address { get; set; }
        public int PostCode { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }
    }
}
