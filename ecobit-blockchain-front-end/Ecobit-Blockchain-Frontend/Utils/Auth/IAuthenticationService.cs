using System.Web;
using Ecobit_Blockchain_Frontend.Models;

namespace Ecobit_Blockchain_Frontend.Utils.Auth
{
    public interface IAuthenticationService
    {

        /// <summary>
        /// Log a user in. A cookie will be set with the userData.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>A cookie with the user credentials</returns>
        HttpCookie Login(string username, string password);
        
        /// <summary>
        /// Log a user out.
        /// </summary>
        void Logout();

        /// <summary>
        /// Deserialize user data from a cookie.
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns>The user that is stored in the cookie.</returns>
        User GetUserData(HttpCookie cookie);

        /// <summary>
        /// Check if the user's credentials are correct.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool AreCredentialsCorrect(string username, string password);

        /// <summary>
        /// Check if data stored in a cookie is correct.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool IsCookieDataCorrect(string username, string password);
    }
}