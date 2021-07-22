using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<int> GetCurrentUserIdAsync();

        int GetCurrentUserId();
    }
}
