using System.Security.Cryptography;
using System.Text;

namespace FruitStore.Helpers
{
    public static class Encriptacion
    {
        public static string StringToSha512(string s)
        {
            using (var sha512 = SHA512.Create())
            {
                var arreglo = Encoding.UTF8.GetBytes(s);
                var hash = sha512.ComputeHash(arreglo);
                return Convert.ToHexString(hash).ToLower();
            }
        }
    }
}
