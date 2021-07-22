namespace IFPS.Factory.Application.Dto
{
    public class ProductsUpdateDto
    {
        public int Id { get; set; }
        public int Missing { get; set; }
        public int Refused { get; set; }

        public ProductsUpdateDto()
        {
        }
    }
}