using System.Security.Cryptography;

namespace LidaCarreiras.Funcoes
{
    public class HashSenha
    {
        public static string GeraHash(string senha)
        {
            byte[] senhaBytes = System.Text.Encoding.UTF8.GetBytes(senha);
            byte[] hashBytes = SHA256.Create().ComputeHash(senhaBytes);
            string hashString = BitConverter.ToString(hashBytes);
            hashString = hashString.Replace("-", String.Empty);
            return hashString;
        }
    }
}