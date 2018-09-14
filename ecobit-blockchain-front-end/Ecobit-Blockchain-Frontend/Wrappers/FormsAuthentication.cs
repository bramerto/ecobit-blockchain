using System.Web.Security;

namespace Ecobit_Blockchain_Frontend.Wrappers
{
    public class FormsAuthentication : IFormsAuthentication
    {
        public string Encrypt(FormsAuthenticationTicket ticket)
        {
            return System.Web.Security.FormsAuthentication.Encrypt(ticket);
        }

        public FormsAuthenticationTicket Decrypt(string encryptedTicket)
        {
            return System.Web.Security.FormsAuthentication.Decrypt(encryptedTicket);
        }

        public void SignOut()
        {
            System.Web.Security.FormsAuthentication.SignOut();
        }

        public string FormsCookieName()
        {
            return System.Web.Security.FormsAuthentication.FormsCookieName;
        }
    }
}