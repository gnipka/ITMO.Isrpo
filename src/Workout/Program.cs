using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using Serilog;
using Workout.Extensions;
using Workout.Filters;
using Workout.Middlewares;

namespace Workout
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Logging.ClearProviders();
            
            builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));
            
            builder.Services.AddMvc(options =>
                {
                    options.InputFormatters.RemoveType<SystemTextJsonInputFormatter>();
                    options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
                })
                .AddNewtonsoftJson(opts =>
                {
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opts.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
                })
                .AddXmlSerializerFormatters();
            
            builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("0.0.1", new OpenApiInfo
                    {
                        Version = "0.0.1",
                        Title = "Расписание тренировок",
                        Description = "Расписание тренировок (ASP.NET Core 7.0)",
                        Contact = new OpenApiContact()
                        {
                            Name = "Swagger Codegen Contributors",
                            Url = new Uri("https://github.com/swagger-api/swagger-codegen"),
                            Email = ""
                        },
                    });
                    c.CustomSchemaIds(type => type.FullName);
                    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), includeControllerXmlComments: true);
                    
                    c.OperationFilter<GeneratePathParamsValidationFilter>();
                });

            builder.Services.AddOpenTelemetry()
                .ConfigureResource(resources => resources.AddService("workout"))
                .WithTracing(tr => tr
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddConsoleExporter()
                    .AddZipkinExporter(o =>
                    {
                        o.Endpoint = new Uri("http://zipkin:9411");
                    }));
            
            builder.Services.AddMetrics();
            
            var app = builder.Build();
            
            app.UseMetricServer();
            app.UseMiddleware<ResponseMetricMiddleware>();
            app.UseHttpMetrics(); 
            app.UseRouting();
            app.UseSerilogRequestLogging();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/0.0.1/swagger.json", "Расписание тренировок");
            });
            
            app.UseSerilogRequestLogging();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });
            
            app.Run();
        }
    }
}
