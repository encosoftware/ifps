using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Infrastructure.Providers
{
    public class UserLanguagePreferenceProvider : RequestCultureProvider
    {
        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return NullProviderCultureResult.Result;
            }

            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int userIdInt))
            {
                return NullProviderCultureResult.Result;
            }

            var userService = GetService<IUserAppService>(httpContext);

            var userPref = await userService.GetUserPreferredLanguageCode(userIdInt);

            var cultureString = "hu-HU";
            switch (userPref)
            {
                case LanguageTypeEnum.EN:
                    {
                        cultureString = "en-GB";
                        break;
                    }
            }

            httpContext.Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);
            httpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cultureString)));

            return new ProviderCultureResult(cultureString);
        }

        private T GetService<T>(HttpContext httpContext)
        {
            return (T)httpContext.RequestServices.GetService(typeof(T));
        }
    }
}