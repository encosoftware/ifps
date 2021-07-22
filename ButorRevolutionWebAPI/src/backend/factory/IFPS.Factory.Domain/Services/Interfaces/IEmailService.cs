using IFPS.Factory.Domain.Model;
using MimeKit;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(User user, string subject, MimeEntity mimeEntity, int emailDataId);
    }
}
