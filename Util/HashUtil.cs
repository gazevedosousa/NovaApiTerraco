using System.Security.Cryptography;
using System.Text;

namespace TerracoDaCida.Util
{
    public class HashUtil
    {
        public static void CriaSenhaHash(string senha, out byte[] hash, out byte[] salt)
        {
            using (HMACSHA512 hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
            }
        }

        public static bool VerificaSenhaHash(string senha, byte[] hash, byte[] salt)
        {
            using (HMACSHA512 hmac = new HMACSHA512(salt))
            {
                byte[] verificaHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
                return verificaHash.SequenceEqual(hash);
            }
        }
    }
}
