namespace LidaCarreiras.Funcoes
{
    public class SenhaTemp
    {
        public static string Gerar()
        {
            string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string senha = "";
            Random randomico = new Random();
            for(int i = 0; i < 6; i++)
            {
                senha += caracteres.Substring(randomico.Next(0, caracteres.Length - 1), 1);
            }
            return senha;
        }
    }
}