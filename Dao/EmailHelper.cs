using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Net.Mail;
using System.IO;
using System.Threading;
using MimeKit;

namespace Dao
{
    class EmailHelper
    {
        private static string[] Scopes = { GmailService.Scope.GmailSend };
        private static string ApplicationName = "AMLSoftMail ";

        public static GmailService GetGmailService()
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes, "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            return new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public static void EnviarCorreo(string destinatario, string asunto, string mensaje)
        {
            try
            {
                var service = GetGmailService();

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("AMLSoft", "noreplyamlsoft@gmail.com"));
                emailMessage.To.Add(new MailboxAddress("", destinatario));
                emailMessage.Subject = asunto;

                emailMessage.Body = new TextPart("html")
                {
                    Text = mensaje
                };

                using (var memoryStream = new MemoryStream())
                {
                    emailMessage.WriteTo(memoryStream);
                    var rawMessage = Convert.ToBase64String(memoryStream.ToArray()).Replace("+", "-").Replace("/", "_").Replace("=", "");

                    Message gmailMessage = new Message { Raw = rawMessage };
                    service.Users.Messages.Send(gmailMessage, "me").Execute();
                }

                Console.WriteLine("Correo enviado con exito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar correo: " + ex.Message);
            }
        }
    }
}
