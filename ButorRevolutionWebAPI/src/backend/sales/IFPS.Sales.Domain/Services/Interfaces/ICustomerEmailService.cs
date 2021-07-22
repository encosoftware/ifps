using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Interfaces
{
    public interface ICustomerEmailService
    {
        Task SendEmailToCustomerAsync(User user, OrderState orderState, int emailDataId, string workingNumber);
    }
}
