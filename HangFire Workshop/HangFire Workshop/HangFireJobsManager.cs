using Hangfire;
using System;

namespace HangFire_Workshop
{
    public static class HangFireJobsManager
    {

        public static void InitializeJobs(IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            InitializeInstantJobs(backgroundJobClient);
            InitializeScheduledJobs(backgroundJobClient);
            InitializeRecurrentJobs(recurringJobManager);
        }

        //Para ejecutar el job instantaneamente
        public static void InitializeInstantJobs(IBackgroundJobClient backgroundJobClient)
        {
            backgroundJobClient.Enqueue(() => EmailsManager.EnviarMail("hangfireworkshop@gmail.com", "Federico Albertengo", "HangFire Workshop - Instant job test", "<h3>This is a test for the HangFire Workshop.</h3><br><br>Currently we are testing the instant job.<br>Unique Id for test: " + Guid.NewGuid().ToString() + "<br>This job was launched at: "));
        }

        //Para ejecutar el job a cierta fecha y hora
        public static void InitializeScheduledJobs(IBackgroundJobClient backgroundJobClient)
        {
            TimeSpan delay = TimeSpan.FromSeconds(30);
            DateTime dateTime = DateTime.Now.AddSeconds(delay.Seconds);
            backgroundJobClient.Schedule(() => EmailsManager.EnviarMail("hangfireworkshop@gmail.com", "Federico Albertengo", "HangFire Workshop - Delayed job test", "<h3>This is a test for the HangFire Workshop.</h3><br><br>Currently we are testing the delayed job, having to sent at: " + dateTime.ToString() + ".<br>Unique Id for test: " + Guid.NewGuid().ToString() + "<br>This job was launched at: "), delay);
        }

        //Para ejecutar tareas recurrentes, pasandole una expresion CRON
        public static void InitializeRecurrentJobs(IRecurringJobManager recurringJobManager)
        {
            recurringJobManager.AddOrUpdate("Tarea recurrente cada 15 minutos", () => EmailsManager.EnviarMail("hangfireworkshop@gmail.com", "Federico Albertengo", "HangFire Workshop - Recurrent job test", "<h3>This is a test for the HangFire Workshop.</h3><br><br>Currently we are testing the recurrent job.<br>Unique Id for test: " + Guid.NewGuid().ToString() + "<br>This job was launched at: "), "*/15 * * * *");
        }
    }
}
