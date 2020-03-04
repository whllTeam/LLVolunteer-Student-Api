using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SchoolInfoManager.MQ.Publish;
using SchoolManager.Core.Interfaces;
using SchoolManager.Infrastructure.Database;
using SchoolManager.Infrastructure.Repositories;
using SchoolManager.Service;

namespace SchoolManager.API
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration,
            ILogger<Startup> logger,
            IHostingEnvironment env
        )
        {
            Configuration = configuration;
            Logger = logger;
            Env = env;
            IsApollo = Env.IsProduction();
        }

        public bool IsApollo { get; set; }
        public IConfiguration Configuration { get; }

        public ILogger Logger { get; }

        public IHostingEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services.AddDbContext<SchoolManagerContext>(option =>
                {
                    option.UseMySql(Configuration.GetConnectionString("Mysql"), builder =>
                        {
                            builder.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        });
                });

            // 注册 UserManagerRepository
            services.AddScoped<IUserManagerRepository, UserManagerRepository>();
            // 注册 schoolInfoService

            services.AddScoped<ISchoolInfoService, SchoolInfoService>();

            // 注册 MQ publish
            services.AddSingleton<SchoolUserPublish, SchoolUserPublish>();
            services.AddCors(options =>
            {
                options.AddPolicy("spa", config =>
                {
                    config
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders(new []{ "PreUrl", "LoginUrl", "LoginViewState" })  
                        .AllowAnyOrigin();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("spa");
            app.UseMvc();
        }
    }
}
