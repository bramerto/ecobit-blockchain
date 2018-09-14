using System.Web.Security;

namespace Ecobit_Blockchain_Frontend.Wrappers
{
    /// <summary>
    /// Wrapper interface for System.Web.Security.FormsAuthenication.
    /// This is mainly used so we mock this class during tests.
    /// </summary>
    public interface IFormsAuthentication
    {
        string Encrypt(FormsAuthenticationTicket ticket);
        FormsAuthenticationTicket Decrypt(string encryptedTicket);
        void SignOut();
        string FormsCookieName();
    }
}