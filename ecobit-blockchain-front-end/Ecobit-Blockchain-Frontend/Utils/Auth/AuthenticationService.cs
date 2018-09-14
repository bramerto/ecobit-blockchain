using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using Ecobit_Blockchain_Frontend.DataAccess.Interfaces;
using Ecobit_Blockchain_Frontend.Exceptions;
using Ecobit_Blockchain_Frontend.Models;
using Ecobit_Blockchain_Frontend.Wrappers;

namespace Ecobit_Blockchain_Frontend.Utils.Auth
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly IUserDao _userDao;
        private readonly IFormsAuthentication _formsAuthentication;

        public AuthenticationService() : this(DependencyFactory.Resolve<IUserDao>(), DependencyFactory.Resolve<IFormsAuthentication>())
        {
        }

        public AuthenticationService(IUserDao userDao, IFormsAuthentication formsAuthentication)
        {
            _userDao = userDao;
            _formsAuthentication = formsAuthentication;
        }

        public HttpCookie Login(string username, string password)
        {
            User userData = _userDao.Read(username);

            if (userData == null || !AreCredentialsCorrect(username, password))
            {
                throw new LoginException("Invalid credentials");
            }
            
            string data = new JavaScriptSerializer().Serialize(userData);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, username, DateTime.Now, DateTime.Now.AddYears(1), false, data);

            string cookieData = _formsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(_formsAuthentication.FormsCookieName(), cookieData)
            {
                HttpOnly = true,
                Expires = ticket.Expiration,
                Name = AuthenticationFilterAttribute.CookieName
            };
            
            return cookie;
        }

        public void Logout()
        {
            _formsAuthentication.SignOut();
        }

        public User GetUserData(HttpCookie cookie)
        {
            User userData = null;

            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = _formsAuthentication.Decrypt(cookie.Value);
                userData = new JavaScriptSerializer().Deserialize(ticket.UserData, typeof(User)) as User;
            }

            return userData;
        }

        public bool AreCredentialsCorrect(string username, string password)
        {
            User userData = _userDao.Read(username);
            
            return !string.IsNullOrEmpty(userData.Companyname) && EncryptionUtil.Verify(password, userData.Password);
        }

        public bool IsCookieDataCorrect(string username, string password)
        {
            User userData = _userDao.Read(username);

            return !string.IsNullOrEmpty(userData.Companyname) && userData.Password.Equals(password);
        }
    }
}