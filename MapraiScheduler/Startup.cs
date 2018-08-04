using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hangfire;
using MapraiScheduler.Notifier;
using MapraiScheduler.Repositories;
using MapraiScheduler.TaskManager;
using MapraiScheduler.TaskManager.BackgroundTasks;
using MapraiScheduler.TaskManager.Commands;
using MapraiScheduler.TaskManager.Commands.Action;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MapraiScheduler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AddConfigurationFromFile();
            services.AddMvc();
            //for mysql database see
            //https://github.com/stulzq/Hangfire.MySql.Core
            //default
            //https://www.c-sharpcorner.com/article/schedule-background-jobs-using-hangfire-in-asp-net-core/
            //the database should be created first

            services.AddHangfire(options => options.UseSqlServerStorage(Configuration["HangFireConnectionString"]));
            //Statics.MySQLConnectionString = ;
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddDbContext<MapRaiContex>(x => x.UseMySQL(Configuration["AppConnectionString"]));
            //tasks
            services.AddScoped<IValidateAssessmentBackgroundTask, ValidateAssessmentBackgroundTask>();
            services.AddScoped<IValidateProjectBackgroundTask, ValidateProjectBackgroundTask>();
            //commands
            services.AddScoped<ICheckAutoStopProject, CheckAutoStopProject>();
            services.AddScoped<ICheckOutOfTimeCommand, CheckOutOfTimeCommand>();
            services.AddScoped<ICheckVeryLateProject, CheckVeryLateProject>();
            services.AddScoped<ICheckProjectUserActivity, CheckProjectUserActivity>();
            services.AddScoped<ILogCommand, LogCommand>();
            //Actions
            services.AddScoped<IStopProjectsAction, StopProjectsAction>();
            //Repositories
            services.AddScoped<INotifyRepository, NotifyRepository>();
            services.AddScoped<INotifyTypeRepository, NotifyTypeRepository>();
            services.AddScoped<IPhoneRepository, PhoneRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            //notifiers
            services.AddScoped<IEmailNotifier, EmailNotifier>();
            services.AddScoped<ISmsNotifier, SmsNotifier>();
            services.AddScoped<IAppNotifier, AppNotifier>();
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            //http://docs.hangfire.io/en/latest/quick-start.html
            app.UseHangfireDashboard();
            app.UseHangfireServer();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            (provider.CreateInstance<TaskManager.TaskManager>()).StartBackgroundTasks();
        }

        private void AddConfigurationFromFile()
        {
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.1&tabs=basicconfiguration
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }
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