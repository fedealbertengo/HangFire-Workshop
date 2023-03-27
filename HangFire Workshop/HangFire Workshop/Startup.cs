using Hangfire;
using HangFire_Workshop.Data;
using HangFire_Workshop.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace HangFire_Workshop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureHangFire(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHangfireDashboard();
            });

            app.UseHangfireDashboard();

            //Tareas programadas
            backgroundJobClient.Enqueue(() => Console.WriteLine("Holaa")); //Para ejecutar el job instantaneamente
            backgroundJobClient.Schedule(() => Console.WriteLine("Saludos, con delay"), TimeSpan.FromSeconds(30)); //Para ejecutar el job a cierta fecha y hora

            //Tareas recurrentes
            recurringJobManager.AddOrUpdate("Tarea recurrente cada minuto", () => Console.WriteLine("Se ejecuto la tarea recurrente."), Cron.Minutely); //Para ejecutar una tarea recurrente
        }
    }
}
