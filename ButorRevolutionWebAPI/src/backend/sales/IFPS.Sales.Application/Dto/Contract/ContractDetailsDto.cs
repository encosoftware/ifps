using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class ContractDetailsDto
    {
        public ProducerDetailsByContractDto Producer { get; set; }
        public CustomerDetailsByContractDto Customer { get; set; }
        public FinancialDetailsByContractDto Financial { get; set; }
        public string Aggrement { get; set; }
        public string Additional { get; set; }
        public DateTime? ContractDate { get; set; }

        public ContractDetailsDto(Order order)
        {
            Producer = new ProducerDetailsByContractDto(order.SalesPerson);
            Customer = new CustomerDetailsByContractDto(order);
            Financial = new FinancialDetailsByContractDto();
            Aggrement = order.Aggrement;
            Additional = order.Additional;
            ContractDate = order.ContractDate;
        }
    }
}
