using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    internal class SecurityService
    {
        public string GenerarSalt(int sizeBytes = 16)
        {
            byte[] salt = new byte[sizeBytes];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public string HashPassword(string password, string saltBase64)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (saltBase64 == null) throw new ArgumentNullException(nameof(saltBase64));

            // Criterio: SHA256( saltBytes || passwordUtf8 )
            byte[] saltBytes = Convert.FromBase64String(saltBase64);
            byte[] passBytes = Encoding.UTF8.GetBytes(password);

            byte[] toHash = new byte[saltBytes.Length + passBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, toHash, 0, saltBytes.Length);
            Buffer.BlockCopy(passBytes, 0, toHash, saltBytes.Length, passBytes.Length);

            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(toHash);
                return Convert.ToBase64String(hash);
            }
        }

        public bool Verify(string password, string saltBase64, string expectedHashBase64)
        {
            if (expectedHashBase64 == null) return false;

            string computed = HashPassword(password, saltBase64);

            // Comparación en tiempo constante
            byte[] a = Convert.FromBase64String(computed);
            byte[] b = Convert.FromBase64String(expectedHashBase64);
            return FixedTimeEquals(a, b);
        }

        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null) return false;

            int diff = a.Length ^ b.Length;
            int len = Math.Min(a.Length, b.Length);

            for (int i = 0; i < len; i++)
                diff |= a[i] ^ b[i];

            return diff == 0;
        }
    }
}
