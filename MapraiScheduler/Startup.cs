using Hangfire;
using MapraiScheduler.Notifier;
using MapraiScheduler.Repositories;
using MapraiScheduler.TaskManager.BackgroundTasks;
using MapraiScheduler.TaskManager.Commands;
using MapraiScheduler.TaskManager.Commands.Action;
using MapraiScheduler.TaskManager.Commands.ReportCommands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Email;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace MapraiScheduler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //AddConfigurationFromFile();
            services.AddMvc();
            //for mysql database see
            //https://github.com/stulzq/Hangfire.MySql.Core
            //default
            //https://www.c-sharpcorner.com/article/schedule-background-jobs-using-hangfire-in-asp-net-core/
            //the database should be created first

            //logger
            //https://stackoverflow.com/questions/46942106/trying-to-configure-serilog-email-sink-with-appsettings-json-to-work-with-gmail
            Log.Logger = new LoggerConfiguration().WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .WriteTo.Email(new EmailConnectionInfo
                {
                    FromEmail = NotifySetting.EmailStatics.FromEmailAddress,
                    ToEmail = "miladbonak@gmail.com",
                    MailServer = "smtp.gmail.com",
                    NetworkCredentials = new NetworkCredential
                    {
                        UserName = NotifySetting.EmailStatics.FromEmailAddress,
                        Password = NotifySetting.EmailStatics.FromEmailPassword
                    },
                    EnableSsl = true,
                    Port = 465,
                    EmailSubject = NotifySetting.EmailStatics.SerlogEmailSubject
                },
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}",
            batchPostingLimit: 10
            , restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error)
            .CreateLogger();

            ManageDependencyInjections(services);
        }

        private void ManageDependencyInjections(IServiceCollection services)
        {
            services.AddHangfire(options => options.UseSqlServerStorage(Configuration["HangFireConnectionString"]));
            //Statics.MySQLConnectionString = ;
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddDbContext<MapRaiContex>(x => x.UseMySQL(Configuration["AppConnectionString"]));
            //tasks
            services.AddScoped<IValidateReportsBackgroundTask, ValidateReportsBackgroundTask>();
            services.AddScoped<IValidateProjectBackgroundTask, ValidateProjectBackgroundTask>();
            //commands
            services.AddScoped<ICheckAutoStopProject, CheckAutoStopProject>();
            services.AddScoped<ICheckOutOfTimeCommand, CheckOutOfTimeCommand>();
            services.AddScoped<ICheckVeryLateProject, CheckVeryLateProject>();
            services.AddScoped<ICheckProjectUserActivity, CheckProjectUserActivity>();
            services.AddScoped<ICheckDamageReports, CheckDamageReports>();
            services.AddScoped<ICheckProjectReports, CheckProjectReports>();
            services.AddScoped<ILogCommand, LogCommand>();
            //Actions
            services.AddScoped<IStopProjectsAction, StopProjectsAction>();
            //Repositories
            services.AddScoped<INotifyRepository, NotifyRepository>();
            services.AddScoped<INotifyTypeRepository, NotifyTypeRepository>();
            services.AddScoped<IPhoneRepository, PhoneRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectReportRepository, ProjectReportRepository>();
            services.AddScoped<IDamageReportRepository, DamageReportRepository>();
            //notifiers
            services.AddScoped<IEmailNotifier, EmailNotifier>();
            services.AddScoped<ISmsNotifier, SmsNotifier>();
            services.AddScoped<IAppNotifier, AppNotifier>();
            //logger
            services.AddSingleton<ILogger>(Log.Logger);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IServiceProvider provider, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            //http://docs.hangfire.io/en/latest/quick-start.html
            app.UseHangfireDashboard();
            app.UseHangfireServer();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });

            (provider.CreateInstance<TaskManager.TaskManager>()).StartBackgroundTasks();
        }

        //private void AddConfigurationFromFile()
        //{
        //    //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.1&tabs=basicconfiguration
        //    var builder = new ConfigurationBuilder()
        //    .SetBasePath(Directory.GetCurrentDirectory())
        //    .AddJsonFile("appsettings.json");
        //    Configuration = builder.Build();
        //}
    }

    public static class ServiceProviderExtensions
    {
        public static TResult CreateInstance<TResult>(this IServiceProvider provider) where TResult : class
        {
            ConstructorInfo constructor = typeof(TResult).GetConstructors()[0];

            if (constructor == null) return null;
            object[] args = constructor
                .GetParameters()
                .Select(o => o.ParameterType)
                .Select(provider.GetService)
                .ToArray();

            return Activator.CreateInstance(typeof(TResult), args) as TResult;
        }
    }
}