namespace Ecobit_Blockchain_Frontend.Utils
{
    public static class EncryptionUtil
    {

        /// <summary>
        /// Generate a BCrypt hash.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>A string containing the hash.</returns>
        public static string GenerateHash(string password)
        {
            string mySalt = BCrypt.Net.BCrypt.GenerateSalt(12);
            return BCrypt.Net.BCrypt.HashPassword(password, mySalt);
        }

        /// <summary>
        /// Check if a hash matches a password.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hash"></param>
        /// <returns>True if the hash matches the password.</returns>
        public static bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        
    }
}