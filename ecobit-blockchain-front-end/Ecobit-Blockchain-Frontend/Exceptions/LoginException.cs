using System;
using System.Runtime.Serialization;

namespace Ecobit_Blockchain_Frontend.Exceptions
{
    [Serializable]
    public class LoginException : Exception
    {
        public LoginException(string message) : base(message)
        {
        }

        public LoginException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LoginException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}