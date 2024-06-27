using System.Security.Cryptography;
using System.Text;

namespace WhoOwesWhat.Domain
{


    public interface IHashUtils
    {
        byte[] GetHash(string inputString);
        string GetHashString(string inputString);
    }

    public class HashUtils : IHashUtils
    {

        public byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA1.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        
    }
}