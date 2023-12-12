using System;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;

namespace HangFire_Workshop
{
    public static class EmailsManager
    {
        public static MailjetSettings mailjetSettings = new MailjetSettings();

        public static async Task EnviarMail(string email, string nombre, string subject, string body)
        {
            DateTime launchedDateTime = DateTime.Now;
            MailjetClient client = new MailjetClient(mailjetSettings.MailApiKey, mailjetSettings.MailApiSecret);
            MailjetRequest request = new MailjetRequest
            {
                Resource = SendV31.Resource
            }.Property(
                Send.Messages,
                new JArray {
                        new JObject {
                            {
                                "From",
                                new JObject {
                                    { "Email", mailjetSettings.MailMessageFromEmail },
                                    { "Name", mailjetSettings.MailMessageFromName }
                                }
                            },
                            {
                                "To",
                                new JArray {
                                    new JObject {
                                        { "Email", email },
                                        { "Name", nombre }
                                    }
                                }
                            },
                            {
                                "Subject",
                                subject },
                            // { "TextPart", "My first Mailjet email" },
                            {
                                "HTMLPart",
                                body + launchedDateTime.ToString()
                            },
                            {
                                "CustomID",
                                "MyClassesApp"
                            }
                        }
                });
            MailjetResponse response = await client.PostAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
        }
    }
}
