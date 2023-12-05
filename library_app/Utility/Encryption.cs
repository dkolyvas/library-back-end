namespace library_app.Utility
{
    public static class Encryption
    {
        public static string Encrypt(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }

        public static bool confirmPassword(string plainPassword, string encryptedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, encryptedPassword);
        }


    }
}
