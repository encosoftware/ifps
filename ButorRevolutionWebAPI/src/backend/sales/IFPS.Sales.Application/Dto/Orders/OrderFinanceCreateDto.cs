using System;

namespace IFPS.Sales.Application.Dto
{
    public class OrderFinanceCreateDto
    {
        public DateTime PaymentDate { get; set; }
        public int PaymentIndex { get; set; }
    }
}
