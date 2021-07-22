using IFPS.Sales.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Implementations
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpContext context;

        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            context = httpContextAccessor.HttpContext;
        }

        public Task<int> GetCurrentUserIdAsync()
        {
            return Task.FromResult(GetCurrentUserId());
        }

        public int GetCurrentUserId()
        {
            string userIdString = null;

            if (context != null)
                userIdString = context.User.FindFirst("UserId")?.Value ?? default(string);

            return int.TryParse(userIdString, out var userId) ? userId : 0;
        }
    }
}
