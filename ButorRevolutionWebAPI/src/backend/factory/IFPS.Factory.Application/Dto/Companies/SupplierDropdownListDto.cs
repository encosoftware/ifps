namespace IFPS.Factory.Application.Dto
{
    public class SupplierDropdownListDto
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }

        public SupplierDropdownListDto(int id, string name)
        {
            Id = id;
            SupplierName = name;
        }
    }
}
