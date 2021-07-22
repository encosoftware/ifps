using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace IFPS.Factory.API.Common
{
    [ApiController]
    public class IFPSControllerBase : ControllerBase
    {
        protected int GetCallerId()
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                var idClaim = identity.Claims.FirstOrDefault(x => x.Type == "UserId")
                    ?? throw new ArgumentException("Id not found!");

                return int.Parse(idClaim.Value);
            }
            throw new Exception("ClaimsIdentity not found!");
        }
    }
}
