using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace CRUDMVC.Resources
{
    public class Utilities
    {
        public static string EncriptarClave(string password)
        {
            StringBuilder sb = new StringBuilder();

            using(SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;

                byte[] resultBytes = hash.ComputeHash(enc.GetBytes(password));

                // Convertir bytes a cadena Base64
                string base64String = Convert.ToBase64String(resultBytes);

                return base64String;
            }
        }

    }
}
