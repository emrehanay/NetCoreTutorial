using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace NetCoreTutorial.Helpers
{
    public class HashHelper
    {
        private const string SaltKey = "ExampleKey";

        public static string HashCreate(string value)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                password: value,
                salt: Encoding.UTF8.GetBytes(SaltKey),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(valueBytes);
        }

        public static bool ValidateHash(string value, string hash)
            => HashCreate(value) == hash;

        public static string RandomHashCreate()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }
}