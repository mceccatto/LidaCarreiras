using Renci.SshNet.Messages;
using System.Net;
using System.Net.Mail;

namespace LidaCarreiras.Funcoes
{
    public class EnviarEmail
    {
        public string DestinatarioEmail { get; set; }
        public string Assunto { get; set; }
        public string Conteudo { get; set; }

        public EnviarEmail(string destinatarioEmail, string assunto, string conteudo)
        {
            this.DestinatarioEmail = destinatarioEmail;
            this.Assunto = assunto;
            this.Conteudo = conteudo;
        }

        public static string Enviar(EnviarEmail dados)
        {
            string retorno = "sucesso";
            var remetente = new MailAddress("no-reply@codelabs.dev.br", "Lida Carreiras");
            var destinatario = new MailAddress(dados.DestinatarioEmail);
            string senha = "no@reply";
            string assunto = dados.Assunto;
            string conteudo = dados.Conteudo;
            var smtp = new SmtpClient
            {
                Host = "mail.codelabs.dev.br",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(remetente.Address, senha),
                Timeout = 10000
            };
            using (var mensagem = new MailMessage(remetente, destinatario)
            {
                Subject = assunto,
                IsBodyHtml = true,
                Body = conteudo
                
            })
            {
                try
                {
                    smtp.Send(mensagem);
                }
                catch (Exception e)
                {
                    retorno = e.Message;
                }
                return retorno;
            }
        }
    }
}