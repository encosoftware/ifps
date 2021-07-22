using IFPS.Factory.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Factory.FunctionalTests.Services
{
    public class TestIdentityService : IIdentityService
    {
        public int GetCurrentUserId()
        {
            return 1;
        }

        public Task<int> GetCurrentUserIdAsync()
        {
            return Task.FromResult(GetCurrentUserId());
        }
    }
}
