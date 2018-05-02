using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UserProfile.Api.Data;
using UserProfile.Api.Interfaces;
using UserProfile.Api.Models;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport;
using zipkin4net.Transport.Http;

namespace UserProfile.Api {
    public abstract class CommonStartup {
        // public CommonStartup (IConfiguration configuration) {
        //     Configuration = configuration;
        // }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddCors (options => {
                options.AddPolicy ("CorsPolicy",
                    builder => builder.AllowAnyOrigin ()
                    .AllowAnyMethod ()
                    .AllowAnyOrigin ()
                    .AllowCredentials ());
            });

            services.AddMvc ();

            services.Configure<Settings> (options => {
                options.ConnectionString = Configuration.GetSection ("MongoConnection:ConnectionString").Value;
                options.Database = Configuration.GetSection ("MongoConnection:Database").Value;
            });

            services.AddTransient<IUserDetailsRepository, UserDetailsRepository> ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {

            var config = ConfigureSettings.CreateConfiguration ();

            loggerFactory.AddConsole ();

            app.UseCors ("CorsPolicy");
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime> ();
            IStatistics statistics = new Statistics();
            
            lifetime.ApplicationStarted.Register (() => {
                TraceManager.SamplingRate = 1.0f;
                var logger = new TracingLogger (loggerFactory, "zipkin4net");
                var httpSender = new HttpZipkinSender ("http://localhost:9411", "application/json");
                var tracer = new ZipkinTracer (httpSender, new JSONSpanSerializer (), statistics);
                TraceManager.RegisterTracer (tracer);
                TraceManager.Start (logger);
            });

            lifetime.ApplicationStopped.Register (() => TraceManager.Stop ());
            app.UseTracing ("userdetailsService");
           // Run (app, config);
        }
       // abstract protected void Run (IApplicationBuilder app, IConfiguration configuration);
    }
}