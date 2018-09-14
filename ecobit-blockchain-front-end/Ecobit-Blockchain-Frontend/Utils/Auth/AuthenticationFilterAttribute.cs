using System.Web.Mvc;
using System.Web.Routing;
using Ecobit_Blockchain_Frontend.Models;

namespace Ecobit_Blockchain_Frontend.Utils.Auth
{
    public class AuthenticationFilterAttribute : ActionFilterAttribute
    {
        public const string CookieName = "Ecobit_Cookie";
        
        private readonly IAuthenticationService _authService;

        public AuthenticationFilterAttribute() : this(DependencyFactory.Resolve<IAuthenticationService>())
        {
        }
        
        public AuthenticationFilterAttribute(IAuthenticationService authService)
        {
            _authService = authService;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var req = filterContext.HttpContext.Request;
            var cookie = req.Cookies.Get(CookieName);
            
            if (cookie != null)
            {
                User userData = _authService.GetUserData(cookie);

                if (_authService.IsCookieDataCorrect(userData.Companyname, userData.Password))
                {
                    return;
                }
            }

            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary 
                { 
                    { "controller", "Account" }, 
                    { "action", "Index" } 
                });
        }
    }
}