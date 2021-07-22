using System.Threading.Tasks;

namespace IFPS.Sales.Application.BackgroundJobs
{
    public interface IEmailSenderJob
    {
        Task SendAllEmails();
    }
}