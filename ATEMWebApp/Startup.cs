using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Models;
//using ATEM.Services.Services;
//using ATEM.Services.Hosts;
//using ATEM.Services.Hubs;
//using BMDSwitcherAPI;
using Atem.Hosts.Switcher;
using Atem.Hosts.Notifiers;
using Atem.Hosts.Services;
using System.IO;
using System.Reflection;
using System;

namespace ATEMWebApp
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

            services.AddControllersWithViews();

            //services.AddSignalR();

            //services.AddSingleton<IAtemService, AtemService>();
            //services.AddSingleton<ISwitcherHost, SwitcherHost>();
            //services.AddSingleton<AtemServicesConfiguration>(new AtemServicesConfiguration
            //{
            //    EnableEvents = true
            //});

            services.AddLogging();
            services.AddSingleton<ISwitcherHost, SwitcherHost>();
            services.AddSingleton<ISwitcherNotifier, LoggerSwitcherNotifer>();
            services.AddSingleton<ISwitcherService, SwitcherService>();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blackmagic Design Switchers Web API (v8.6.4 SDK)", Version = "v1" });
                
                // Adds XML documentation from method comments
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "api",
                    pattern: "api/{controller}/{action=Index}/{id?}");
                //endpoints.MapHub<ATEMEventsHub>("/events");
                endpoints.MapSwagger();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Blackmagic Design Switchers Web API v1 (v8.6.4 SDK)");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
