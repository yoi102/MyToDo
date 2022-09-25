using System.Security.Cryptography;
using System.Text;

namespace MyToDo.Shared.Extensions
{
    public static class StringExtensions
    {

        public static string GetMD5(this string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            var hash = MD5.Create().ComputeHash(Encoding.Default.GetBytes(data));

            return Convert.ToBase64String(hash);
        }


    }
}