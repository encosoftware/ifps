using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class OrderFinanceDetailsDto
    {
        public DateTime? FirstPaymentDate { get; set; }
        public bool FirstPaymentSet { get; set; }
        public DateTime? SecondPaymentDate { get; set; }
        public bool SecondPaymentSet { get; set; }
        public PriceListDto FirstPaymentAmount { get; set; }
        public PriceListDto SecondPaymentAmount { get; set; }


        public static Func<Order, OrderFinanceDetailsDto> FromEntity = entity => new OrderFinanceDetailsDto
        {
            FirstPaymentAmount = entity.FirstPayment != null ? new PriceListDto(entity.FirstPayment.Price) : null,
            FirstPaymentSet = entity.FirstPayment.PaymentDate != null,
            SecondPaymentAmount = entity.SecondPayment != null ? new PriceListDto(entity.SecondPayment.Price) : null,
            SecondPaymentSet =entity.SecondPayment.PaymentDate != null,
            FirstPaymentDate = entity.FirstPayment != null ? entity.FirstPayment.PaymentDate.GetValueOrDefault() : (DateTime?)null,
            SecondPaymentDate = entity.SecondPayment != null ? entity.SecondPayment.PaymentDate.GetValueOrDefault() : (DateTime?)null
        };
    }
}
