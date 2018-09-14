using System;

namespace Application.Util
{
    public static class GenerateIdUtil
    {
        /// <summary>
        /// Generates a unique id
        /// </summary>
        /// <returns>a Transaction</returns>
        public static string GenerateUniqueId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}