using System.Text;

namespace LidaCarreiras.Funcoes
{
    public class Base64
    {
        public static string Encode(string stringDados)
        {
            var valores = Encoding.UTF8.GetBytes(stringDados);
            return Convert.ToBase64String(valores);
        }

        public static string Decode(string stringDados)
        {
            var valores = Convert.FromBase64String(stringDados);
            return Encoding.UTF8.GetString(valores);
        }
    }
}