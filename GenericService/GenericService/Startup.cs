using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using GenericService.Models;
using GenericService.Repositories;
using GenericService.Repositories.Interfaces;
using GenericService.Services;
using GenericService.Services.Interfaces;
using GenericService.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace GenericService
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
            services.AddControllers();
            RegisterContactServices(services);

            RegisterSwagger(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // configure swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"); });

            ConfigureAutoMapper();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Setup Swagger to describe controller endpoints.
        /// </summary>
        /// <param name="services">Provided by Configure services startup method</param>
        protected virtual void RegisterSwagger(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                c.CustomSchemaIds(type => type.FullName); // avoids conflicts between same-named view and domain model

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });
        }

        protected virtual void RegisterContactDbContext(IServiceCollection services)
        {
            //services
            services.AddEntityFrameworkNpgsql().AddDbContext<ContactDbContext>(ServiceLifetime.Transient).BuildServiceProvider();
        }


        private void RegisterContactServices(IServiceCollection services)
        {
            RegisterContactDbContext(services);

            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();
        }

            public static void ConfigureAutoMapper()
        {
            Mapper.Reset();
            Mapper.Initialize(InitializeMapperConfig);
        }

        /// <summary>
        /// Split initialize Mapper to share map creation with unit tests. see ContactControllerTests.cs
        /// </summary>
        /// <param name="cfg"></param>
        public static void InitializeMapperConfig(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Contact, Contact>();
            cfg.CreateMap<ContactView, Contact>();
            cfg.CreateMap<Contact, ContactView>();
            cfg.CreateMap<Email, Email>();
            cfg.CreateMap<EmailView, Email>();
            cfg.CreateMap<Email, EmailView>();
        }
    }
}
