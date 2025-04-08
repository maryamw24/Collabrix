    namespace Collabrix.Helper
    {
        public static class PasswordHasher
        {
            // This method hashes the password
            public static string HashPassword(string password)
            {
                return BCrypt.Net.BCrypt.HashPassword(password);
            }

            // This method verifies the password
            public static bool VerifyPassword(string password, string hashedPassword)
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
        }
    }
