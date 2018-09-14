using System;
using System.Web;
using System.Web.Mvc;
using Ecobit_Blockchain_Frontend.Exceptions;
using Ecobit_Blockchain_Frontend.Models;
using Ecobit_Blockchain_Frontend.Utils;
using Ecobit_Blockchain_Frontend.Utils.Auth;

namespace Ecobit_Blockchain_Frontend.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authService;
        
        public AccountController() : this(DependencyFactory.Resolve<IAuthenticationService>())
        {
            
        }
        
        public AccountController(IAuthenticationService authenticationService)
        {
            _authService = authenticationService;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                HttpCookie cookie = _authService.Login(model.Username, model.Password);
                Response.Cookies.Set(cookie);
            }
            catch (LoginException e)
            {
                ModelState.AddModelError("LoginError", e.Message);
                return View("Index", model);       
            }
            HttpContext.Session["Username"] = model.Username;
            return RedirectToAction("Index", "Batch"); 
        }
        
        [HttpGet]
        public ActionResult LogOut()
        {
            var cookie = new HttpCookie(AuthenticationFilterAttribute.CookieName, "") {Expires = DateTime.Now.AddYears(-1)};
            Response.Cookies.Add(cookie);
            _authService.Logout();
            HttpContext.Session["Username"] = null;
            return RedirectToAction("Index", "Account", null);
        }
    }
}
