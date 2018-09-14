using System;
using System.Runtime.Serialization;

namespace Ecobit_Blockchain_Frontend.Exceptions
{
    [Serializable]
    public class UserException : Exception
    {
        public UserException(string message) : base(message)
        {
        }

        public UserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}