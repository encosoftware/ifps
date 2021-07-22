using System.Threading.Tasks;

namespace IFPS.Factory.Application.BackgroundJobs
{
    public interface IEmailSenderJob
    {
        Task SendAllEmails();
    }
}
