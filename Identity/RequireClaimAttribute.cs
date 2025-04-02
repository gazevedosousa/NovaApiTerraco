using Microsoft.AspNetCore.Mvc.Filters;
using TerracoDaCida.Util;

namespace TerracoDaCida.Identity
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireClaimAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _claimName;
        private readonly string _claimValue;

        public RequireClaimAttribute(string claimName, string claimValue)
        {
            _claimName = claimName;
            _claimValue = claimValue;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (!filterContext.HttpContext.User.HasClaim(_claimName, _claimValue))
            {
                throw new Exception("Usuário sem permissão para realizar essa ação");
            }
        }
    }
}
