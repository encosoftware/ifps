namespace IFPS.Factory.Application.Dto
{
    public class OrderStateTypeDropdownListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public OrderStateTypeDropdownListDto() { }

        public OrderStateTypeDropdownListDto(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
