using Hangfire;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace HangFire_Workshop
{
    public static class HangFireJobsManager
    {

        public static MailjetSettings mailjetSettings = new MailjetSettings();

        public static void InitializeJobs(IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            InitializeInstantJobs(backgroundJobClient);
            InitializeScheduledJobs(backgroundJobClient);
            InitializeRecurrentJobs(recurringJobManager);
        }

        //Para ejecutar el job instantaneamente
        public static void InitializeInstantJobs(IBackgroundJobClient backgroundJobClient)
        {
            backgroundJobClient.Enqueue(() => EnviarMail("fede_albertengo@hotmail.com", "Federico Albertengo", "HangFire Workshop - Instant job test", "<h3>This is a test for the HangFire Workshop.</h3><br><br>Currently we are testing the instant job.<br>Unique Id for test: " + Guid.NewGuid().ToString() + "<br>This job was launched at: "));
        }

        //Para ejecutar el job a cierta fecha y hora
        public static void InitializeScheduledJobs(IBackgroundJobClient backgroundJobClient)
        {
            TimeSpan delay = TimeSpan.FromSeconds(30);
            DateTime dateTime = DateTime.Now.AddSeconds(delay.Seconds);
            backgroundJobClient.Schedule(() => EnviarMail("fede_albertengo@hotmail.com", "Federico Albertengo", "HangFire Workshop - Delayed job test", "<h3>This is a test for the HangFire Workshop.</h3><br><br>Currently we are testing the delayed job, having to sent at: " + dateTime.ToString() + ".<br>Unique Id for test: " + Guid.NewGuid().ToString() + "<br>This job was launched at: "), delay);
        }

        //Para ejecutar tareas recurrentes, pasandole una expresion CRON
        public static void InitializeRecurrentJobs(IRecurringJobManager recurringJobManager)
        {
            recurringJobManager.AddOrUpdate("Tarea recurrente cada minuto", () => EnviarMail("fede_albertengo@hotmail.com", "Federico Albertengo", "HangFire Workshop - Recurrent job test", "<h3>This is a test for the HangFire Workshop.</h3><br><br>Currently we are testing the recurrent job.<br>Unique Id for test: " + Guid.NewGuid().ToString() + "<br>This job was launched at: "), Cron.Minutely);
        }

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
